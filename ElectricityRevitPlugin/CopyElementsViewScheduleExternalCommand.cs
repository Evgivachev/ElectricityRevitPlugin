//using MoreLinq;
//using MoreLinq.Extensions;

namespace ElectricityRevitPlugin;

using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Extensions;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class CopyElementsViewScheduleExternalCommand : IExternalCommand
{
    private ExternalCommandData _commandData;
    public ViewSchedule ActiveViewSchedule;
    public Application Application;
    public Document OpenedDocument;
    public ViewSchedule[] SchedulesInOpenFile;
    public ViewSchedule SimilarViewSchedule;

    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        _commandData = commandData;
        var uiApp = commandData.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        Application = uiApp.Application;
        var result = Result.Succeeded;
        ActiveViewSchedule = doc.ActiveView as ViewSchedule;
        var window = new CopyElementsInViewScheduleWPF(this);
        window.CopyElements();
        return result;
    }

    public ViewSchedule[] GetSimilarKeySchedules(Document doc, ViewSchedule schedule)
    {
        var similarViewSchedule = new FilteredElementCollector(doc)
            .OfCategory(BuiltInCategory.OST_Schedules)
            .OfType<ViewSchedule>()
            .Where(s => s.Definition.IsKeySchedule)
            .Where(s => s.Definition.CategoryId == ActiveViewSchedule.Definition.CategoryId)
            .ToArray();
        return similarViewSchedule;
    }

    public void Work()
    {
        var uiApp = _commandData.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        using (var tr = new Transaction(doc))
        {
            tr.Start("Temp");
            var nElements = new FilteredElementCollector(OpenedDocument, SimilarViewSchedule.Id)
                .ToElements();
            var ownElements = new FilteredElementCollector(doc, ActiveViewSchedule.Id)
                .ToElements();
            var keys = ownElements.Select(x => x.get_Parameter(BuiltInParameter.REF_TABLE_ELEM_NAME).AsString())
                .ToHashSet();
            foreach (var nElement in nElements)
            {
                var nKey = nElement.get_Parameter(BuiltInParameter.REF_TABLE_ELEM_NAME).AsString();
                if (keys.Contains(nKey))
                    continue;
                ActiveViewSchedule.AddElement(nElement, false);
            }

            tr.Commit();
        }
    }
}
