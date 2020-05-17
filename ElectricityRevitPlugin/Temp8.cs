using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class Temp8 : DefaultExternalCommand
    {
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            var elId = new ElementId(24839863);
            var el = Doc.GetElement(elId);
            var point = el.Location as LocationPoint;
            var selection = UiDoc.Selection;


            try
            {
                using (var tr = new Transaction(Doc))
                {
                    

                    while (true)
                    {
                        tr.Start("Вставка элементов");

                        var fromEl = selection.PickObject(ObjectType.LinkedElement, "Выбор связанного элемента");
                        var coord = fromEl.GlobalPoint;
                        var load = ElementTransformUtils.CopyElement(Doc, elId, coord-point?.Point);
                        tr.Commit();
                    }
                    
                }
            }

            catch (OperationCanceledException e)
            {
                return Result.Succeeded;
            }
        }
    }
}
