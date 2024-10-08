﻿namespace ElectricityRevitPlugin;

using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
class Temp5 : IExternalCommand
{
    private Guid _projectSectionParameterGuid = new("ffe3351b-555f-40fb-86fd-4e5a4a446d27");

    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiApp = commandData.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        var result = Result.Succeeded;
        try
        {
            using (var tr = new Transaction(doc))
            {
                tr.Start("Установка параметров раздел проектирования для цепей");
                var sharedParameterApplicableRule = new SharedParameterApplicableRule("Раздел проектирования");
                var elementParameterFilter = new ElementParameterFilter(sharedParameterApplicableRule);
                var allElements = new FilteredElementCollector(doc)
                    .WherePasses(elementParameterFilter);
                foreach (var el in allElements)
                {
                    var param = el.get_Parameter(_projectSectionParameterGuid);
                    if (param.IsReadOnly || param is null)
                        continue;
                    param.AsString();
                    var value = "_@";
                    switch (param.AsString())
                    {
                        case "_@":
                            continue;
                        case "ТЭ_ПП":
                            continue;
                        case "ПП@":
                            continue;
                        case "ТЭ@":
                            continue;
                        case "ЭОМ":
                            value = "ЭОМ@";
                            break;
                        case "ЭОМ@":
                            continue;
                        case "ТЭ ПП":
                            value = "ТЭ_ПП@";
                            break;
                        case "ТЭ ПП@":
                            value = "ТЭ_ПП@";
                            break;
                        case "ПП_ЭОМ@":
                            value = "ПП@";
                            break;
                        case "ПП ЭОМ@":
                            value = "ПП@";
                            break;
                        case "АО@":
                            continue;
                        default:
                            continue;
                    }

                    try
                    {
                        param.Set(value);
                    }
                    catch (Exception)
                    {
                        // ignored
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
        finally
        {
        }

        return result;
    }
}
