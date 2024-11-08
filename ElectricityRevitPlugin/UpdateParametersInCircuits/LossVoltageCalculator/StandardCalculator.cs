namespace ElectricityRevitPlugin.UpdateParametersInCircuits.LossVoltageCalculator;

using System;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using CommonUtils.Extensions;
using CommonUtils.Helpers;
using MoreLinq;

class StandardCalculator : LossVoltageCalculator

{
    private readonly double _tolerance = 1e-8;

    internal override double CalculateLossVoltage(ElectricalSystem el)
    {
        var devices = el
            .Elements
            .Cast<Element>()
            .ToDictionary(element => element.Id);
        Debug.Print($"Цепь {el.get_Parameter(SharedParametersFile.Nomer_Gruppy_Po_GOST).AsString()}");
        var baseDevice = el.BaseEquipment ?? devices.First().Value as FamilyInstance;
        Debug.Print($"shield is {baseDevice.Name}");
        var baseDeviceLocation = baseDevice.Location as LocationPoint;
        var baseDevicePoint = baseDeviceLocation.Point;
        Debug.Print($"Location {baseDevicePoint}");
        var lastPoint = baseDevicePoint;
        //Расчет потерь напряжения
        //Количество фаз
        var polesNumber = el.get_Parameter(BuiltInParameter.RBS_ELEC_NUMBER_OF_POLES).AsInteger();
        Debug.Print($"polesNumber {polesNumber}");
        //Напряжение
        var voltage = UnitUtils.ConvertFromInternalUnits(el.get_Parameter(BuiltInParameter.RBS_ELEC_VOLTAGE).AsDouble(),
            UnitTypeId.Volts);
        Debug.Print($"Voltage {voltage}");
        //Сечение кабеля
        el.LookupParameter("Сечение кабеля").AsDouble();
        //r Перевод сопротивления на 1м, не на км
        var r = el.LookupParameter("Активное сопротивление").AsDouble() / 1000;
        Debug.Print($"r, Ом/м {r}");
        //x
        var x = el.LookupParameter("Индуктивное сопротивление").AsDouble() / 1000;
        Debug.Print($"x, Ом/м {x}");
        //Количество параллельных кабелей
        var n = el.LookupParameter("Кол-во кабелей (провод) в одной группе").AsDouble();
        Debug.Print($"Количество пар кабелей {n}");
        double activePower = 0, reactivePower = 0;
        var loadFactor = el.get_Parameter(SharedParametersFile.Koeffitsient_Sprosa_V_SHCHitakh).AsDouble();
        foreach (Element element in el.Elements)
        {
            if (element is FamilyInstance fi)
            {
                var tryGetElectricalParameters = fi.TryGetElectricalParameters(out var ap, out var cosP, out _);
                if (!tryGetElectricalParameters)
                    continue;
                activePower += ap * loadFactor;
                var _tgP = Math.Sqrt(1 / cosP / cosP - 1);
                var _q = ap * _tgP;
                reactivePower += _q;
            }
        }

        var tgPhi = activePower < _tolerance ? 0 : reactivePower / activePower;
        var cosPhi = Math.Sqrt(1 / (1 + tgPhi * tgPhi));

        //Активная нагрузка
        //var activePower = el
        //    .Elements
        //    .OfType<FamilyInstance>()
        //    .Select(fi =>
        //    {
        //        fi.TryGetElectricalParameters(out double ap, out var cosP, out _);
        //        return (ap, cosP);
        //    })
        //    .Sum();
        //UnitUtils.ConvertFromInternalUnits(el.get_Parameter(BuiltInParameter.RBS_ELEC_TRUE_LOAD).AsDouble(),
        //    DisplayUnitType.DUT_WATTS);
        Debug.Print($"Активная нагрузка {activePower}");
        //cos F
        //var cosPhi = el.get_Parameter(BuiltInParameter.RBS_ELEC_POWER_FACTOR).AsDouble();
        Debug.Print($"cos phi {cosPhi}");
        //var tgPhi = Math.Sqrt(1 / cosPhi / cosPhi - 1);
        //Q
        //var reactivePower = activePower * tgPhi;
        Debug.Print($"Реактивная нагрузка {reactivePower}");
        //Общая потеря напряжения в сети
        var du0 = 0.0;
        var previousL = 0.0;
        var isTrueCalculating = true;
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
            Debug.Print($"\nЭлектроприемник {fi.Name}");
            devices.Remove(fi.Id);
            //Расчет потерь напряжения
            var l = previousL + UnitUtils.ConvertFromInternalUnits(nearest.Item2, UnitTypeId.Meters);
            Debug.Print($"Длина до предыдущего,м {l}");
            Debug.Print($"Активная мощность по участку, {activePower}");
            Debug.Print($"Реактивная мощность по участку, {reactivePower}");
            var du = CalculateLossVoltage(activePower, reactivePower, r, x, l, voltage, n, polesNumber);
            Debug.Print($"Реактивная Мощность приемника {reactivePower}");
            if (double.IsNaN(du) || double.IsInfinity(du))
            {
                isTrueCalculating = false;
                break;
            }

            Debug.Print($"Потери до предыдущего, В {du}");
            //Мощность приемника
            var tryGetElectricalParameters = fi.TryGetElectricalParameters(out var activePowerFi, out var powerFactorFi, out _);
            if (!tryGetElectricalParameters || activePower < _tolerance || powerFactorFi < _tolerance)
            {
                previousL = l;
                continue;
            }

            previousL = 0;
            var tgPhiFi = Math.Sqrt(1 / powerFactorFi / powerFactorFi - 1);
            var reactivePowerFi = activePowerFi * tgPhiFi;
            activePower -= activePowerFi;
            reactivePower -= reactivePowerFi;
            du0 += du;
            Debug.Print($"Общая потеря напряжения {du0}");
        }

        du0 = du0 / voltage * 100;
        Debug.Print($"Общая потеря напряжения,% {du0}");
        if (!isTrueCalculating)
            du0 = -1;

        return du0;
    }

    private double CalculateLossVoltage(
        double p,
        double q,
        double r,
        double x,
        double l,
        double u0,
        double n,
        int polesNumber)
    {
        var du = (polesNumber == 1 ? 2 : 1) * l * (p * r + q * x) / u0 / n;
        return du;
    }
}
