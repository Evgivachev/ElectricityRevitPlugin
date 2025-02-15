using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BimRenRes.Sheets;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class DuplicateSheets : IExternalCommand
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
                var duplicatedSheets = new List<ViewSheet>();
                var selection = uiDoc.Selection;
                using (var tr = new Transaction(doc))
                {
                    tr.Start(this.GetType().Name);
                    var selectedIds = selection.GetElementIds();
                    var sheets = selectedIds
                        .Select(x => doc.GetElement(x))
                        .OfType<ViewSheet>()
                        .ToArray();

                    foreach (var sheet in sheets)
                    {
                        var newSheet = CopySheet(sheet, ref message);
                        if (newSheet != null)
                            duplicatedSheets.Add(newSheet);
                    }
                    selection.SetElementIds(new List<ElementId>());
                    selection.SetElementIds(duplicatedSheets.Select(x => x.Id).ToArray());
                    doc.Regenerate();
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

    private ViewSheet CopySheet(ViewSheet sheet, ref string message)
    {
            var doc = sheet.Document;
            var elementsOnView = new FilteredElementCollector(doc, sheet.Id)
                .ToElements();
            var titleBlock = elementsOnView
                .FirstOrDefault(el => el.Category.Id.IntegerValue == (int)BuiltInCategory.OST_TitleBlocks);
            if (titleBlock is null)
                return null;
            elementsOnView.Remove(titleBlock);

            var newView = ViewSheet.Create(doc, titleBlock.GetTypeId());
            doc.Regenerate();

            var newTitleBlock = new FilteredElementCollector(doc, newView.Id)
                .ToElements()
                .FirstOrDefault(el => el.Category.Id.IntegerValue == (int)BuiltInCategory.OST_TitleBlocks);
            CopyParameters(newTitleBlock, titleBlock, false, "SheetNumber", "Номер листа");
            newTitleBlock.Location.Move(((LocationPoint)titleBlock.Location).Point);
            CopyParameters(newView, sheet, false, "Номер листа");
            newView.SheetNumber = sheet.SheetNumber + "_Copy";
            //newView.LookupParameter("Стадия на листе").Set("П");
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
                    //else if (ownerView.ViewType == ViewType.DraftingView || ownerView.ViewType == ViewType.Rendering
                    //|| ownerView.ViewType == ViewType.ThreeD
                    //)
                    //{
                    //    if (!ownerView.CanViewBeDuplicated(ViewDuplicateOption.WithDetailing))
                    //    {
                    //        continue;
                    //    }
                    //    dependedViewId = ownerView.Duplicate(ViewDuplicateOption.WithDetailing);
                    //}
                    else
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
                else if (el is ScheduleSheetInstance || el.Category.Id.IntegerValue == (int)BuiltInCategory.OST_RasterImages)
                {
                    elementsToCopy.Add(el.Id);
                }

            }
            if (elementsToCopy.Any())
                ElementTransformUtils.CopyElements(sheet, elementsToCopy, newView, null, null);
            return newView;
        }

    private static Result CopyParameters(Element to, Element from, bool openTransaction = false, params string[] excludedParams)
    {
            var result = Result.Failed;
            if (openTransaction)
            {
                using (var tr = new Transaction(from.Document))
                {
                    tr.Start("CopyParameters");
                    result = CopyParameters(to, from, false, excludedParams);
                    tr.Commit();
                }
            }
            else
            {
                var fromParametersMap = from.ParametersMap;
                var toParameterMap = to.ParametersMap;
                foreach (Parameter toParam in toParameterMap)
                {
                    if (toParam.IsReadOnly)
                        continue;
                    if (!fromParametersMap.Contains(toParam.Definition.Name))
                        continue;
                    if (excludedParams.Contains(toParam.Definition.Name))
                        continue;
                    var fromParam = fromParametersMap.get_Item(toParam.Definition.Name);
                    if (!fromParam.HasValue)
                        continue;
                    var value = fromParam.GetValueDynamic();
                    if (value is null)
                        continue;
                    var flag = toParam.Set(value);
                }
            }
            return result;
        }
}