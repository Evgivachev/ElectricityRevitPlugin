namespace ElectricityRevitPlugin.Updaters;

using System;
using System.Linq;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using UpdateParametersInCircuits;

public class LossVoltage : MyUpdater
{
    private readonly Guid _isUnEditable = new("be64f474-c030-40cf-9975-6eaebe087a84");

    public LossVoltage(AddInId id)
        : base(id)
    {
    }

    protected override Guid UpdaterGuid { get; } = new("92CA0F1E-7BB8-4249-9064-751C8691BA90");

    protected override string Name { get; } = "Обновление потерь напряжения в цепи";
    protected override ChangePriority ChangePriority { get; } = ChangePriority.MEPCalculations;
    protected override string AdditionalInformation { get; } = "Обновление потерь напряжения в цепи";
    public override ElementFilter ElementFilter { get; } = new ElementCategoryFilter(BuiltInCategory.OST_ElectricalCircuit);

    protected override void ExecuteInner(UpdaterData data)
    {
        try
        {
            var doc = data.GetDocument();
            var command = new LossVoltageOfElectricalSystemExternalCommand();
            command.Doc = doc;
            var systems = data
                .GetModifiedElementIds()
                .Select(x => doc.GetElement(x) as ElectricalSystem)
                .Concat(data.GetAddedElementIds().Select(x => doc.GetElement(x) as ElectricalSystem));
            foreach (var system in systems)
            {
                var isUnEditable = system.get_Parameter(_isUnEditable).AsInteger() == 1;
                if (isUnEditable)
                    continue;
                command.UpdateParameters(system);
            }
        }
        catch (Exception e)
        {
            MessageBox.Show($"{e.Message}\n{e.StackTrace}");
        }
    }
}
