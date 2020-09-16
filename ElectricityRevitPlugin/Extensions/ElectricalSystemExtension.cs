using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;

namespace ElectricityRevitPlugin.Extensions
{
    public static class ElectricalSystemExtension
    {
        private static readonly Guid ByGost  = new Guid("8d1b8079-3007-4140-835c-73f0de4e81bd");

        public static string GetGroupByGost(this ElectricalSystem es)
        {
            return es.get_Parameter(ByGost).AsString();
        }

        public static string GetLoadName(this ElectricalSystem es)
        {
            var doc = es.Document;
            var types = new HashSet<string>();
            var spaces = new HashSet<string>();
            var elements = es
                .Elements
                .Cast<FamilyInstance>();

            foreach (var fi in elements)
            {
                var type = string.Empty;
                var category = fi.Category;
                if (category.Id == new ElementId(BuiltInCategory.OST_ElectricalEquipment))
                {
                    type = fi
                        .get_Parameter(BuiltInParameter.RBS_ELEC_PANEL_NAME)?
                        .AsString();



                }
                else
                {
                    fi.GetElectricalParameters(out var activePower, out _, out var loadClassification);
                    if (activePower <= 0)
                        continue;
                    type = doc.GetElement(loadClassification).Name;
                }
                
                if (!string.IsNullOrEmpty(type) && (type != "/" && type != "\\" && type != "Соединитель"))
                {
                    types.Add(type);
                }
                var space = fi.Space?
                    .Number;

                if (!string.IsNullOrEmpty(space))
                    spaces.Add(space);
            }

            var result = new StringBuilder();
            result.Append(string.Join(", ", types));
            if (spaces.Count > 0)
                result.Append($" пом. ");
            result.Append(string.Join(", ", spaces.OrderBy(x => x)));
            var resultStr = result.ToString();
            return resultStr;
        }
    }
}
