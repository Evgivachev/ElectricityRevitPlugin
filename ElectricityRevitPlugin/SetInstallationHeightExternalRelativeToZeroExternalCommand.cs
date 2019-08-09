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
    public class SetInstallationHeightExternalRelativeToZeroExternalCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var doc = uiDoc.Document;
            var app = uiApp.Application;
            var result = Result.Succeeded;
            var parameterName = "Высотная отметка";
            try
            {
                using (var tr = new Transaction(doc))
                {
                    tr.Start("Установка высоты установки элементов");
                    //var selection = uiDoc.Selection;
                    //var elementIds = selection.GetElementIds();

                    var sharedParameterApplicableRule = new SharedParameterApplicableRule(parameterName);
                    var elementParameterFilter = new ElementParameterFilter(sharedParameterApplicableRule);
                    var allElements = new FilteredElementCollector(doc)
                       .OfClass(typeof(FamilyInstance))
                       .WherePasses(elementParameterFilter)
                       .Cast<Element>();

                    foreach (var el in allElements)
                    {
                        try
                        {
                           // var el = doc.GetElement(elId);
                            var location = el.Location as LocationPoint;
                            if (location is null) continue;
                            var z = UnitUtils.ConvertFromInternalUnits(location.Point.Z, DisplayUnitType.DUT_METERS);
                            //z = Math.Round(z, 3);
                            var parameter = el.LookupParameter(parameterName);
                            if (parameter is null)
                                continue;
                            parameter.Set(z);
                            //var level = doc.GetElement(el.LevelId) as Level;
                            //if(level is null)
                            //{
                            //    parameter.Set(z);
                            //}
                            //else
                            //{
                            //    parameter.Set(z + UnitUtils.ConvertFromInternalUnits(level.Elevation, DisplayUnitType.DUT_METERS));
                            //}
                        }
                        catch
                        {

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
}
