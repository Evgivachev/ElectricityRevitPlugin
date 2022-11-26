namespace Diagrams.ExternalCommands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class PhaseDistributionExternalCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var app = uiApp.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var doc = uiDoc.Document;
            var result = Result.Succeeded;
            FamilyInstance currentShield = null;
            try
            {
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
                var names = baseShields.Select(x => x.Name).ToArray();
                using (var tr = new Transaction(doc))
                {
                    tr.Start("Распределение по фазам");
                    while (shieldsQueue.Count > 0)
                    {
                        currentShield = shieldsQueue.Dequeue();
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
                }
            }
            catch (Exception e)
            {
                message += e.GetType().Name + '\n' + e.Message + '\n' + e.StackTrace;
                elements.Insert(currentShield);
                result = Result.Failed;
            }

            return result;
        }

        public void DistributionPhase(FamilyInstance el)
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
                var phaseCurrents = phaseDistribution.LoadOnPhases.Select(x => Math.Round(x * ks, 2)).ToArray();
                for (var i = 0; i < iParameter.Length; i++)
                {
                    iParameter[i].Set(phaseCurrents[i]);
                }
            }

            currentImbalanceParam.Set(currentImbalance);
        }
    }
}
