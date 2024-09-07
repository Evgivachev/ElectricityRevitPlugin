namespace ElectricityRevitPlugin;

using System;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
class ConnectElectricalSystemToPanelExternalCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiApp = commandData.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        var result = Result.Succeeded;
        try
        {
            using (var tr = new Transaction(doc))
            {
                tr.Start("ConnectSystemsToShield");
                var selection = uiDoc.Selection;
                var selectedIds = selection.GetElementIds();
                var selectedElements = selectedIds
                    .Select(x => doc.GetElement(x))
                    .OfType<ElectricalSystem>()
                    .ToArray();
                var shield = (FamilyInstance)doc.GetElement(selection.PickObject(ObjectType.Element).ElementId);
                if (shield is null)
                    throw new NullReferenceException("Следует выбрать щит и элементы");

                foreach (var element in selectedElements)
                    element.SelectPanel(shield);

                tr.Commit();
            }
        }
        catch (Exception e)
        {
            message += e.Message + '\n' + e.StackTrace;
            result = Result.Failed;
        }
        finally
        {
        }

        return result;
    }
}
