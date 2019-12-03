using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class CopySheetsExternalCommand : IExternalCommand
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
                    tr.Start(this.GetType().Name);
                    var selection = uiDoc.Selection;
                    var selectedIds = selection.GetElementIds();
                    var sheets = selectedIds
                        .Select(x => doc.GetElement(x))
                        .OfType<ViewSheet>()
                        .ToArray();

                    foreach (var sheet in sheets)
                    {
                        CopySheet(sheet,ref message);
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

        private void CopySheet(ViewSheet sheet,ref string message)
        {
            var doc = sheet.Document;
            var elementsOnView = new FilteredElementCollector(doc, sheet.Id)
                .ToElements();
            var titleBlock = elementsOnView
                .FirstOrDefault(el => el.Category.Id.IntegerValue == (int)BuiltInCategory.OST_TitleBlocks);
            elementsOnView.Remove(titleBlock);

            var newView = ViewSheet.Create(doc, titleBlock.GetTypeId());
            doc.Regenerate();

            var newTitleBlock = new FilteredElementCollector(doc, newView.Id)
                .ToElements()
                .FirstOrDefault(el => el.Category.Id.IntegerValue == (int)BuiltInCategory.OST_TitleBlocks);
            newTitleBlock.CopyParameters(titleBlock, false, "SheetNumber", "Номер листа");
            newTitleBlock.Location.Move((titleBlock.Location as LocationPoint).Point);

            newView.CopyParameters(sheet, false, "Номер листа");
            newView.SheetNumber = sheet.SheetNumber + "_Copy";
            newView.LookupParameter("Стадия на листе").Set("П");

            var elementsToCopy = new List<ElementId>();
            foreach (var el in elementsOnView)
            {
                if (el is Viewport viewport)
                {
                    var name = viewport.Name;
                    var ownerViewId = viewport.ViewId;
                    var ownerView = doc.GetElement(ownerViewId) as View;
                    ElementId dependedViewId = null;
                    if (ownerView.CanViewBeDuplicated(ViewDuplicateOption.AsDependent))
                    {
                        dependedViewId = ownerView.Duplicate(ViewDuplicateOption.AsDependent);
                    }
                    else if (ownerView.ViewType == ViewType.Legend)
                        dependedViewId = ownerViewId;
                    else if (ownerView.ViewType == ViewType.DraftingView)
                    {
                        if (!ownerView.CanViewBeDuplicated(ViewDuplicateOption.WithDetailing))
                        {
                            continue;
                        }
                        dependedViewId = ownerView.Duplicate(ViewDuplicateOption.WithDetailing);
                    }

                    if (dependedViewId is null)
                    {
                        message += ownerView.Name + "\n";
                    }

                    doc.Regenerate();
                    //var dependedView = doc.GetElement(dependedViewId) as View;
                    var bb = el.get_BoundingBox(sheet);
                    var center = viewport.GetBoxCenter();
                    UV location = new UV(center.X, center.Y);
                    Viewport.Create(doc, newView.Id, dependedViewId, new XYZ(location.U, location.V, 0));
                }
                else if (el is ScheduleSheetInstance || el.Category.Id.IntegerValue==(int)BuiltInCategory.OST_RasterImages)
                {
                    elementsToCopy.Add(el.Id);
                }

            }
            if(elementsToCopy.Any())
            ElementTransformUtils.CopyElements(sheet, elementsToCopy, newView, null, null);

        }
    }
}
