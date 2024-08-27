namespace ElectricityRevitPlugin;

using System;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
class SetProjectSectionForElectricalSystems : IExternalCommand
{
    private Guid _projectSectionParameterGuid = new("ffe3351b-555f-40fb-86fd-4e5a4a446d27");

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
                tr.Start("Установка параметров раздел проектирования для цепей");
                var sharedParameterApplicableRule = new SharedParameterApplicableRule("Раздел проектирования");
                var elementParameterFilter = new ElementParameterFilter(sharedParameterApplicableRule);
                var allElements = new FilteredElementCollector(doc)
                    .WherePasses(elementParameterFilter)
                    .OfClass(typeof(ElectricalSystem))
                    .OfType<ElectricalSystem>();
                foreach (var el in allElements)
                {
                    var param = el.get_Parameter(_projectSectionParameterGuid);
                    if (param is null)
                        continue;
                    var shield = el.BaseEquipment;
                    var shPS = shield.get_Parameter(_projectSectionParameterGuid).AsString();
                    if (string.IsNullOrEmpty(shPS))
                        param.Set("");
                    else
                        param.Set(shPS);
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
