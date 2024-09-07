namespace ElectricityRevitPlugin;

using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

[Regeneration(RegenerationOption.Manual)]
[Transaction(TransactionMode.Manual)]
class SpotDimensionCreatorExternalCommand : DefaultExternalCommand
{
    protected override Result DoWork(ref string message, ElementSet elements)
    {
        var selection = UiDoc.Selection;
        var lightingFixtures = selection.GetElementIds()
            .Select(id => Doc.GetElement(id))
            .Where(el => el.Category.Id.IntegerValue == (int)BuiltInCategory.OST_LightingFixtures)
            .Cast<FamilyInstance>();
        using (var tr = new Transaction(Doc))
        {
            tr.Start("Создание высотной отметки");
            foreach (var fixture in lightingFixtures)
            {
                var view = Doc.ActiveView;
                var reference = new Reference(fixture);
                var originLocationPoint = fixture.Location as LocationPoint;
                var origin = originLocationPoint.Point;
                var bend = origin + new XYZ(0, 1, 4);
                var end = bend + new XYZ(0, 2, 4);
                Doc.Create.NewSpotElevation(view, reference, origin, bend, end, origin, false);
            }

            tr.Commit();
        }

        return Result.Succeeded;
    }
}
