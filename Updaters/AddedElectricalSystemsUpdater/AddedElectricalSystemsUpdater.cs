namespace AddedElectricalSystemsUpdater;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using CommonUtils;
using CommonUtils.Helpers;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Settings;

[UsedImplicitly]
public class AddedElectricalSystemsUpdater : UpdaterBase
{
    private readonly IConfiguration _configuration;
    public AddedElectricalSystemsUpdater(Application app, IConfiguration configuration) : base(app)
    {
        _configuration = configuration;
    }

    public override void Execute(UpdaterData data)
    {
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
            using var scheduleCollector = new FilteredElementCollector(doc, schedule.Id);

            var firstElementId = scheduleCollector.FirstElementId();
            var parameter = schedule.KeyScheduleParameterName;
            schedulesParameters[parameter] = firstElementId;
        }
        
        foreach (var system in systems)
        {
            system!.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set("77777");

            foreach (Parameter systemParameter in system.Parameters)
            {
                if(schedulesParameters.ContainsKey(systemParameter.Definition.Name))
                {
                    systemParameter.Set(schedulesParameters[systemParameter.Definition.Name]);
                }
            }
            
            var mode = system.CircuitPathMode;
            if (mode == ElectricalCircuitPathMode.FarthestDevice)
                system.CircuitPathMode = ElectricalCircuitPathMode.AllDevices;
            system.get_Parameter(SharedParametersFile.Koeffitsient_Sprosa_V_SHCHitakh).Set(1.0);
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
