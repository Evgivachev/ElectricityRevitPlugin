namespace CommonUpdateCmd.Infrastructure.UpdateElectricalSystem;

using System;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using CommonUtils.Helpers;
using MoreLinq;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class SetLengthForElectricalSystemsExternalCommand : IUpdaterParameters<ElectricalSystem>
{
    //Запретить изменение
    private readonly Guid _isUnEditable = new("be64f474-c030-40cf-9975-6eaebe087a84");

    //Длина через все устройства
    private readonly Guid _lengthThrowAllDevicesGuid = new("43387d31-d9b2-4374-916d-69ce7cec588f");

    //Длина через все элементы со смещением
    private readonly Guid _lengthThrowAllDevicesWithShiftGuid = new("48cb22b9-5f47-4d45-978d-4a5749462053");

    //Длина до наиболее удаленного устройства
    private readonly Guid _lengthToMostRemoteDeviceGuid = new("541bbad9-d80d-48b1-b0b5-1460a13a8d66");

    //Длина цепи до ближайшего устройства
    private readonly Guid _lengthToNearestDeviceGuid = new("1be3a6d5-8647-4044-be28-833b56f39086");

    //смещение для электрической цепи
    private readonly Guid _shiftForElectricalCircuit = new("8598960d-1bdb-4e68-82e0-f5202c76bce1");

    public string UpdateParameters(ElectricalSystem el)
    {
        return SetLengthElectricalSystems(el);
    }

    private string SetLengthElectricalSystems(ElectricalSystem el)
    {
        //Ключевая спецификация способ расчета длины
        var calculateLengthType = el.LookupParameter("Способ расчета длины")?.AsValueString();
        calculateLengthType = calculateLengthType ?? "(нет)";
        var k = el.LookupParameter("Коэффициент для длины электрической цепи")?.AsDouble() ?? 0.0;
        var lengthToNearestDeviceParameter = el.get_Parameter(_lengthToNearestDeviceGuid);
        var lengthToMostRemoteDeviceParameter = el.get_Parameter(_lengthToMostRemoteDeviceGuid);
        var lengthThrowAllDeviceParameter = el.get_Parameter(_lengthThrowAllDevicesGuid);
        var lengthTrowAllDevicesWithShiftParameter = el.get_Parameter(_lengthThrowAllDevicesWithShiftGuid);
        var shiftForElectricalCircuits = el.get_Parameter(_shiftForElectricalCircuit).AsDouble();
        CalculateLengthsOfElSystem(el,
            shiftForElectricalCircuits,
            out var lengthToNearestDevice,
            out var lengthToMostRemote,
            out var lengthTrowAllDevice,
            out var lengthThrowAllDevicesWithShift);
        _ = new[]
        {
            !lengthToNearestDeviceParameter.IsReadOnly && lengthToNearestDeviceParameter.Set(lengthToNearestDevice),
            !lengthToMostRemoteDeviceParameter.IsReadOnly && lengthToMostRemoteDeviceParameter.Set(lengthToMostRemote),
            !lengthThrowAllDeviceParameter.IsReadOnly && lengthThrowAllDeviceParameter.Set(lengthTrowAllDevice),
            !lengthTrowAllDevicesWithShiftParameter.IsReadOnly &&
            lengthTrowAllDevicesWithShiftParameter.Set(lengthThrowAllDevicesWithShift)
        };
        SetLengthOfCableForDiagrams(el, calculateLengthType, k);
        //
        return null;
    }

    private void SetLengthOfCableForDiagrams(ElectricalSystem el, string type, double k = 0)
    {
        var doc = el.Document;
        doc.Regenerate();
        var numberOfCables = el.LookupParameter("Кол-во кабелей (провод) в одной группе").AsDouble();
        var lengthForDiagramsParameter = el.get_Parameter(new Guid("387ba243-768e-45cf-9c22-ce1b5650fe3d"));
        var lengthForDiagrams = el.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_LENGTH_PARAM).AsDouble();
        var isReserveGroupGuid = new Guid("cd2dc469-276a-40f4-bd34-c6ab2ae05348");
        var isReserveGroup = el.get_Parameter(isReserveGroupGuid).AsInteger() == 1;
        var isControlGroup = el.get_Parameter(SharedParametersFile.Kontrolnye_TSepi).AsInteger() == 1;
        if (isReserveGroup || isControlGroup)
            lengthForDiagrams = 0;
        else if (type == "(нет)")
        {
            //Остается длина
        }
        else if (type.StartsWith("*"))
            lengthForDiagrams *= k;
        else if (type == "=Через все элементы")
            lengthForDiagrams = el.get_Parameter(_lengthThrowAllDevicesGuid).AsDouble();
        else if (type == "=До наиболее удаленного устройства")
            lengthForDiagrams = el.get_Parameter(_lengthToMostRemoteDeviceGuid).AsDouble();
        else if (type == "Через все элементы со смещением")
            lengthForDiagrams = el.get_Parameter(_lengthThrowAllDevicesWithShiftGuid).AsDouble();

        //Длина кабелей для ОС
        if (!lengthForDiagramsParameter.IsReadOnly)
            lengthForDiagramsParameter.Set(lengthForDiagrams);
        //длина в миллиметрах
        var storeLengthForTube = el.get_Parameter(new Guid("25122ee0-d761-4a5f-af49-b507b64188e3")).AsDouble();
        //длина в миллиметрах
        var tubeLengthParam = el.LookupParameter("Длина труб для спецификации");
        var value = Math.Max(0, numberOfCables * (lengthForDiagrams + storeLengthForTube));
        tubeLengthParam.Set(value);
    }

    private void CalculateLengthsOfElSystem(
        ElectricalSystem el,
        double shift,
        out double lengthToNearestDevice,
        out double lengthToMostRemoteDevice,
        out double lengthThrowAllDevice,
        out double lengthThrowAllDevicesWithShift)
    {
        el.GetCircuitPath();
        var devices = el
            .Elements
            .Cast<Element>()
            .ToDictionary(element => element.Id);
        var baseDevice = el.BaseEquipment;
        lengthToNearestDevice = 0;
        lengthToMostRemoteDevice = 0;
        lengthThrowAllDevice = 0;
        lengthThrowAllDevicesWithShift = 0;
        if (baseDevice is null)
            return;
        //TODO взять точку семейства
        var baseDevicePoint = ((LocationPoint)baseDevice.Location).Point;
        //GetCoordinateOfElectricalConnector(baseDevice);
        foreach (var pair in devices)
        {
            var device = (FamilyInstance)pair.Value;
            var electricalConnectorLocation = GetCoordinateOfElectricalConnector(device);
            var point = electricalConnectorLocation;
            var distanceToBaseDevice = CalculateDistanceBetweenPoints2(point, baseDevicePoint);

            //(point - baseDevicePoint);
            //var distanceToBaseDevice = Math.Abs(distanceToBaseDeviceVector.X) + Math.Abs(distanceToBaseDeviceVector.Y) +
            //                                 Math.Abs(distanceToBaseDeviceVector.Z);
            lengthToNearestDevice = lengthToNearestDevice > 0
                ? Math.Min(lengthToNearestDevice, distanceToBaseDevice)
                : distanceToBaseDevice;
            lengthToMostRemoteDevice = Math.Max(lengthToMostRemoteDevice, distanceToBaseDevice);
        }

        //Расчет длины через все устройства
        var lastPoint = baseDevicePoint;
        ////Расчет потерь напряжения
        ////Количество фаз
        //var polesNumber = el.get_Parameter(BuiltInParameter.RBS_ELEC_NUMBER_OF_POLES).AsInteger();
        ////Напряжение
        //var voltage = el.get_Parameter(BuiltInParameter.RBS_ELEC_VOLTAGE).AsDouble();
        ////Сечение кабеля
        //var crossSection = el.LookupParameter("Сечение кабеля").AsDouble();
        ////r
        //var r = el.LookupParameter("Активное сопротивление").AsDouble();
        ////x
        //var x = el.LookupParameter("Индуктивное сопротивление").AsDouble();
        ////Количество параллельных кабелей
        //var n = el.LookupParameter("Кол-во кабелей (провод) в одной группе").AsDouble();
        ////var P = el.
        while (devices.Any())
        {
            var point = lastPoint;
            var nearest = devices.Select(pair => pair.Value)
                .Select(element =>
                {
                    var p = GetCoordinateOfElectricalConnector((FamilyInstance)element);
                    var d = CalculateDistanceBetweenPoints2(point, p);
                    return Tuple.Create(element, d, p);
                })
                .MinBy(tuple => tuple.Item2)
                .FirstOrDefault();
            lastPoint = nearest.Item3;
            lengthThrowAllDevice += nearest.Item2;
            devices.Remove(nearest.Item1.Id);
        }

        //TODO считает с соединителями
        var connectedElementsCount = el.Elements.Size;
        lengthThrowAllDevicesWithShift = lengthThrowAllDevice + shift * connectedElementsCount;
    }

    private XYZ GetCoordinateOfElectricalConnector(FamilyInstance fi)
    {
        var connectorSet = fi
            .MEPModel
            .ConnectorManager
            .Connectors;
        //TODO что если в семействе несколько электрических соединителей
        var connector = connectorSet.Cast<Connector>().FirstOrDefault(x => x.ConnectorType is ConnectorType.End);
        var p = connector!
            .CoordinateSystem
            .Origin;
        return p;
    }


    /// <summary>
    /// Метрика Манхеттен
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <returns></returns>
    private double CalculateDistanceBetweenPoints2(XYZ point1, XYZ point2)
    {
        var dx = point2.X - point1.X;
        var dy = point2.Y - point1.Y;
        var dz = point2.Z - point1.Z;
        return Math.Abs(dx) + Math.Abs(dy) + Math.Abs(dz);
    }
}
