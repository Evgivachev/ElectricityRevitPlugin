namespace ElectricityRevitPlugin;

using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class Temp11 : DefaultExternalCommand
{
    protected override Result DoWork(ref string message, ElementSet elements)
    {
        var selection = UiDoc.Selection;
        var viewIds = selection.GetElementIds();
        var views = viewIds
            .Select(Doc.GetElement)
            .OfType<ViewDrafting>();
        using (var tr = new Transaction(Doc, "Rename"))
        {
            var currentRevitServerAccelerator = App.CurrentRevitServerAccelerator;
            var loadedApps = UiApp.LoadedApplications.Cast<IExternalApplication>();
            var updaters = UpdaterRegistry.GetRegisteredUpdaterInfos();
            var ups = UpdaterRegistry.GetRegisteredUpdaterInfos(Doc);
        }

        return Result.Succeeded;
    }
}
