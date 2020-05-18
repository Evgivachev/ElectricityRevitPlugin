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
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class Temp8 : DefaultExternalCommand
    {
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            var selection = UiDoc.Selection;
            //Элемент для копирования
            var elId = new ElementId(24837780);
            var el = Doc.GetElement(elId);
            //Выбор связи
            try
            {
                using (var tr = new Transaction(Doc))
                {
                    tr.Start("Копироваине элементов");
                    while (true)
                    {


                        var linkedEl = selection.PickObject(ObjectType.LinkedElement, "Выберите элемент");
                        var copy = ElementTransformUtils.CopyElement(Doc, elId, linkedEl.GlobalPoint);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                return Result.Succeeded;

            }

        }
    }
}
