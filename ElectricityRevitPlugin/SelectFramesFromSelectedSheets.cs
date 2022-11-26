//using MoreLinq.Extensions;

namespace ElectricityRevitPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;

    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class SelectFramesFromSelectedSheets : IExternalCommand
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
                if (!selectedElementsIds.Any())
                    return result;
                var lists = selectedElementsIds.Select(x => doc.GetElement(x) as ViewSheet)
                    .Where(x => x != null)
                    .Select(x => x.Id.IntegerValue)
                    .ToHashSet();
                var framesIds = new List<ElementId>();
                var allFrames = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_TitleBlocks)
                    .WhereElementIsNotElementType()
                    .ToElements();
                foreach (var element in allFrames)
                {
                    var ownerView = element.OwnerViewId.IntegerValue;
                    if (lists.Contains(ownerView))
                        framesIds.Add(element.Id);
                }

                selection.SetElementIds(framesIds);
                return result;
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
