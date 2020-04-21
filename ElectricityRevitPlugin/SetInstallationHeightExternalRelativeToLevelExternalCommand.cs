using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SetInstallationHeightExternalRelativeToLevelExternalCommand : IExternalCommand
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
                    tr.Start("Установка высоты установки элементов относительно уровня");
                    var sharedParameterApplicableRule = new SharedParameterApplicableRule("Высота установки");
                    var elementParameterFilter = new ElementParameterFilter(sharedParameterApplicableRule);

                    var allElements = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilyInstance))
                        .WherePasses(elementParameterFilter)
                        .Cast<Element>();

                    foreach(var el in allElements)
                    {
                        var param = el.LookupParameter("Высота установки");
                        var value = el.GetInstallationHeightRelativeToLevel(DisplayUnitType.DUT_MILLIMETERS);
                       // value = Math.Round(value, 0);
                        param.Set(value);
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
}
