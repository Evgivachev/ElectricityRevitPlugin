using System;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using MoreLinq;

namespace ElectricityRevitPlugin.UpdateParametersInCircuits
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SetLengthForElectricalSystemsExternalCommand : IExternalCommand, IUpdaterParameters<ElectricalSystem>
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var doc = uiDoc.Document;
            var app = uiApp.Application;
            var result = Result.Succeeded;
            try
            {
                using (var tr = new Transaction(doc))
                {
                    tr.Start("Установка длин кабелей и труб для электрических цепей");
                    var electricalSystems = new FilteredElementCollector(doc)
                        .OfClass(typeof(ElectricalSystem))
                        .WhereElementIsNotElementType()
                        .OfType<ElectricalSystem>();
                    foreach (var el in electricalSystems)
                        SetLengthElectricalSystems(el);

                    tr.Commit();
                }
            }
            catch (Exception e)
            {
                message += e.Message + '\n' + e.StackTrace;
                result = Result.Failed;
            }
            finally
            {

            }
            return result;
        }

        private string SetLengthElectricalSystems(ElectricalSystem el)
        {
            var name = el.Name;
            //Длина цепи до ближайшего устройства
            var lengthToNearestDeviceGuid = new Guid("1be3a6d5-8647-4044-be28-833b56f39086");
            //Длина до наиболее удаленного устройства
            var lengthToMostRemoteDeviceGuid = new Guid("541bbad9-d80d-48b1-b0b5-1460a13a8d66");
            //Длина через все устройства
            var lengthThrowAllDevicesGuid = new Guid("43387d31-d9b2-4374-916d-69ce7cec588f");
            //Ключевая спецификация способ расчета длины
            var calculateLengthType = el.LookupParameter("Способ расчета длины").AsValueString();

            var lengthToNearestDeviceParameter = el.get_Parameter(lengthToNearestDeviceGuid);
            var lengthToMostRemoteDeviceParameter = el.get_Parameter(lengthToMostRemoteDeviceGuid);
            var lengthThrowAllDeviceParameter = el.get_Parameter(lengthThrowAllDevicesGuid);

            var isCalculated = CalculateLengthsOfElSystem(el, out var lengthToNearestDevice, out var lengthToMostRemote, out var lengthTrowAllDevice);


            var q = new[]
            {
                lengthToNearestDeviceParameter.Set(lengthToNearestDevice),
                lengthToMostRemoteDeviceParameter.Set(lengthToMostRemote),
                lengthThrowAllDeviceParameter.Set(lengthTrowAllDevice)
            };

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

        private bool CalculateLengthsOfElSystem(ElectricalSystem el, out double lengthToNearestDevice, out double lengthToMostRemoteDevice, out double lengthThrowAllDevice)
        {
            //TODO изменить координаты точки на координаты электрического соединителя
            var path = el.GetCircuitPath();
            var devices = el
                .Elements
                .Cast<Element>()
                .ToDictionary(x => x.Id);


            var baseDevice = el.BaseEquipment;
            var baseDeviceLocation = baseDevice.Location as LocationPoint;
            var baseDevicePoint = baseDeviceLocation.Point;

            lengthToNearestDevice = 0;
            lengthToMostRemoteDevice = 0;
            lengthThrowAllDevice = 0;
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
                        return Tuple.Create(x, d,p);
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
