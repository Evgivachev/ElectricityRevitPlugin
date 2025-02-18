﻿namespace AddedElectricalSystemsUpdater;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using CommonUtils;
using CommonUtils.Extensions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Settings;

[UsedImplicitly]
public class AddedElectricalSystemsUpdater(AddInId addInId, IConfiguration configuration) : UpdaterBase(addInId)
{
    private CircuitInitialValues _initialValues = null!;

    public override void Execute(UpdaterData data)
    {        
        //TODO
        _initialValues = configuration.GetSection(nameof(CircuitInitialValues)).Get<CircuitInitialValues>();
        var doc = data.GetDocument();
        var addedSystems = data.GetAddedElementIds()
            .Select(x => doc.GetElement(x) as ElectricalSystem)
            .Where(x => x != null);
        SetParameters(doc, addedSystems!);
    }

    public override bool IsOptional => true;

    public override ChangePriority GetChangePriority() => ChangePriority.MEPSystems;
    public override string GetUpdaterName() => nameof(AddedElectricalSystemsUpdater);

    public override string GetAdditionalInformation() => "Устанавливает начальные значения параметров для добавленных электрических цепей";

    protected override Guid Guid => new("49391026-0405-4CB3-8E91-D3BB0D1F35E0");

    protected override ElementFilter ElementFilter => new ElementClassFilter(typeof(ElectricalSystem));

    protected override ChangeType ChangeType => Element.GetChangeTypeElementAddition();

    private void SetParameters(Document doc, IEnumerable<ElectricalSystem> systems)
    {
        
        var schedules = GetKeySchedules(doc);
        var schedulesParameters = new Dictionary<string, ElementId>();
        foreach (var schedule in schedules)
        {
            if (!_initialValues.FromKeyScheduleValues.ContainsKey(schedule.Name))
                continue;
            using var scheduleCollector = new FilteredElementCollector(doc, schedule.Id);

            var elementIds = scheduleCollector.ToElementIds();
            var elementId = elementIds.FirstOrDefault(id => id.IntegerValue == _initialValues.FromKeyScheduleValues[schedule.Name]);
            if (elementId == null)
                continue;
            var parameter = schedule.KeyScheduleParameterName;
            schedulesParameters[parameter] = elementId;
        }

        foreach (var system in systems)
        {
            foreach (Parameter systemParameter in system.Parameters)
            {
                if (schedulesParameters.ContainsKey(systemParameter.Definition.Name))
                {
                    systemParameter.Set(schedulesParameters[systemParameter.Definition.Name]);
                }
            }

            foreach (var pair in _initialValues.FromBuiltInParameters)
            {
                if (Enum.TryParse<BuiltInParameter>(pair.Key, out var parameter))
                {
                    system.get_Parameter(parameter)?.SetDynamicValue(pair.Value);
                }
            }

            foreach (var pair in _initialValues.FromSharedParameters)
            {
                if (Guid.TryParse(pair.Key, out var guid))
                    system.get_Parameter(guid)?.SetDynamicValue(pair.Value);
            }

            var mode = system.CircuitPathMode;
            if (mode == ElectricalCircuitPathMode.FarthestDevice)
                system.CircuitPathMode = ElectricalCircuitPathMode.AllDevices;
        }
    }

    private IReadOnlyList<ViewSchedule> GetKeySchedules(Document document)
    {
        using var collector = new FilteredElementCollector(document);
        var schedules = collector
            .WhereElementIsNotElementType()
            .OfClass(typeof(ViewSchedule))
            .OfType<ViewSchedule>()
            .Where(x => x.Definition.IsKeySchedule)
            .Where(x => x.Definition.CategoryId.IntegerValue == (int)BuiltInCategory.OST_ElectricalCircuit)
            .ToArray();
        return schedules;
    }
}
