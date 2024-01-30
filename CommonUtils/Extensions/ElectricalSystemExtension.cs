namespace CommonUtils.Extensions
{
    using Helpers;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;

    /// <summary>
    /// Методы расширения для <see cref="ElectricalSystem"/>
    /// </summary>
    public static class ElectricalSystemExtension
    {
        /// <summary>
        /// Возвращает значение параметра "Номер группы по ГОСТ"
        /// </summary>
        /// <param name="es"></param>
        public static string GetGroupByGost(this ElectricalSystem es)
        {
            return es.get_Parameter(SharedParametersFile.Nomer_Gruppy_Po_GOST).AsString();
        }

        /// <summary>
        /// Возвращает имя нагрузки для эл. цепи.
        /// </summary>
        /// <param name="es"></param>
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
                string? type;
                var category = fi.Category;
                if (category.Id == new ElementId(BuiltInCategory.OST_ElectricalEquipment))
                {
                    type = fi
                        .get_Parameter(BuiltInParameter.RBS_ELEC_PANEL_NAME)?
                        .AsString();
                }
                else
                {
                    var tryGetElectricalParameters =
                        fi.TryGetElectricalParameters(out var activePower, out _, out var loadClassification);
                    if (!tryGetElectricalParameters)
                        continue;
                    type = doc.GetElement(loadClassification).Name;
                }

                if (!string.IsNullOrEmpty(type) && type != "/" && type != "\\" && type != "Соединитель")
                {
                    types.Add(type!);
                }

                var space = fi.Space?
                    .Number;
                if (!string.IsNullOrEmpty(space))
                    spaces.Add(space!);
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