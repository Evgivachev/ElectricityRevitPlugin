using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using ElectricityRevitPlugin.Extensions;

namespace ElectricityRevitPlugin.GeneralSubject
{
    public class CableParameterUpdater : ParameterUpdater
    {
        public CableParameterUpdater() : base()
        { }
        public CableParameterUpdater(Element fromElement) : base(fromElement)
        {
            var doc = fromElement.Document;

            ParametersDictionary = new Dictionary<dynamic, dynamic>
            {
                //Установленная мощность
                { BuiltInParameter.RBS_ELEC_TRUE_LOAD, new Guid("9ebba55d-0d75-4556-8fcf-93b5362c3e27")},
                //Расчётный ток
                { BuiltInParameter.RBS_ELEC_APPARENT_CURRENT_PARAM, new Guid("3e12a7ce-cfff-44d3-8c9b-8a08095f6fcd") },
                //Коэффициент мощности
                { BuiltInParameter.RBS_ELEC_POWER_FACTOR, new Guid("2ca28edf-3aaf-486a-830a-fae82079832d")},
            };
        }

        public override MyCollectionOfCheckableItems GetValidateElements(Document document)
        {
            var elss = new FilteredElementCollector(document)
                .OfCategory(BuiltInCategory.OST_ElectricalCircuit)
                .OfType<ElectricalSystem>()
                .OrderBy(x=>x.PanelName)
                .ThenBy(x=>x.GetGroupByGost(),new RevitNameComparer())
                .GroupBy(x=> x.PanelName??"???")
                //.GroupBy(x => x.PanelName)
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
                foreach (var system in group)
                {
                    var child = new CheckableItem(item)
                    {
                        Name = system.GetGroupByGost(),
                        Item = system,
                        IsChecked = false
                    };
                    item.Children.Add(child);
                }
            }
            return result;
        }

        public override FamilyInstance InsertInstance(FamilySymbol familySymbol, XYZ xyz)
        {
            var line = Line.CreateBound(xyz, xyz + new XYZ(0, 0.1, 0));
           var instance = Doc.Create.NewFamilyInstance(line,familySymbol, Doc.ActiveView);
            return instance;
        }
    }
}
