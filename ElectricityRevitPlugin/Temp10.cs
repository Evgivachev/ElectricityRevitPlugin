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
    class Temp10 : DefaultExternalCommand
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
                .Where(x=>x.GetTypeId() == symbolIds.First())
                ;

            try
            {
                using (var tr = new Transaction(Doc))
                {
                    tr.Start("Temp");
                    var n = 1;
                    foreach (var opening  in openings)
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
                            DisplayUnitType.DUT_MILLIMETERS);
                        height = UnitUtils.ConvertFromInternalUnits(
                            height,
                            DisplayUnitType.DUT_MILLIMETERS);
                        var length = UnitUtils.ConvertFromInternalUnits(
                            opening.get_Parameter(new Guid("ad30441a-c30a-4fcf-b9e0-e12964f0c7b7")).AsDouble(),
                            DisplayUnitType.DUT_MILLIMETERS);

                        var downHeightMark =
                            UnitUtils.ConvertFromInternalUnits(elevationDown + shiftDown, DisplayUnitType.DUT_METERS);
                        var mark = $"Отв.{n}\n {length:F0}x{width:F0}x{height:F0}\nОтм. {downHeightMark:F3} м";
                        markParameter.Set(mark);
                        n++;
                    }
                    tr.Commit();
                }
            }

            catch (OperationCanceledException e)
            {
                return Result.Failed;
            }

            return Result.Succeeded;
        }
    }
}