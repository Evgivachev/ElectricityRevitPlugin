using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using ShieldPanel.ViewOfDevicesOfShield;
using Document = Autodesk.Revit.DB.Document;

namespace ShieldPanel.SelectModelOfShield;

public class ShieldGetter
{
    private readonly Document Doc;

    public ShieldGetter(ExternalCommandData cd)
    {
        var uiApp = cd?.Application;
        var uiDoc = uiApp?.ActiveUIDocument;
        Doc = uiDoc?.Document;
    }

    public IEnumerable<FamilyInstance> GetShields()
    {
        var shields = new FilteredElementCollector(Doc)
            .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
            .OfType<FamilyInstance>()
            .Where(sh =>
            {

                if (sh.LookupParameter("К ВУ") is null)
                    return false;
                var u = sh.LookupParameter("Напряжение в щите")?.AsDouble().ConvertVolts();
                return u.HasValue && u.Value > 200;
            });

        return shields;
    }

    public static int GetNumberOfModules(FamilyInstance sh)
    {
        var result = 0;
        var es = sh.MEPModel?.GetAssignedElectricalSystems()?.OfType<ElectricalSystem>();
        if (es is null)
            return result;
        foreach (var e in es)
        {
            var n1 = e.LookupParameter("Количество модулей ОУ1").AsDouble();
            var n2 = e.LookupParameter("Количество модулей ОУ2").AsDouble();
            result += (int)(n1 + n2);
        }

        return result;
    }
}
