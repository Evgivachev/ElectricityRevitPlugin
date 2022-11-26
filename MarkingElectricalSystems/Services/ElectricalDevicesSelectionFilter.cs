namespace MarkingElectricalSystems.Services;

using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

class ElectricalDevicesSelectionFilter : ISelectionFilter
{
    public bool AllowElement(Element elem)
    {
        var type = elem.Category;
        var trueCatugories = new[]
        {
            BuiltInCategory.OST_ElectricalEquipment,
            BuiltInCategory.OST_LightingFixtures,
            BuiltInCategory.OST_ElectricalFixtures
        };
        return trueCatugories.Contains((BuiltInCategory)type.Id.IntegerValue);
    }

    public bool AllowReference(Reference reference, XYZ position)
    {
        return false;
    }
}