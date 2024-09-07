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
        viewIds
            .Select(Doc.GetElement)
            .OfType<ViewDrafting>();
        using (new Transaction(Doc, "Rename"))
        {
            UpdaterRegistry.GetRegisteredUpdaterInfos();
            UpdaterRegistry.GetRegisteredUpdaterInfos(Doc);
        }

        return Result.Succeeded;
    }
}
