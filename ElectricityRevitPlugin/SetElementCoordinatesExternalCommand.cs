namespace ElectricityRevitPlugin;

using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class SetElementCoordinatesExternalCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiApp = commandData.Application;
        var app = uiApp.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        var selection = uiDoc.Selection;
        var selectedElementsIds = selection.GetElementIds();
        var selectedElements = selectedElementsIds.Select(id
                => doc.GetElement(id))
            .ToArray();
        var model = new CoordinateModelMvc(selectedElements);
        var q = new GetCoordinateFromUserWindow(model);
        q.ShowDialog();
        return Result.Succeeded;
    }
}
