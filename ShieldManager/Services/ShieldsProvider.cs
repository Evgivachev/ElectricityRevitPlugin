namespace ShieldManager.Services;

using System.Collections.Generic;
using System.Linq;
using Abstractions;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Extensions;
using Models;

class ShieldsProvider : IShieldsProvider
{
    private readonly UIApplication _uiApplication;

    public ShieldsProvider(UIApplication uiApplication)
    {
        _uiApplication = uiApplication;
    }

    public IEnumerable<ShieldWrapper> GetShields()
    {
        var shields = new FilteredElementCollector(_uiApplication.ActiveUIDocument.Document)
            .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
            .OfType<FamilyInstance>()
            .Where(sh =>
            {
                if (sh.LookupParameter("К ВУ") is null)
                    return false;
                var u = sh.LookupParameter("Напряжение в щите")?.AsDouble().ConvertVolts();
                return u.HasValue && u.Value > 200;
            })
            .Select(s => new ShieldWrapper(s));
        return shields;
    }
}
