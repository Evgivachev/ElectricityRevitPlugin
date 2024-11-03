namespace ElectricityRevitPluginApp.Triggers;

using AddedElectricalSystemsUpdater;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using JetBrains.Annotations;

[UsedWith(nameof(AddedElectricalSystemsUpdater))]
[UsedImplicitly]
public class OnCreatedElectricalSystemTrigger : IUpdaterTrigger
{
    public ElementFilter ElementFilter => new ElementClassFilter(typeof(ElectricalSystem));

    public ChangeType ChangeType => Element.GetChangeTypeElementAddition();
}
