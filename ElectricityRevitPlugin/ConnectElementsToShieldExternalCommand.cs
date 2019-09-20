using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class ConnectElementsToShieldExternalCommand : IExternalCommand
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
                    tr.Start("ConnectElementsToShield");
                    var selection = uiDoc.Selection;
                    var selectedIds = selection.GetElementIds();
                    var selectedElements = selectedIds
                        .Select(x => doc.GetElement(x))
                        .OfType<FamilyInstance>()
                        .ToArray();
                    var shield = selectedElements.FirstOrDefault(x =>
                        x.Category.Id.IntegerValue == (int) BuiltInCategory.OST_ElectricalEquipment);
                    if (shield is null)
                    {
                        throw new NullReferenceException("Следует выбрать щит и элементы");
                    }
                    foreach (var element in selectedElements)
                    {
                        if(element == shield)
                            continue;

                        var nEs = ElectricalSystem.Create(doc, new List<ElementId>() { element.Id },
                            ElectricalSystemType.PowerCircuit);
                        nEs.SelectPanel(shield);
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
