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
            ParametersDictionary = new Dictionary<dynamic, dynamic>
            {
                //Установленная мощность
                //{ BuiltInParameter.RBS_ELEC_TRUE_LOAD, new Guid("9ebba55d-0d75-4556-8fcf-93b5362c3e27")},
                ////Расчётный ток 
                //{ BuiltInParameter.RBS_ELEC_APPARENT_CURRENT_PARAM, new Guid("3e12a7ce-cfff-44d3-8c9b-8a08095f6fcd") },

                //Коэффициент мощности
                { BuiltInParameter.RBS_ELEC_POWER_FACTOR, new Guid("2ca28edf-3aaf-486a-830a-fae82079832d")},
            };

            FuncParametricDictionary = new Dictionary<string, Func<object, dynamic>>
            {
                {"Расчетный ток", e =>
                {
                    var es = (ElectricalSystem) e;
                    //Ток в щитах
                    var p = es.get_Parameter(new Guid("86d2b171-cfb3-4fcf-811f-38d9a253a297")).AsDouble();
                    //Питающая сеть (не групповая)
                    var isPowerSystem = es
                        .Elements
                        .Cast<Element>()
                        .Any(el => el.Category.Id.IntegerValue == (int) BuiltInCategory.OST_ElectricalEquipment);
                    if (isPowerSystem)
                        return p;
                    return es
                        .get_Parameter(BuiltInParameter.RBS_ELEC_APPARENT_CURRENT_PARAM)
                        .AsDouble();
                }  },
                {"Расчетная активная мощность" , obj =>
                {
                    var es = (ElectricalSystem) obj;
                    var isPowerSystem = es
                        .Elements
                        .Cast<Element>()
                        .Any(el => el.Category.Id.IntegerValue == (int) BuiltInCategory.OST_ElectricalEquipment);
                    if (isPowerSystem)
                        //Активная мощность в щитах
                        return   UnitUtils.ConvertToInternalUnits(es.get_Parameter(new Guid("1a63996b-777a-471f-aa56-b91d1c1f7232")).AsDouble(),DisplayUnitType.DUT_KILOWATTS);
                    //Установленная мощность
                    return  es.get_Parameter(BuiltInParameter.RBS_ELEC_TRUE_LOAD).AsDouble();
                } }


            };

        }

        public override CollectionOfCheckableItems GetValidateElements(Document document)
        {
            var elss = new FilteredElementCollector(document)
                .OfCategory(BuiltInCategory.OST_ElectricalCircuit)
                .OfType<ElectricalSystem>()
                .OrderBy(x => x.PanelName)
                .ThenBy(x => x.GetGroupByGost(), new RevitNameComparer())
                .GroupBy(x => x.PanelName ?? "???")
                //.GroupBy(x => x.PanelName)
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
            var instance = Doc.Create.NewFamilyInstance(line, familySymbol, Doc.ActiveView);
            return instance;
        }
    }
}
