using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using ElectricityRevitPlugin.Extensions;

namespace ElectricityRevitPlugin.GeneralSubject
{
    class ShieldParameterUpdater : ParameterUpdater
    {
        public ShieldParameterUpdater()
        {
        }
        public ShieldParameterUpdater(Element fromElement) : base(fromElement)
        {
            FuncParametricDictionary = new Dictionary<string, Func<object, dynamic>>
            {
                { "Имя панели", (s) =>
                    {
                        var shield = (FamilyInstance) s;
                        var value = shield.Name;
                        return value;
                    }
                },

                { "Тип ОУ1", (s) =>
                    {
                        var shield = (FamilyInstance) s;

                        var value = shield.LookupParameter("Тип вводного автомата").AsString();
                        return value;
                    }
                },

            };
        }

        public override MyCollectionOfCheckableItems GetValidateElements(Document document)
        {
            var elss = new FilteredElementCollector(document)
                    .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                    .WhereElementIsNotElementType()
                    .OfType<FamilyInstance>()
                    .OrderBy(x=>x.GetPowerElectricalSystem()?.GetGroupByGost())
                    .GroupBy(x=>x.get_Parameter(BuiltInParameter.RBS_ELEC_PANEL_SUPPLY_FROM_PARAM).AsString())
                    .OrderBy(x=>x.Key)
                ;
            var result = new MyCollectionOfCheckableItems();
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
