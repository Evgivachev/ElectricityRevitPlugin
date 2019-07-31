using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ElectricityRevitPlugin
{
[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
    public class SetElementCoordinatesExternalCommand :IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var app = uiApp.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var doc = uiDoc.Document;

            var selection = uiDoc.Selection;
            var selectedElements = selection.GetElementIds();
            var element = doc.GetElement(selectedElements.First());
            var q = GetCoordinateFromUserWpf.GetCoordinate(element);
            return Result.Succeeded;
            

        }
    }
}
