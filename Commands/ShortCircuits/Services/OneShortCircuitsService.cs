using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using CommonUtils;
using CommonUtils.Helpers;
using Microsoft.Extensions.Hosting;

namespace ShortCircuits.Services;

/// <inheritdoc />
public class OneShortCircuitsService : DefaultUseCase
{
    private double _resistanceOfElectricalContacts;
    private double _resistanceOfElectricalAcr;
    private double _lowVoltage;

    // все сопротивления в мОм!!!
    /// <inheritdoc />
    public override Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var document = commandData.Application.ActiveUIDocument.Document;
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
                .AsDouble() * 3;
            var x = transformer.Symbol
                .get_Parameter(SharedParametersFile.Obshchee_Reaktivnoe_Soprotivlenie_MOm)
                .AsDouble() * 3;
            _lowVoltage =
                UnitUtils.ConvertFromInternalUnits(transformer.Symbol.LookupParameter("Uнн").AsDouble(),
                    UnitTypeId.Volts);
            using var tr = new Transaction(document);
            tr.Start("Расчёт токов 1кз");
            SetParametersToElectricalSystemsInShield(transformer, r, x);
            tr.Commit();
        }

        return Result.Succeeded;
    }

    private void SetParametersToElectricalSystemsInShield(
        FamilyInstance panel,
        double r,
        double x)
    {
        if (panel.LookupParameter("Вводное отключающее устройство").AsElementId() != null)
            r += _resistanceOfElectricalContacts;

        var currentShort1 = CalculateShortCircuits(r + _resistanceOfElectricalAcr, x);
        var internalCurrent = UnitUtils.ConvertToInternalUnits(currentShort1, UnitTypeId.Amperes);
        panel.get_Parameter(SharedParametersFile.Tok_1KZ_A)?.Set(internalCurrent);
        var connectedSystems = panel.MEPModel?.GetAssignedElectricalSystems();
        if (connectedSystems == null) return;
        foreach (var system in connectedSystems)
        {
            var r1 = r;
            var x1 = x;
            var device1 = system.LookupParameter("Отключающее устройство 1").AsString();
            if (device1 != null && device1 != "-")
                r1 += _resistanceOfElectricalContacts;

            //Подключенное оборудование к цепи
            var connectedShields = system
                .Elements
                .Cast<FamilyInstance>()
                .Where(shield => shield.Category.Id.IntegerValue == (int)BuiltInCategory.OST_ElectricalEquipment);

            var (rPs, xPs) = GetResistanceOfElectricalSystem(system);
            r1 += 2 * rPs;
            x1 += 2 * xPs;
            var currentShort1Parameter = system.get_Parameter(SharedParametersFile.Tok_1KZ_A);
            currentShort1 = CalculateShortCircuits(r1 + _resistanceOfElectricalAcr, x1);
            internalCurrent = UnitUtils.ConvertToInternalUnits(currentShort1, UnitTypeId.Amperes);
            currentShort1Parameter?.Set(internalCurrent);
            foreach (var shield in connectedShields)
                SetParametersToElectricalSystemsInShield(shield, r1, x1);
        }
    }

    private double CalculateShortCircuits(double r, double x)
    {
        //в амперах
        var i = _lowVoltage / Math.Sqrt(r * r + x * x) * 1000;
        return i;
    }

    /// <summary>
    /// Выполняет расчет сопротивления кабеля в мОм
    /// </summary>
    private static (double r, double x) GetResistanceOfElectricalSystem(ElectricalSystem system)
    {
        // Ом / км
        var dr = system.LookupParameter("Активное сопротивление").AsDouble();
        var dx = system.LookupParameter("Индуктивное сопротивление").AsDouble();
        //Длина кабелей для ОС, конвертируются в метры
        var length = system.get_Parameter(SharedParametersFile.Dlina_Kabeley_Dlya_OS).AsDouble();
        length = UnitUtils.ConvertFromInternalUnits(length, UnitTypeId.Meters);
        var n = system.LookupParameter("Кол-во кабелей (провод) в одной группе").AsDouble();
        if (n < 1)
            throw new ArgumentException(
                $"Неверное значение \"Кол-во кабелей (провод) в одной группе\" в цепи {system.CircuitNumber}");
        var r = dr * length / n;
        var x = dx * length / n;
        return (r, x);
    }
}
