namespace GeneralSubjectDiagram.Services.ParametersUpdaters
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;
    using CommonUtils.Extensions;
    using CommonUtils.Helpers;
    using JetBrains.Annotations;
    using ViewModels;

    [UsedImplicitly]
    public class CableParameterUpdater : ParameterUpdater
    {
        public CableParameterUpdater()
        {
            ParametersDictionary = new Dictionary<dynamic, dynamic>
            {
                //Коэффициент мощности
                { BuiltInParameter.RBS_ELEC_POWER_FACTOR, new Guid("2ca28edf-3aaf-486a-830a-fae82079832d") },
            };
            FuncParametricDictionary = new Dictionary<string, Func<object, dynamic>>
            {
                {
                    "Расчетный ток", e =>
                    {
                        var es = (ElectricalSystem)e;
                        //Ток в щитах
                        var p = es.get_Parameter(new Guid("86d2b171-cfb3-4fcf-811f-38d9a253a297")).AsDouble();
                        //Питающая сеть (не групповая)
                        var isPowerSystem = es
                            .Elements
                            .Cast<Element>()
                            .Any(el => el.Category.Id.IntegerValue == (int)BuiltInCategory.OST_ElectricalEquipment);
                        if (isPowerSystem)
                            return p;
                        return es
                            .get_Parameter(BuiltInParameter.RBS_ELEC_APPARENT_CURRENT_PARAM)
                            .AsDouble();
                    }
                },
                {
                    "Расчетная активная мощность", obj =>
                    {
                        var es = (ElectricalSystem)obj;
                        var isPowerSystem = es
                            .Elements
                            .Cast<Element>()
                            .Any(el => el.Category.Id.IntegerValue == (int)BuiltInCategory.OST_ElectricalEquipment);
                        if (isPowerSystem)
                            //Активная мощность в щитах
                            return UnitUtils.ConvertToInternalUnits(
                                es.get_Parameter(new Guid("1a63996b-777a-471f-aa56-b91d1c1f7232")).AsDouble(), UnitTypeId.Kilowatts);
                        //Установленная мощность
                        return es.get_Parameter(BuiltInParameter.RBS_ELEC_TRUE_LOAD).AsDouble();
                    }
                }
            };
        }

        public override ObservableCollection<CheckableItem> GetValidateElements(Document document)
        {
            var elss = new FilteredElementCollector(document)
                .OfCategory(BuiltInCategory.OST_ElectricalCircuit)
                .OfType<ElectricalSystem>()
                .GroupBy(x => x.PanelName ?? "???");
            var result = new ObservableCollection<CheckableItem>();
            foreach (var group in elss.OrderBy(x => x.Key))
            {
                var item = new CheckableItem()
                {
                    Name = group.Key,
                    Item = group.Key,
                    IsChecked = false
                };
                result.Add(item);
                foreach (var system in group.OrderBy(x => x.GetGroupByGost(), new RevitNameComparer()))
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

        /// <inheritdoc />
        public override string FamilyNameToInsert => "ВРУ.Кабель";

        public override FamilyInstance InsertInstance(FamilySymbol familySymbol, XYZ xyz)
        {
            var doc = familySymbol.Document;
            var line = Line.CreateBound(xyz, xyz + new XYZ(0, 0.1, 0));
            var instance = doc.Create.NewFamilyInstance(line, familySymbol, doc.ActiveView);
            return instance;
        }
    }
}
