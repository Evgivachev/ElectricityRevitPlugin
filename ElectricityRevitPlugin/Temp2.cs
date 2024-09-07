//using MoreLinq.Extensions;

namespace ElectricityRevitPlugin;

using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class Temp2 : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiApp = commandData.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        var result = Result.Succeeded;
        new FilteredElementCollector(doc)
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
            var allFrames = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_TitleBlocks)
                .WhereElementIsNotElementType()
                .ToElements();
            foreach (var frame in allFrames)
            {
                var ownerViewId = frame.OwnerViewId.IntegerValue;
                var ownerView = doc.GetElement(new ElementId(ownerViewId));
                if (!lists.Contains(ownerViewId))
                    continue;
                var isEdit = frame.LookupParameter("Изменения").AsInteger() == 1;
                if (!isEdit)
                    continue;
                var editNumber = "_Номер изменения";
                var editName = "_Лист";
                ownerView.LookupParameter("Примечание").Set("");
                var en = string.Empty;
                var edit = string.Empty;
                for (var i = 1; i < 5; i++)
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
