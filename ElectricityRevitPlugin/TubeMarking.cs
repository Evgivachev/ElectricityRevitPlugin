namespace ElectricityRevitPlugin
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class TubeMarking : DefaultExternalCommand
    {
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            var allTubes = new FilteredElementCollector(Doc)
                .OfCategory(BuiltInCategory.OST_Conduit)
                .WhereElementIsNotElementType();
            var allTrays = new FilteredElementCollector(Doc)
                .OfCategory(BuiltInCategory.OST_CableTray)
                .WhereElementIsNotElementType();
            using (var tr = new Transaction(Doc))
            {
                tr.Start("Маркировка труб");
                foreach (var tube in allTubes)
                {
                    var name = tube.LookupParameter("Трубы_Наименование и технические характеристики")?.AsString();
                    if (name is null)
                        name = "";
                    var parameter = tube.LookupParameter("Лотки");
                    parameter?.Set(name);
                }

                foreach (var element in allTrays)
                {
                    var name = element.LookupParameter("Лотки Тип, марка, обозначение документа, опросного листа")?.AsString() ??
                               "";
                    var parameter = element.LookupParameter("Лотки");
                    parameter?.Set(name);
                }

                tr.Commit();
            }

            return Result.Succeeded;
        }
    }
}
