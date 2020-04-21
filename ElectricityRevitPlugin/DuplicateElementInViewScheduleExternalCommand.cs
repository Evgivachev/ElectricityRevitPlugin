using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class DuplicateElementInViewScheduleExternalCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var doc = uiDoc.Document;
            var app = uiApp.Application;
            var result = Result.Succeeded;
            try
            {
                using (var tr = new Transaction(doc))
                {
                    tr.Start("Temp");
                    var selection = uiDoc.Selection;
                    var selectedIds = selection.GetElementIds();
                    selectedIds.Add(new ElementId(20151058));
                    var selectedElements = selectedIds
                        .Select(x => doc.GetElement(x));
                    foreach (var element in selectedElements)
                    {
                        var viewId = element.OwnerViewId;
                        if (viewId is null)
                            continue;
                        var view = doc.GetElement(viewId) as ViewSchedule;
                        if (view is null)
                            continue;
                        var cat = Category.GetCategory(doc, view.Definition.CategoryId);
                        view.AddElement(element, false);
                    }
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
}
