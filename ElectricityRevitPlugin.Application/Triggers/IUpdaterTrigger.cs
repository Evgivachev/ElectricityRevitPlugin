namespace ElectricityRevitPlugin.Application.Triggers;

using Autodesk.Revit.DB;

public interface IUpdaterTrigger
{
    ElementFilter ElementFilter { get; }

    ChangeType ChangeType { get; }
}
