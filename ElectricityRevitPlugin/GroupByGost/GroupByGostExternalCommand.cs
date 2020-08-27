// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SpecificationRevitToExcel
 * GroupByGostExternalCommand.cs
 * https://revit-addins.blogspot.ru
 * © EvgIv, 2018
 *
 * The external command.
 */
#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using ElectricityRevitPlugin.Extensions;

#endregion

namespace ElectricityRevitPlugin.GroupByGost
{
    /// <summary>
    /// Revit external command.
    /// </summary>	
	[Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]

    public sealed class GroupByGostExternalCommand : DefaultExternalCommand
    {
        private readonly Guid _groupByGostGuid = new Guid("8d1b8079-3007-4140-835c-73f0de4e81bd");
        private readonly string _defaultGroupByGost = "???";
        private readonly Guid _disableChangeGuid = new Guid("be64f474-c030-40cf-9975-6eaebe087a84");
        private readonly Guid _idLinkElement = new Guid("dca1fe51-4090-4178-9f12-a83aa5986266");

        protected override Result DoWork(ref string message, ElementSet elements)
        {
            using (var tr = new Transaction(Doc, "trName"))
            {
                if (TransactionStatus.Started == tr.Start())
                {
                    //Добавить быстрый фильтр
                    var allElements = new FilteredElementCollector(Doc)
                        .WherePasses(new ElementParameterFilter(ParameterFilterRuleFactory.CreateSharedParameterApplicableRule("Номер группы по ГОСТ")))
                        .OfType<FamilyInstance>();

                    var shields = new FilteredElementCollector(Doc)
                        .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                        .OfClass(typeof(FamilyInstance))
                        .Cast<FamilyInstance>();

                    SetValuesToShield(shields);

                    foreach (var element in allElements)
                    {
                        //Не брать элементы типовых аннотаций (Однолинейные схемы)
                        var category = element.Category;
                        if (category.Id.IntegerValue == (int)BuiltInCategory.OST_GenericAnnotation)
                            continue;
                        SetValuesToElement(element);
                    }

                    tr.Commit();
                    return Result.Succeeded;
                }

                return Result.Failed;
            }
        }

        public void SetValuesToElement(FamilyInstance fi)
        {
            var parameter = fi.get_Parameter(_groupByGostGuid);

            if (parameter is null)
                return;
            Element powerCable = fi.GetPowerElectricalSystem();
            if (powerCable is null)
            {
                var linkedElementParameterId = fi.get_Parameter(_idLinkElement)?.AsString();
                if (linkedElementParameterId != null)
                    powerCable = Doc.GetElement(linkedElementParameterId);
                else
                {
                    parameter.Set(_defaultGroupByGost);
                    return;
                }
            }
            var circuitName = powerCable?.Name;
            var circuitGost = powerCable?.get_Parameter(_groupByGostGuid)?.AsString();
            parameter?.Set(string.IsNullOrEmpty(circuitGost) ? circuitName : circuitGost);

        }
        public void SetValuesToElement(ElectricalSystem electricalSystem)
        {
            var shield = electricalSystem.BaseEquipment;
            if (shield is null)
            {
                var param = electricalSystem.get_Parameter(_groupByGostGuid);
                param.Set(_defaultGroupByGost);
                return;
            }
            SetValuesToShield(new[] { shield });
        }
        private void SetValuesToShield(IEnumerable<FamilyInstance> shields)
        {
            foreach (var shield in shields)
            {
                //var uGuid = new Guid("dd0d401b-feb3-4c57-bb19-1e4463b5f310");
                //var uParameter = shield.get_Parameter(uGuid);
                //var u = UnitUtils.ConvertFromInternalUnits(uParameter.AsDouble(), DisplayUnitType.DUT_VOLTS);
                var circuits = shield
                    .MEPModel?
                    .AssignedElectricalSystems?
                    .Cast<ElectricalSystem>()
                    .OrderBy(s =>
                    {
                        var isReserve = s.LookupParameter("Резервная группа")?.AsInteger() ?? 0;
                        var isControlCircuit = s.LookupParameter("Контрольные цепи")?.AsInteger() ?? 0;
                        return isReserve * 10 + isControlCircuit;
                    })
                    .ThenBy(x => x.StartSlot);
                if (circuits is null) continue;
                var prefix = shield.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_PREFIX).AsString();
                var separator = shield.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_PREFIX_SEPARATOR).AsString();
                var number = 1;
                foreach (var circuit in circuits)
                {
                    var isDisableChangeParameter = circuit.get_Parameter(_disableChangeGuid)?.AsInteger() == 1;
                    if (isDisableChangeParameter)
                        continue;
                    var param = circuit.get_Parameter(_groupByGostGuid);
                    if (!param.IsReadOnly)
                        param.Set($"{prefix}{separator}{number}");

                    var numberQfParam = circuit.LookupParameter("Номер QF");
                    if (!numberQfParam.IsReadOnly)
                        numberQfParam.Set($"QF{number}");
                    number++;
                }
            }
        }
    }
}