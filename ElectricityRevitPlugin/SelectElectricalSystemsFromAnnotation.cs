namespace ElectricityRevitPlugin;

using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class SelectElectricalSystemsFromAnnotation : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiApp = commandData.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var app = uiApp.Application;
        var doc = uiDoc.Document;
        var result = Result.Succeeded;
        var systems = new Dictionary<string, ElectricalSystem>();
        var systems1 = new FilteredElementCollector(doc)
            .OfCategory(BuiltInCategory.OST_ElectricalCircuit)
            .Cast<ElectricalSystem>();
        foreach (var system in systems1)
            systems[system.Name] = system;

        var selection = uiDoc.Selection.GetElementIds()
            .Select(x => doc.GetElement(x))
            .Select(x => x.LookupParameter("Номер цепи").AsString())
            .Where(x => x != null)
            .Select(x =>
            {
                if (systems.ContainsKey(x))
                    return systems[x];
                return null;
            })
            .Where(x => x != null);
        uiDoc.Selection.SetElementIds(selection.Select(x => x.Id).ToArray());
        return result;
    }
}
