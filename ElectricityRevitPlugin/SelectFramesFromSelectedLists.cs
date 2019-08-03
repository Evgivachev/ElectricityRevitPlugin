using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace ElectricityRevitPlugin
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class SelectFramesFromSelectedLists : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var app = uiApp.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var doc = uiDoc.Document;

            var result = Result.Succeeded;
            try
            {
                var selection = uiDoc.Selection;
                var selectedElementsIds = selection.GetElementIds();
                var framesIds = new List<ElementId>();
                foreach (var id in selectedElementsIds)
                {
                    var list = doc.GetElement(id) as ViewSheet;
                    if (list is null)
                        continue;
                    var framesOnView = new FilteredElementCollector(doc, id)
                        .OfCategory(BuiltInCategory.OST_TitleBlocks)
                        .WhereElementIsNotElementType()
                        .ToElementIds()
                        .Where(elementId => elementId != null)
                        .ToArray();
                    if (framesOnView.Any())
                        framesIds.AddRange(framesOnView);
                }

                if (framesIds.Any())
                    selection.SetElementIds(framesIds);
            }
            catch (Exception e)
            {
                message += e.Message + '\n' + e.StackTrace;
                result = Result.Failed;
            }

            return result;
        }
    }
}