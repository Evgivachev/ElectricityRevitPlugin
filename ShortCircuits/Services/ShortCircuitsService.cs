using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using CommonUtils.Helpers;
using ShortCircuits.Abstractions;

namespace ShortCircuits.Services;

/// <inheritdoc />
public class ShortCircuitsService : IShortCircuitsService
{
    private double _resistanceOfElectricalContacts;
    private double _resistanceOfElectricalAcr;
    private double _lowVoltage;

    /// <inheritdoc />
    public void Calculate(Document document)
    {
        //Получить значения общих параметров
        _resistanceOfElectricalAcr = document.ProjectInformation
            .get_Parameter(SharedParametersFile.Rd_Dugi)
            .AsDouble();
        _resistanceOfElectricalContacts = document.ProjectInformation
            .get_Parameter(SharedParametersFile.Soprotivlenie_Kontaktov_Gr_SHCHitov_Zpk)
            .AsDouble();

        //Получить все трансформаторы
        var transformers = new FilteredElementCollector(document)
            .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
            .WhereElementIsNotElementType()
            .OfClass(typeof(FamilyInstance))
            .Cast<FamilyInstance>()
            .Where(x => x.Symbol.FamilyName == "Трансформатор");

        foreach (var transformer in transformers)
        {
            var r = transformer.Symbol
                .get_Parameter(SharedParametersFile.Obshchee_Aktivnoe_Soprotivlenie_MOm)
                .AsDouble();
            var x = transformer.Symbol
                .get_Parameter(SharedParametersFile.Obshchee_Reaktivnoe_Soprotivlenie_MOm)
                .AsDouble();
            _lowVoltage =
                UnitUtils.ConvertFromInternalUnits(transformer.Symbol.LookupParameter("Uнн").AsDouble(),
                    UnitTypeId.Volts);
            using var tr = new Transaction(document);
            tr.Start("Расчёт токов 3кз");
            SetParametersToElectricalSystemsInShield(null, transformer, r, x);
            tr.Commit();
        }
    }

    private void SetParametersToElectricalSystemsInShield(
        ElectricalSystem? powerSystem,
        FamilyInstance panel,
        double r,
        double x)
    {
        if (powerSystem != null)
        {
            var (rPs, xPs) = GetResistanceOfElectricalSystem(powerSystem);
            r += rPs;
            x += xPs;
        }

        if (panel.LookupParameter("Вводное отключающее устройство").AsElementId() != null)
        {
            r += _resistanceOfElectricalContacts;
        }

        var connectedSystems = panel.MEPModel?.GetAssignedElectricalSystems();
        if (connectedSystems == null) return;
        foreach (var system in connectedSystems)
        {
            var device1 = system.LookupParameter("Отключающее устройство 1").AsString();
            if (device1 != null && device1 != "-")
                r += _resistanceOfElectricalContacts;
            var currentShort3 = CalculateShortCircuits(r + _resistanceOfElectricalAcr, x);
            var currentShort3Parameter = system.get_Parameter(SharedParametersFile.Tok_3KZ_A);
            var internalCurrent = UnitUtils.ConvertToInternalUnits(currentShort3, UnitTypeId.Amperes);
            currentShort3Parameter.Set(internalCurrent);

            //Подключенное оборудование к цепи
            var connectedShields = system
                .Elements
                .Cast<FamilyInstance>()
                .Where(shield => shield.Category.Id.IntegerValue == (int) BuiltInCategory.OST_ElectricalEquipment);
            foreach (var shield in connectedShields)
            {
                SetParametersToElectricalSystemsInShield(system, shield, r, x);
            }
        }
    }

    private double CalculateShortCircuits(double r, double x)
    {
        //в амперах
        var i = _lowVoltage / Math.Sqrt(3 * (r * r + x * x)) * Math.Pow(10, 3);
        return i;
    }

    private static (double r, double x) GetResistanceOfElectricalSystem(ElectricalSystem system)
    {
        var dr = system.LookupParameter("Активное сопротивление").AsDouble();
        var dx = system.LookupParameter("Индуктивное сопротивление").AsDouble();
        //Длина кабелей для ОС
        var length = system.get_Parameter(SharedParametersFile.Dlina_Kabeley_Dlya_OS).AsDouble();
        var n = system.LookupParameter("Кол-во кабелей (провод) в одной группе").AsDouble();
        if (n < 1)
            throw new ArgumentException(
                $"Неверное значение \"Кол-во кабелей (провод) в одной группе\" в цепи {system.CircuitNumber}");
        var r = dr / 1000 * length / n;
        var x = dx / 1000 * length / n;
        return (r, x);
    }
}