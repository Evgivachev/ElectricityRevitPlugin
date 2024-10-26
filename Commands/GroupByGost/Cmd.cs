namespace GroupByGost;

using System;
using System.Collections.Generic;
using System.Linq;
using Application;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using CommonUtils;
using CommonUtils.Extensions;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Revit external command.
/// </summary>	
[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class Cmd : CmdBase
{
    private readonly string _defaultGroupByGost = "???";
    private readonly Guid _disableChangeGuid = new("be64f474-c030-40cf-9975-6eaebe087a84");
    private readonly Guid _groupByGostGuid = new("8d1b8079-3007-4140-835c-73f0de4e81bd");
    private readonly Guid _idLinkElement = new("dca1fe51-4090-4178-9f12-a83aa5986266");
    private Guid _isControlCircuit = new("0f13e1e5-71bb-4b0f-b3dc-18054c25e1ee");

    private Guid _isReserveGroupGuid = new("cd2dc469-276a-40f4-bd34-c6ab2ae05348");

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ICmdUseCase, UseCase>();
        services.AddBll();
        services.AddInfrastructure();
    }

    /*/// <inheritdoc />
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var doc = commandData.Application.ActiveUIDocument.Document;
        using var tr = new Transaction(doc, "trName");
        if (TransactionStatus.Started == tr.Start())
        {
            //Добавить быстрый фильтр
            var allElements = new FilteredElementCollector(doc)
                .WherePasses(new ElementParameterFilter(
                    ParameterFilterRuleFactory.CreateSharedParameterApplicableRule("Номер группы по ГОСТ")))
                .OfType<FamilyInstance>();
            var shields = new FilteredElementCollector(doc)
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
    }*/

    public void SetValuesToElement(FamilyInstance fi)
    {
        var parameter = fi.get_Parameter(_groupByGostGuid);
        if (parameter is null)
            return;
        var powerCable = fi.GetPowerElectricalSystem();
        if (powerCable is null)
        {
            var parsing = int.TryParse(fi.get_Parameter(_idLinkElement)?.AsString(),
                out var linkedElementParameterId);
            if (parsing)
                powerCable = fi.Document.GetElement(new ElementId(linkedElementParameterId)) as ElectricalSystem;
            else
            {
                parameter.Set(_defaultGroupByGost);
                return;
            }
        }

        var circuitName = powerCable?.Name;
        var circuitGost = powerCable?.get_Parameter(_groupByGostGuid)?.AsString();
        parameter.Set(string.IsNullOrEmpty(circuitGost) ? circuitName ?? string.Empty : circuitGost);
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
            var circuits = shield
                .MEPModel?
                .GetAssignedElectricalSystems()?
                .OrderBy(s =>
                {
                    var isReserve = s.get_Parameter(_isReserveGroupGuid).AsInteger();
                    var isControlCircuit = s.get_Parameter(_isControlCircuit).AsInteger();
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
                {
                    number++;
                    continue;
                }

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
