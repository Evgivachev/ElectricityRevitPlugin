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
    public class TubeMarking : DefaultExternalCommand
    {
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            var allTubes = new FilteredElementCollector(Doc)
                .OfCategory(BuiltInCategory.OST_Conduit);
            using (var tr = new Transaction(Doc))
            {
                tr.Start("Маркировка труб");
                foreach (var tube in allTubes)
                {
                    var name = tube.LookupParameter("Трубы_Наименование и технические характеристики")?.AsString();
                    if (name is null)
                        name = "";
                    var parameter = tube.LookupParameter("Лотки");
                    if(parameter is null)
                        continue;
                    parameter.Set(name);
                }
                tr.Commit();
            }

            return Result.Succeeded;
        }
    }
}
