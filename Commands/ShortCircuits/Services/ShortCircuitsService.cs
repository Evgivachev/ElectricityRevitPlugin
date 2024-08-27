using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using CommonUtils;
using CommonUtils.Helpers;
using Microsoft.Extensions.Hosting;

namespace ShortCircuits.Services;

/// <inheritdoc />
public class ShortCircuitsService : DefaultUseCase
{
    private double _resistanceOfElectricalContacts;
    private double _resistanceOfElectricalAcr;
    private double _lowVoltage;

    public ShortCircuitsService(IApplicationLifetime applicationLifetime)
        : base(applicationLifetime)
    {
    }

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
                .AsDouble();
            var x = transformer.Symbol
                .get_Parameter(SharedParametersFile.Obshchee_Reaktivnoe_Soprotivlenie_MOm)
                .AsDouble();
            _lowVoltage =
                UnitUtils.ConvertFromInternalUnits(transformer.Symbol.LookupParameter("Uнн").AsDouble(),
                    UnitTypeId.Volts);
            using var tr = new Transaction(document);
            tr.Start("Расчёт токов 3кз");
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

        var connectedSystems = panel.MEPModel?.GetAssignedElectricalSystems();
        var currentShort3 = CalculateShortCircuits(r + _resistanceOfElectricalAcr, x);
        var internalCurrent = UnitUtils.ConvertToInternalUnits(currentShort3, UnitTypeId.Amperes);
        panel.get_Parameter(SharedParametersFile.Tok_3KZ_A)?.Set(internalCurrent);
        if (connectedSystems == null) return;
        foreach (var system in connectedSystems)
        {
            var currentShort3Parameter = system.get_Parameter(SharedParametersFile.Tok_3KZ_A);
            if (system.HotConductorsNumber < 3)
            {
                currentShort3Parameter.Set(0);
                continue;
            }

            var r1 = r;
            var x1 = x;
            var device1 = system.LookupParameter("Отключающее устройство 1").AsString();
            if (device1 != null && device1 != "-")
                r1 += _resistanceOfElectricalContacts;
            currentShort3 = CalculateShortCircuits(r1 + _resistanceOfElectricalAcr, x);
            internalCurrent = UnitUtils.ConvertToInternalUnits(currentShort3, UnitTypeId.Amperes);
            currentShort3Parameter.Set(internalCurrent);

            var kud = CalculateKud(r1, x);
            system.get_Parameter(SharedParametersFile.Kud_Koeffitsient_Udarnogo_Toka)?.Set(kud);
            //Подключенное оборудование к цепи
            var connectedShields = system
                .Elements
                .Cast<FamilyInstance>()
                .Where(shield => shield.Category.Id.IntegerValue == (int)BuiltInCategory.OST_ElectricalEquipment);

            var (rPs, xPs) = GetResistanceOfElectricalSystem(system);
            r1 += rPs;
            x1 += xPs;
            foreach (var shield in connectedShields)
                SetParametersToElectricalSystemsInShield(shield, r1, x1);
        }
    }

    private double CalculateShortCircuits(double r, double x)
    {
        //в амперах
        var i = _lowVoltage / Math.Sqrt(3 * (r * r + x * x)) * 1000;
        return i;
    }

    /// <summary>
    /// Коэффициент ударного тока.
    /// </summary>
    /// <param name="r">Активное сопротивление.</param>
    /// <param name="x">Реактивное сопротивление</param>
    private static double CalculateKud(double r, double x)
    {
        // постоянная времени затухания апериодической составляющей тока КЗ
        var ta = x / r / (2 * Math.PI / 50);
        // угол сдвига по фазе напряжения или ЭДС источника и периодической составляющей тока КЗ
        var phi = Math.Atan2(x, r);
        // время от начала КЗ до появления ударного тока, с
        var tud = 0.01 * (Math.PI / 2 + phi) / Math.PI;

        var kud = 1 + Math.Sin(phi) * Math.Exp(-tud / ta);

        return kud;
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
