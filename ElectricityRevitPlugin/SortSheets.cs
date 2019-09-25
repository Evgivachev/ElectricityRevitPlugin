using System;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ElectricityRevitPlugin.Extensions;
using MoreLinq;

namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SortSheets : IExternalCommand
    {
        private Guid _listManuallyNumberParameterGuid = new Guid("c7687445-c508-4eb1-aefd-3ae0c9e6abfa");
        private Guid _endToEndNumberingParameterGuid = new Guid("88cb0f2b-89b8-4158-8f56-eb605da286c6");
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
                    .Select(x => x.get_Parameter(_listManuallyNumberParameterGuid)
                        .AsDouble())
                    .ToArray();
                var minNumber = numbers.Min();
                minNumber = minNumber == 0 ? 1 : minNumber;
                var maxNumber = numbers.Max();
                var sortedSheets = allSheet
                    .OrderBy(x =>
                    {
                        var param = x.get_Parameter(_listManuallyNumberParameterGuid);
                        if (param.HasValue)
                            return param.AsInteger();
                        return maxNumber + 1;
                    })
                    .ThenBy(x => x.SheetNumber.Length)
                    .ThenBy(x => x.SheetNumber)
                    .ToArray();
                var currentNumber = minNumber;

                var indexEndToEndNumbering = 0;
                double? minEndToEndNumbering = null;
                for (int i = 0; i < sortedSheets.Length; i++)
                {
                    var param = sortedSheets[i].get_Parameter(_endToEndNumberingParameterGuid);
                    if(!param.HasValue)
                        continue;
                    var endToEndNumber = param.AsDouble();
                    if (!minEndToEndNumbering.HasValue || minEndToEndNumbering.Value > endToEndNumber)
                    {
                        indexEndToEndNumbering = i;
                        minEndToEndNumbering = endToEndNumber;
                    }
                }
                using (var tr = new Transaction(doc))
                {
                    tr.Start("Задание номеров листов");
                    for (var index = 0; index < sortedSheets.Length; index++)
                    {
                        var sheet = sortedSheets[index];
                        var newNumber = currentNumber++;
                        var param = sheet.get_Parameter(_listManuallyNumberParameterGuid);
                        var sheetName = sheet.Name;
                        var flag = param.Set(newNumber);
                        var grOfSheet = sheet.LookupParameter(grName).AsString();

                        var repeat = 0;
                        while (true)
                        {
                            try
                            {
                                sheet.SheetNumber = $"{grOfSheet}_{newNumber}" + (repeat == 0 ? "" : $"({repeat})");
                                break;
                            }
                            catch (Exception e)
                            {
                                repeat++;
                            }
                        }

                        if (minEndToEndNumbering.HasValue)
                        {
                            var endToEndNumberingParam = sheet.get_Parameter(_endToEndNumberingParameterGuid);
                            endToEndNumberingParam.Set(minEndToEndNumbering.Value - indexEndToEndNumbering + index);
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