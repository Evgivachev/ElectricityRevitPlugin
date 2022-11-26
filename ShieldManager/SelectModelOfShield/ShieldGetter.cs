using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using ShieldPanel.ViewOfDevicesOfShield;
using Document = Autodesk.Revit.DB.Document;

namespace ShieldPanel.SelectModelOfShield
{
    public class ShieldGetter
    {
        private UIApplication UiApp;
        private UIDocument UiDoc;
        private Document Doc;
        private Application App;

        public ShieldGetter(ExternalCommandData cd)
        {
            UiApp = cd?.Application;
            UiDoc = UiApp?.ActiveUIDocument;
            Doc = UiDoc?.Document;
            App = UiApp?.Application;
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
            var es = sh.MEPModel?.AssignedElectricalSystems?.OfType<ElectricalSystem>();
            if (es is null)
            {
                return result;
            }
            foreach (var e in es)
            {
                var n1 = e.LookupParameter("Количество модулей ОУ1").AsDouble();
                var n2 = e.LookupParameter("Количество модулей ОУ2").AsDouble();
                result += (int)(n1 + n2);
            }

            return result;
        }
    }
}
