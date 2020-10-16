using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Temp10 :DefaultExternalCommand
    {
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            var selection = UiDoc.Selection;
            var viewIds = selection.GetElementIds();
            var views = viewIds
                .Select(Doc.GetElement)
                .OfType<ViewDrafting>();
            using (var tr = new Transaction(Doc, "Rename"))
            {
                tr.Start();


                foreach (var view in views)
                {
                    var name = view.Name;
                    var endWithPattern = new Regex(@"копия 1$");
                    if (endWithPattern.IsMatch(name))
                        view.Name = endWithPattern.Replace(name, "");
                    else
                    {
                        view.Name = name + " " + "Стадия П";
                    }
                }
                tr.Commit();
            }

            return Result.Succeeded;

        }
    }
}
