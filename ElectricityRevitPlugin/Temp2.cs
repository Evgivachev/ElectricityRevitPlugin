using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using MoreLinq.Extensions;

namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Temp2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var app = uiApp.Application;
            var doc = uiDoc.Document;
            var result = Result.Succeeded;
            var sheets = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Sheets)
                .Cast<ViewSheet>();
            using (var tr = new Transaction(doc))
            {
                tr.Start("sds");
                var selection = uiDoc.Selection;
                var selectedElementsIds = selection.GetElementIds();
                if (!selectedElementsIds.Any())
                    return result;
                var lists = selectedElementsIds
                    .Select(x => doc.GetElement(x) as ViewSheet)
                    .Where(x => x != null)
                    .Select(x => x.Id.IntegerValue)
                    .ToHashSet();

                var framesIds = new List<ElementId>();
                var allFrames = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_TitleBlocks)
                    .WhereElementIsNotElementType()
                    .ToElements();
                foreach (var frame in allFrames)
                {
                    var ownerViewId = frame.OwnerViewId.IntegerValue;
                    var ownerView = doc.GetElement(new ElementId(ownerViewId));
                    var viewName = ownerView.Name;
                    if (!lists.Contains(ownerViewId))
                        continue;
                    var isEdit = frame.LookupParameter("Изменения").AsInteger() == 1;
                    if (!isEdit)
                        continue;
                    var editNumber = "_Номер изменения";
                    var editName = "_Лист";
                    ownerView.LookupParameter("Примечание").Set("");
                    string en = string.Empty;
                    string edit = string.Empty;
                    for (int i = 1; i < 5; i++)
                    {
                        var ien = ownerView.LookupParameter($"{i}{editNumber}").AsString();
                        if (string.IsNullOrEmpty(ien))
                            break;

                        var iedit = ownerView.LookupParameter($"{i}{editName}").AsString();
                        if (string.IsNullOrEmpty(iedit))
                            break;
                        edit = iedit;
                        en = ien;
                    }
                    if (string.IsNullOrEmpty(en) || string.IsNullOrEmpty(edit))
                        continue;
                    ownerView.LookupParameter("Примечание").Set($"Изм.{en} ({edit}) ");
                }
                tr.Commit();
            }
            return result;
        }
    }
}
