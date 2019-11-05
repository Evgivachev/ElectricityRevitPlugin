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

namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Temp3 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var app = uiApp.Application;
            var doc = uiDoc.Document;
            var result = Result.Succeeded;
            var systems = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_ElectricalCircuit)
                .Cast<ElectricalSystem>();
            using (var tr = new Transaction(doc))
            {
                tr.Start("sds");
                foreach (var s in systems)
                {
                    // var name = sheet.Name;
                    var type = s.LookupParameter("Марка кабеля");
                    var typeString = type.AsValueString();
                    if (typeString is null || typeString.Contains("LSLTx"))
                    {
                        type.Set(new ElementId(22411615));
                    }
                }
                tr.Commit();
            }

            return result;

        }
    }
}
