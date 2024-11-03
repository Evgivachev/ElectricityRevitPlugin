namespace GroupByGost.Infrastructure;

using System;
using System.Collections.Generic;
using System.Linq;
using Application;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using CommonUtils.Extensions;
using Domain;
using Element = Domain.Element;

public class DbRepository : IDbRepository
{
    private readonly Document _document;
    private readonly Guid _disableChangeGuid = new("be64f474-c030-40cf-9975-6eaebe087a84");
    private readonly Guid _groupByGostGuid = new("8d1b8079-3007-4140-835c-73f0de4e81bd");
    private readonly Guid _idLinkElement = new("dca1fe51-4090-4178-9f12-a83aa5986266");
    private readonly Guid _isControlCircuit = new("0f13e1e5-71bb-4b0f-b3dc-18054c25e1ee");
    private readonly Guid _isReserveGroupGuid = new("cd2dc469-276a-40f4-bd34-c6ab2ae05348");

    public DbRepository(Document document)
    {
        _document = document;
    }

    public IReadOnlyCollection<Shield> GetShields()
    {
        using var fec = new FilteredElementCollector(_document);
        var shields = fec
            .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
            .OfClass(typeof(FamilyInstance))
            .Cast<FamilyInstance>()
            .Select(shield =>
            {
                var circuits = shield
                    .MEPModel?
                    .GetAssignedElectricalSystems()?
                    .Select(c => new ElectricalCircuit()
                    {
                        Id = c.Id.IntegerValue,
                        IsControlCircuit = c.get_Parameter(_isControlCircuit).AsInteger() == 1,
                        IsDisableChange = c.get_Parameter(_disableChangeGuid)?.AsInteger() == 1,
                        IsReserve = c.get_Parameter(_isReserveGroupGuid).AsInteger() == 1,
                        StartSlot = c.StartSlot,
                    })
                    .ToArray() ?? Array.Empty<ElectricalCircuit>();
                return shield.ToShield(circuits);
            });

        return shields.ToArray();
    }
    public IReadOnlyCollection<Element> GetElectricalElements()
    {
        using var fec = new FilteredElementCollector(_document);
        var elements = fec
            .OfClass(typeof(FamilyInstance))
            .WherePasses(new ElementParameterFilter(
                ParameterFilterRuleFactory.CreateSharedParameterApplicableRule("Номер группы по ГОСТ")))
            .OfType<FamilyInstance>()
            .Where(e =>
            {
                var category = e.Category;
                return category.Id.IntegerValue != (int)BuiltInCategory.OST_GenericAnnotation;
            })
            .Select(element =>
            {
                var powerCable = element.GetPowerElectricalSystem();
                int.TryParse(element.get_Parameter(_idLinkElement)?.AsString(),
                    out var linkedElementParameterId);
                var parentElement = _document.GetElement(new ElementId(linkedElementParameterId)) as ElectricalSystem;
                return new Element
                {
                    Id = element.Id.IntegerValue,
                    ParentElementId = parentElement?.Id.IntegerValue,
                    PowerCableId = powerCable?.Id.IntegerValue,
                };
            });
        return elements.ToArray();

    }

    public void UpdateCircuits(IReadOnlyCollection<ElectricalCircuit> circuits)
    {
        foreach (var circuit in circuits)
        {
            if (circuit.IsDisableChange)
            {
                continue;
            }

            var element = _document.GetElement(new ElementId(circuit.Id));

            var param = element.get_Parameter(_groupByGostGuid);
            if (!param.IsReadOnly)
                param.Set(circuit.GroupByGost);
            var numberQfParam = element.LookupParameter("Номер QF");
            if (!numberQfParam.IsReadOnly)
                numberQfParam.Set(circuit.QFNumber);
        }
    }
    public void UpdateElements(IReadOnlyCollection<Element> elements)
    {
        foreach (var element in elements)
        {
            var elementDb = _document.GetElement(new ElementId(element.Id));
            var param = elementDb.get_Parameter(_groupByGostGuid);
            if (!param.IsReadOnly)
                param.Set(element.GroupByGost);
        }
    }
    
    public IReadOnlyCollection<ElectricalCircuit> GetElectricalSystems()
    {
        using var fec = new FilteredElementCollector(_document);
        var electricalSystems = fec
            .OfCategory(BuiltInCategory.OST_ElectricalCircuit)
            .OfClass<ElectricalSystem>()
            .Select(c => new ElectricalCircuit()
            {
                Id = c.Id.IntegerValue,
                IsControlCircuit = c.get_Parameter(_isControlCircuit).AsInteger() == 1,
                IsDisableChange = c.get_Parameter(_disableChangeGuid)?.AsInteger() == 1,
                IsReserve = c.get_Parameter(_isReserveGroupGuid).AsInteger() == 1,
                StartSlot = c.StartSlot,
            });

        return electricalSystems.ToArray();
    }
}
