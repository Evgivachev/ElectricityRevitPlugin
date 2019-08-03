using System;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ElectricityRevitPlugin.Extensions;

namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SortSheets : IExternalCommand
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
                var selectedSheet = selection.GetElementIds().Select(x => doc.GetElement(x) as ViewSheet)
                    .FirstOrDefault(x => x != null);
                if (selectedSheet is null)
                    return result;
                var grName = "Раздел проекта";
                var gr = selectedSheet.LookupParameter(grName).AsString();
                var allSheet = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_Sheets)
                    .WhereElementIsNotElementType()
                    .OfType<ViewSheet>()
                    .Where(x => x.LookupParameter(grName).AsString() == gr).ToArray();
                
                if (!allSheet.Any())
                    return result;
                var numbers = allSheet
                    .Select(x => x.LookupParameter("Номер листа (вручную)").AsInteger())
                    .ToArray();
                var minNumber = numbers.Min();
                minNumber = minNumber == 0 ? 1 : minNumber;
                var maxNumber = numbers.Max();
                var sortedSheets = allSheet
                    .OrderBy(x =>
                    {
                        var param = x.LookupParameter("Номер листа (вручную)");
                        if (param.HasValue)
                            return param.AsInteger();
                        return maxNumber + 1;
                    })
                    .ThenBy(x => x.SheetNumber.Length)
                    .ThenBy(x => x.SheetNumber);
                var currentNumber = minNumber;
                using (var tr = new Transaction(doc))
                {
                    tr.Start("Задание номеров листов");
                    foreach (var sheet in sortedSheets)
                    {
                        var newNumber = currentNumber++;
                        var q1 = sheet.GetSheetNumberManuallyString();
                        var param = sheet.LookupParameter("Номер листа (вручную)");
                        var sheetName = sheet.Name;
                        var flag = param.Set(newNumber);
                        var q = sheet.GetSheetNumberManuallyString();
                        var grOfSheet = sheet.LookupParameter(grName).AsString();

                        var repeat = 0;
                        while (true)
                        {
                            try
                            {
                                sheet.SheetNumber = $"{grOfSheet}_{newNumber}"+(repeat==0? "" : $"({repeat})");
                                break;

                            }
                            catch (Exception e)
                            {
                                repeat++;
                            }
                            
                        }
                        
                    }

                    tr.Commit();
                }
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