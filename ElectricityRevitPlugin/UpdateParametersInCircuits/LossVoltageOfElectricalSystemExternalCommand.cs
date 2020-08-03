using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using ElectricityRevitPlugin.Extensions;
using MoreLinq;

namespace ElectricityRevitPlugin.UpdateParametersInCircuits
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class LossVoltageOfElectricalSystemExternalCommand : DefaultExternalCommand,
        IUpdaterParameters<ElectricalSystem>
    {

        private readonly Guid _lossVoltageParameterGuid = new Guid("b4954a6d-3d42-44ff-b700-e308cf0fcc46");
        private readonly double _tolerance = 1e-8;
        private readonly Guid _disableChangeGuid = new Guid("be64f474-c030-40cf-9975-6eaebe087a84");
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            var allElectricalSystems = new FilteredElementCollector(Doc)
                .OfCategory(BuiltInCategory.OST_ElectricalCircuit)
                .Cast<ElectricalSystem>()
                //.Where(el=>el.Id.IntegerValue == 24431899)
                //.Where(el=>el?.BaseEquipment?.Name =="ЩС-0")

                ;

            using (var tr = new Transaction(Doc, "Расчет потерь напряжения в цепях"))
            {
                tr.Start();
                foreach (var electricalSystem in allElectricalSystems)
                {
                    try
                    {
                        var isDisableChange = electricalSystem.get_Parameter(_disableChangeGuid)?.AsInteger() == 1;
                        if (isDisableChange)
                            continue;
                        //TODO добавить выбор метода расчета 
                        var resultMessage = UpdateParameters(electricalSystem);
                    }
                    catch (Exception e)
                    {
                        message += '\n';
                        message += e.Message;
                        message += electricalSystem.Id.ToString();
                        return Result.Failed;
                    }

                }

                tr.Commit();
            }
            return Result.Succeeded;
        }

        public string UpdateParameters(ElectricalSystem el)
        {
            var devices = el
                .Elements
                .Cast<Element>()
                .ToDictionary(element => element.Id);

            var baseDevice = el.BaseEquipment ?? devices.First().Value as FamilyInstance;
            var baseDeviceLocation = baseDevice.Location as LocationPoint;
            var baseDevicePoint = baseDeviceLocation.Point;
            var lastPoint = baseDevicePoint;
            //Расчет потерь напряжения
            //Количество фаз
            var polesNumber = el.get_Parameter(BuiltInParameter.RBS_ELEC_NUMBER_OF_POLES).AsInteger();
            //Напряжение
            var voltage = UnitUtils.ConvertFromInternalUnits(el.get_Parameter(BuiltInParameter.RBS_ELEC_VOLTAGE).AsDouble(), DisplayUnitType.DUT_VOLTS);
            //Сечение кабеля
            var crossSection = el.LookupParameter("Сечение кабеля").AsDouble();
            //r Перевод сопротивления на 1м, не на км
            var r = el.LookupParameter("Активное сопротивление").AsDouble() / 1000;
            //x
            var x = el.LookupParameter("Индуктивное сопротивление").AsDouble() / 1000;
            //Количество параллельных кабелей
            var n = el.LookupParameter("Кол-во кабелей (провод) в одной группе").AsDouble();
            //Активная нагрузка
            var activePower = UnitUtils.ConvertFromInternalUnits(el.get_Parameter(BuiltInParameter.RBS_ELEC_TRUE_LOAD).AsDouble(), DisplayUnitType.DUT_WATTS);

            //cos F
            var cosPhi = el.get_Parameter(BuiltInParameter.RBS_ELEC_POWER_FACTOR).AsDouble();
            var tgPhi = Math.Sqrt(1 / cosPhi / cosPhi - 1);
            //Q
            var reactivePower = activePower * tgPhi;

            //Общая потеря напряжения в сети
            var du0 = 0.0;
            var previousL = 0.0;
            while (devices.Any())
            {
                var nearest = devices.Select(pair => pair.Value)
                    .Select(element =>
                    {
                        var lp = element.Location as LocationPoint;
                        var p = lp.Point;
                        var v = lastPoint - p;
                        var d = Math.Abs(v.X) + Math.Abs(v.Y) + Math.Abs(v.Z);
                        return Tuple.Create(element, d, p);
                    })
                    .MinBy(tuple => tuple.Item2)
                    .FirstOrDefault();
                lastPoint = nearest.Item3;
                var fi = nearest.Item1 as FamilyInstance;
                devices.Remove(fi.Id);
                //Расчет потерь напряжения
                var l = previousL + UnitUtils.ConvertFromInternalUnits(nearest.Item2, DisplayUnitType.DUT_METERS);
                var du = CalculateLossVoltage(activePower, reactivePower, r, x, l, voltage, n, polesNumber);
                //Мощность приемника
                fi.GetElectricalParameters(out var activePowerFi, out var powerFactorFi);
                if (activePower < _tolerance || powerFactorFi < _tolerance)
                    continue;
                previousL = 0;
                l = 0;
                var tgPhiFi = Math.Sqrt(1 / powerFactorFi / powerFactorFi - 1);
                var reactivePowerFi = activePowerFi * tgPhiFi;


                activePower -= activePowerFi;
                reactivePower -= reactivePowerFi;
                du0 += du;
            }
            //Параметр имеет тип string
            var lossVoltageParameter = el.get_Parameter(_lossVoltageParameterGuid);
            lossVoltageParameter = el.LookupParameter("Тестовый");


            du0 = du0 / voltage * 100;
            if (!lossVoltageParameter.IsReadOnly)
            {
                var flag = lossVoltageParameter.Set(du0.ToString(CultureInfo.InvariantCulture));
            }
            return null;
        }

        private double CalculateLossVoltage(double p, double q, double r, double x, double l, double u0, double n, int polesNumber)
        {
            var du = (polesNumber == 1 ? 2 : 1) * l * (p * r + q * x) / u0 / n;
            return du;
        }
    }
}
