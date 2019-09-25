using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ElectricityRevitPlugin.Extensions;

namespace ElectricityRevitPlugin
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    class SelectPowerElectricalSystemsFromSelectionExternalCommand :IExternalCommand
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
                var elementIds = selection
                    .GetElementIds();
                var elSystems = elementIds
                    .Select(e => doc.GetElement(e))
                    .Where(e => e != null)
                    .OfType<FamilyInstance>()
                    .Select(f => f.GetPowerElectricalSystem())
                    .Where(x=>x!=null);

                var elSystemIds = elSystems
                    .Select(x=>x.Id)
                    .ToArray();
                selection.SetElementIds(elSystemIds);

                //using (var trGr = new TransactionGroup(doc))
                //{
                //    trGr.Start(this.ToString());
                    
                //    var sheets = selection.Select(x => doc.GetElement(x))
                //        .OfType<ViewSheet>()
                //        .ToArray();
                //    if (!sheets.Any())
                //        throw new Exception("Следует выделить листы в диспетчере проекта");
                //    foreach (var view in sheets)
                //    {
                //        ExecuteOnTheViewSheet(commandData, ref message, elements, view, ShiftTitleBlock);
                //    }

                //    trGr.Assimilate();
                //}
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
