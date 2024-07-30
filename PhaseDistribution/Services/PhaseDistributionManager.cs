namespace PhaseDistribution.Services;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommonUtils;
using CommonUtils.Extensions;
using CommonUtils.Helpers;
using Helpers;
using Microsoft.Extensions.Hosting;

class PhaseDistributionManager : DefaultUseCase
{
    private readonly UIApplication _application;

    public PhaseDistributionManager(IApplicationLifetime applicationLifetime, UIApplication application)
        : base(applicationLifetime)
    {
        _application = application;
    }

    public override Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var doc = _application.ActiveUIDocument.Document;
        var baseShields = new FilteredElementCollector(doc)
            .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
            .WhereElementIsNotElementType()
            .Where(shield =>
                UnitUtils.ConvertFromInternalUnits(
                    shield.LookupParameter("Напряжение в щите").AsDouble(),
                    UnitTypeId.Volts) > 100)
            .OfType<FamilyInstance>()
            .Where(sh => sh.GetPowerElectricalSystem() is null)
            .ToArray();
        var shieldsQueue = new Queue<FamilyInstance>(baseShields);
        using var tr = new Transaction(doc);
        tr.Start("Распределение по фазам");
        while (shieldsQueue.Count > 0)
        {
            var currentShield = shieldsQueue.Dequeue();
            var shieldName = currentShield.Name;
            DistributionPhase(currentShield);
            var connectedShields = currentShield.MEPModel?
                .GetAssignedElectricalSystems()?
                .SelectMany(s => s.Elements.OfType<FamilyInstance>())
                .Where(sh => sh.IsShield());
            if (connectedShields != null)
                foreach (var shield in connectedShields)
                {
                    shieldsQueue.Enqueue(shield);
                }
        }

        tr.Commit();
        return Result.Succeeded;
    }

    private void DistributionPhase(FamilyInstance el)
    {
        //Количество полюсов, количество фаз не корректно работает
        var phaseCountOfShield = el.LookupParameter("Количество полюсов")?.AsInteger();
        if (phaseCountOfShield is null)
            return;
        var currentImbalance = 0.0;
        var currentImbalanceParam = el.LookupParameter("Перекос фаз");
        if (phaseCountOfShield == 1)
        {
            var powerSystem = el.GetPowerElectricalSystem();
            var phaseOfPowerSystem = powerSystem?.LookupParameter("Фаза").AsString() ?? "L1";
            var elSystems = el.MEPModel?
                .GetAssignedElectricalSystems();
            if (elSystems is null || elSystems.Count == 0)
                return;
            foreach (var elSystem in elSystems)
            {
                elSystem.LookupParameter("Фаза").Set(phaseOfPowerSystem);
            }
        }
        else
        {
            var ks = el.LookupParameter("Коэффициент спроса в щитах").AsDouble();
            var iParameter = new Parameter[]
            {
                el.LookupParameter("Суммарный ток L1"),
                el.LookupParameter("Суммарный ток L2"),
                el.LookupParameter("Суммарный ток L3")
            };
            var elSystems = el.MEPModel?
                .GetAssignedElectricalSystems()?
                .Where(s => s.get_Parameter(SharedParametersFile.Zapretit_Izmenenie)?.AsInteger() != 1)
                .OrderByDescending(s => s.HotConductorsNumber)
                // ток от полной установленной мощности
                .ThenByDescending(elS =>
                    elS.get_Parameter(BuiltInParameter.RBS_ELEC_APPARENT_CURRENT_PARAM).AsDouble())
                .ToList();
            if (elSystems is null || elSystems.Count == 0)
                return;
            var phaseDistribution = new PhaseDistribution();
            foreach (var system in elSystems)
            {
                var param = system.LookupParameter("Фаза");
                var phaseCountElSystem = system.HotConductorsNumber;
                var current = system.get_Parameter(BuiltInParameter.RBS_ELEC_APPARENT_CURRENT_PARAM).AsDouble();
                var phase = phaseDistribution.AddCurrent(current, phaseCountElSystem);
                param.Set(phase);
            }

            currentImbalance = ks == 0 ? 0 : phaseDistribution.GetPhacesImbalance();
            var phaseCurrents = phaseDistribution.LoadOnPhases
                .Select(x => Math.Round(x * ks, 2))
                .ToArray();
            for (var i = 0; i < iParameter.Length; i++)
            {
                iParameter[i].Set(phaseCurrents[i]);
            }
        }

        currentImbalanceParam.Set(currentImbalance);
    }
}