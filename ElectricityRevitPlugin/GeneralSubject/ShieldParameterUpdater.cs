namespace ElectricityRevitPlugin.GeneralSubject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.Revit.DB;
    using Extensions;

    class ShieldParameterUpdater : ParameterUpdater
    {
        public ShieldParameterUpdater()
        {
        }

        public ShieldParameterUpdater(Element fromElement)
            : base(fromElement)
        {
            FuncParametricDictionary = new Dictionary<string, Func<object, dynamic>>
            {
                {
                    "Имя панели", (s) =>
                    {
                        var shield = (FamilyInstance)s;
                        var value = shield.Name;
                        return value;
                    }
                },
                {
                    "Тип ОУ1", (s) =>
                    {
                        var shield = (FamilyInstance)s;
                        var value = shield.LookupParameter("Тип вводного автомата")?.AsString() ?? "";
                        return value;
                    }
                },
            };
        }

        public override CollectionOfCheckableItems GetValidateElements(Document document)
        {
            var elss = new FilteredElementCollector(document)
                    .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                    .WhereElementIsNotElementType()
                    .OfType<FamilyInstance>()
                    .Select(x => new { Family = x, Group = x.GetPowerElectricalSystem()?.GetGroupByGost() })
                    .OrderBy(x => x.Group?.Length)
                    .ThenBy(x => x.Group)
                    .Select(x => x.Family)
                    .GroupBy(x => x.get_Parameter(BuiltInParameter.RBS_ELEC_PANEL_SUPPLY_FROM_PARAM).AsString())
                    .OrderBy(x => x.Key)
                ;
            var result = new CollectionOfCheckableItems();
            foreach (var group in elss)
            {
                var item = new CheckableItem()
                {
                    Name = group.Key,
                    Item = group.Key,
                    IsChecked = false
                };
                result.Add(item);
                foreach (var shield in group)
                {
                    var child = new CheckableItem(item)
                    {
                        Name = shield.Name,
                        Item = shield,
                        IsChecked = false
                    };
                    item.Children.Add(child);
                }
            }

            return result;
        }
    }
}
