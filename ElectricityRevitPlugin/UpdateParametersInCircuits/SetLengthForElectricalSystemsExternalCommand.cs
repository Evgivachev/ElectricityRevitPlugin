using System;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using ElectricityRevitPlugin.Annotations;
using MoreLinq;

namespace ElectricityRevitPlugin.UpdateParametersInCircuits
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SetLengthForElectricalSystemsExternalCommand : DefaultExternalCommand, IUpdaterParameters<ElectricalSystem>
    {
        //Длина цепи до ближайшего устройства
        readonly Guid _lengthToNearestDeviceGuid = new Guid("1be3a6d5-8647-4044-be28-833b56f39086");
        //Длина до наиболее удаленного устройства
        readonly Guid _lengthToMostRemoteDeviceGuid = new Guid("541bbad9-d80d-48b1-b0b5-1460a13a8d66");
        //Длина через все устройства
        readonly Guid _lengthThrowAllDevicesGuid = new Guid("43387d31-d9b2-4374-916d-69ce7cec588f");
        //Запретить изменение
        readonly Guid _isUnEditable = new Guid("be64f474-c030-40cf-9975-6eaebe087a84");
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            using (var tr = new Transaction(Doc))
            {
                tr.Start("Установка длин кабелей и труб для электрических цепей");
                var electricalSystems = new FilteredElementCollector(Doc)
                    .OfClass(typeof(ElectricalSystem))
                    .WhereElementIsNotElementType()
                    .OfType<ElectricalSystem>();
                foreach (var el in electricalSystems)
                {
                    var isUneditable = el.get_Parameter(_isUnEditable).AsInteger() == 1;
                    if (isUneditable)
                        continue;
                    SetLengthElectricalSystems(el);
                }
                tr.Commit();
            }
            return Result.Succeeded;
        }

        private string SetLengthElectricalSystems(ElectricalSystem el)
        {
            var name = el.Name;
            //Ключевая спецификация способ расчета длины
            var calculateLengthType = el.LookupParameter("Способ расчета длины").AsValueString();
            var k = el.LookupParameter("Коэффициент для длины электрической цепи").AsDouble();

            var lengthToNearestDeviceParameter = el.get_Parameter(_lengthToNearestDeviceGuid);
            var lengthToMostRemoteDeviceParameter = el.get_Parameter(_lengthToMostRemoteDeviceGuid);
            var lengthThrowAllDeviceParameter = el.get_Parameter(_lengthThrowAllDevicesGuid);

            var isCalculated = CalculateLengthsOfElSystem(el, out var lengthToNearestDevice, out var lengthToMostRemote, out var lengthTrowAllDevice);

            var q = new[]
            {
                lengthToNearestDeviceParameter.Set(lengthToNearestDevice),
                lengthToMostRemoteDeviceParameter.Set(lengthToMostRemote),
                lengthThrowAllDeviceParameter.Set(lengthTrowAllDevice)
            };

            SetLengthOfCableForDiagrams(el, calculateLengthType, k);

            var numberOfCables = el.LookupParameter("Кол-во кабелей (провод) в одной группе").AsDouble();
            //Длина кабелей для ОС
            //число в метрах
            var lengthForDiagrams = el.get_Parameter(new Guid("387ba243-768e-45cf-9c22-ce1b5650fe3d"));
            var lengthForDiagramsDouble = el.get_Parameter(new Guid("387ba243-768e-45cf-9c22-ce1b5650fe3d")).AsDouble();
            //длина в миллиметрах
            var storeLengthForTube = UnitUtils.ConvertFromInternalUnits(el.get_Parameter(new Guid("25122ee0-d761-4a5f-af49-b507b64188e3")).AsDouble(), DisplayUnitType.DUT_METERS);
            //длина в миллиметрах
            var tubeLengthParam = el.LookupParameter("Длина труб для спецификации");
            var value = Math.Max(0, numberOfCables * (lengthForDiagramsDouble + storeLengthForTube));
            tubeLengthParam.Set(UnitUtils.ConvertToInternalUnits(value, DisplayUnitType.DUT_METERS));

            //
            return null;
        }

        private bool SetLengthOfCableForDiagrams(ElectricalSystem el, string type, double k = 0)
        {
            //Doc.Regenerate();
            var numberOfCables = el.LookupParameter("Кол-во кабелей (провод) в одной группе").AsDouble();
            var lengthForDiagramsParameter = el.get_Parameter(new Guid("387ba243-768e-45cf-9c22-ce1b5650fe3d"));
            var lengthForDiagrams = el.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_LENGTH_PARAM).AsDouble();
            if (type == "(нет)")
            {
                //Остается так же
            }
            else if (type.StartsWith("*"))
            {
                lengthForDiagrams *= k;
            }
            else if (type == "Через все элементы")
            {
                lengthForDiagrams = el.get_Parameter(_lengthThrowAllDevicesGuid).AsDouble();
            }
            else if (type == "До наиболее удаленного устройства")
            {
                lengthForDiagrams = el.get_Parameter(_lengthToMostRemoteDeviceGuid).AsDouble();
            }
            //Длина кабелей для ОС
            //число в метрах
            lengthForDiagramsParameter.Set(UnitUtils.ConvertFromInternalUnits(lengthForDiagrams, DisplayUnitType.DUT_METERS));
            //длина в миллиметрах
            var storeLengthForTube = el.get_Parameter(new Guid("25122ee0-d761-4a5f-af49-b507b64188e3")).AsDouble();
            //длина в миллиметрах
            var tubeLengthParam = el.LookupParameter("Длина труб для спецификации");
            var value = Math.Max(0, numberOfCables * (lengthForDiagrams + storeLengthForTube));
            tubeLengthParam.Set(value);
            return true;
        }

        private bool CalculateLengthsOfElSystem(ElectricalSystem el, out double lengthToNearestDevice, out double lengthToMostRemoteDevice, out double lengthThrowAllDevice)
        {
            //TODO изменить координаты точки на координаты электрического соединителя
            var path = el.GetCircuitPath();
            var devices = el
                .Elements
                .Cast<Element>()
                .ToDictionary(x => x.Id);


            var baseDevice = el.BaseEquipment;
            lengthToNearestDevice = 0;
            lengthToMostRemoteDevice = 0;
            lengthThrowAllDevice = 0;
            if (baseDevice is null)
                return true;
            var baseDeviceLocation = baseDevice.Location as LocationPoint;
            var baseDevicePoint = baseDeviceLocation.Point;


            foreach (var pair in devices)
            {
                var device = pair.Value;
                var location = device.Location as LocationPoint;
                var point = location.Point;
                var distanceToBaseDeviceVector = (point - baseDevicePoint);
                var distanceToBaseDevice = Math.Abs(distanceToBaseDeviceVector.X) + Math.Abs(distanceToBaseDeviceVector.Y) +
                                                 Math.Abs(distanceToBaseDeviceVector.Z);
                lengthToNearestDevice = lengthToNearestDevice > 0
                    ? Math.Min(lengthToNearestDevice, distanceToBaseDevice)
                    : distanceToBaseDevice;
                lengthToMostRemoteDevice = Math.Max(lengthToMostRemoteDevice, distanceToBaseDevice);
            }

            //Расчет длины через все устройства
            var lastPoint = baseDevicePoint;
            while (devices.Any())
            {
                var nearest = devices.Select(x => x.Value)
                    .Select(x =>
                    {
                        var lp = x.Location as LocationPoint;
                        var p = lp.Point;
                        var v = lastPoint - p;
                        var d = Math.Abs(v.X) + Math.Abs(v.Y) + Math.Abs(v.Z);
                        return Tuple.Create(x, d, p);
                    })
                    .MinBy(x => x.Item2)
                    .FirstOrDefault();
                lastPoint = nearest.Item3;
                lengthThrowAllDevice += nearest.Item2;
                devices.Remove(nearest.Item1.Id);
            }

            return true;

        }

        public string UpdateParameters(ElectricalSystem el)
        {
            return SetLengthElectricalSystems(el);
        }
    }
}
