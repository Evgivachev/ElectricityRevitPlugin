namespace ElectricityRevitPlugin;

using System;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
class MarkingOpeningExternalCommand : DefaultExternalCommand
{
    protected override Result DoWork(ref string message, ElementSet elements)
    {
        var familyId = new ElementId(25767172);
        var family = Doc.GetElement(familyId) as Family;
        var symbolIds = family.GetFamilySymbolIds();
        var filter = new FamilySymbolFilter(symbolIds.First());
        var openings = new FilteredElementCollector(Doc)
                .OfCategory(BuiltInCategory.OST_GenericModel)
                .WhereElementIsNotElementType()
                .OfClass(typeof(FamilyInstance))
                .OfType<FamilyInstance>()
                .Where(x => x.GetTypeId() == symbolIds.First())
            ;
        try
        {
            using (var tr = new Transaction(Doc))
            {
                tr.Start("Temp");
                var n = 1;
                foreach (var opening in openings)
                {
                    var markParameter = opening.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
                    var heightParameter = opening.get_Parameter(new Guid("2a89f56e-07b4-4271-b4b3-8da8da3f6681"));
                    var shiftUp = opening.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM).AsDouble();
                    var shiftDown = opening.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM).AsDouble();
                    var levelDownId = opening.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).AsElementId();
                    var levelUpId = opening.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).AsElementId();
                    var levelDown = Doc.GetElement(levelDownId) as Level;
                    var levelUp = Doc.GetElement(levelUpId) as Level;
                    var elevationDown = levelDown.Elevation;
                    var elevationUp = levelUp.Elevation;
                    var height = elevationUp + shiftUp - elevationDown - shiftDown;
                    heightParameter.Set(height);
                    var width = UnitUtils.ConvertFromInternalUnits(
                        opening.get_Parameter(new Guid("4ba36057-8bb6-40c2-93e4-2258f77c1f27")).AsDouble(),
                        UnitTypeId.Millimeters);
                    height = UnitUtils.ConvertFromInternalUnits(
                        height,
                        UnitTypeId.Millimeters);
                    var length = UnitUtils.ConvertFromInternalUnits(
                        opening.get_Parameter(new Guid("ad30441a-c30a-4fcf-b9e0-e12964f0c7b7")).AsDouble(),
                        UnitTypeId.Millimeters);
                    var downHeightMark =
                        UnitUtils.ConvertFromInternalUnits(elevationDown + shiftDown, UnitTypeId.Meters);
                    var mark = $"Отв.{n}\n {length:F0}x{width:F0}x{height:F0}\nОтм. {downHeightMark:F3} м";
                    markParameter.Set(mark);
                    n++;
                }

                tr.Commit();
            }
        }
        catch (OperationCanceledException)
        {
            return Result.Failed;
        }

        return Result.Succeeded;
    }
}
