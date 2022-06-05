namespace ElectricityRevitPlugin.Short_Circuits
{
    using System;
    using System.Linq;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;
    using Autodesk.Revit.UI;

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class ShortCircuits : DefaultExternalCommand
    {
        private double _lowVoltage;
        private double _resistanceOfElectricalAcr;
        private double _resistanceOfElectricalContacts;

        protected override Result DoWork(ref string message, ElementSet elements)
        {
            //Получить значения общих параметров
            _resistanceOfElectricalAcr = Doc.ProjectInformation
                .get_Parameter(new Guid("d1df6d54-7620-4295-85f1-f8083961da61")).AsDouble();
            _resistanceOfElectricalContacts = Doc.ProjectInformation
                .get_Parameter(new Guid("a405f1a1-4105-4924-ad33-d73c3aac7b81")).AsDouble();

            //Получить все трансформаторы
            var transformers = new FilteredElementCollector(Doc)
                .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                .WhereElementIsNotElementType()
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .Where(x => x.Symbol.FamilyName == "Трансформатор");
            foreach (var transformer in transformers)
            {
                var r = transformer.Symbol.get_Parameter(new Guid("37e879f7-5971-40a8-8f77-7752d59771ad")).AsDouble();
                var x = transformer.Symbol.get_Parameter(new Guid("f7dfafba-c368-43c1-b37d-4e1b8b5ebb3f")).AsDouble();
                _lowVoltage =
                    UnitUtils.ConvertFromInternalUnits(transformer.Symbol.LookupParameter("Uнн").AsDouble(),
                        UnitTypeId.Volts);
                using (var tr = new Transaction(Doc))
                {
                    tr.Start("Расчёт токов 3кз");
                    SetParametersToElectricalSystemsInShield(null, transformer, r, x);
                    tr.Commit();
                }
            }

            return Result.Succeeded;
        }

        private void SetParametersToElectricalSystemsInShield(ElectricalSystem powerSystem, FamilyInstance panel, double r, double x)
        {
            if (powerSystem != null)
            {
                var (rPs, xPs) = GetResistanceOfElectricalSystem(powerSystem);
                r += rPs;
                x += xPs;
            }

            if (panel.LookupParameter("Вводное отключающее устройство").AsElementId() != null)
            {
                r += _resistanceOfElectricalContacts;
            }

            var connectedSystems = panel.MEPModel?.GetAssignedElectricalSystems();
            if (connectedSystems == null) return;
            foreach (ElectricalSystem system in connectedSystems)
            {
                var device1 = system.LookupParameter("Отключающее устройство 1").AsString();
                if (device1 != null && device1 != "-")
                    r += _resistanceOfElectricalContacts;
                var i = CalculateShortCircuits(system, r + _resistanceOfElectricalAcr, x);
                //Подключенное оборудование к цепи
                var connectedShields = system
                    .Elements
                    .Cast<FamilyInstance>()
                    .Where(shield => shield.Category.Id.IntegerValue == (int)BuiltInCategory.OST_ElectricalEquipment);
                foreach (var shield in connectedShields)
                {
                    SetParametersToElectricalSystemsInShield(system, shield, r, x);
                }
            }
        }

        private double CalculateShortCircuits(ElectricalSystem system, double r, double x)
        {
            //в амперах
            var i = _lowVoltage / Math.Sqrt(3 * (r * r + x * x)) * Math.Pow(10, 3);
            var p = system.get_Parameter(new Guid("2202784b-0ccb-4569-91b7-6f8d98fb7b72"));
            var internalCurrent = UnitUtils.ConvertToInternalUnits(i, UnitTypeId.Amperes);
            var flag = p.Set(internalCurrent);
            return i;
        }

        private (double r, double x) GetResistanceOfElectricalSystem(ElectricalSystem system)
        {
            var dr = system.LookupParameter("Активное сопротивление").AsDouble();
            var dx = system.LookupParameter("Индуктивное сопротивление").AsDouble();
            //Длина кабелей для ОС
            var length = system.get_Parameter(new Guid("387ba243-768e-45cf-9c22-ce1b5650fe3d")).AsDouble();
            var n = system.LookupParameter("Кол-во кабелей (провод) в одной группе").AsDouble();
            if (n < 1)
                throw new ArgumentException($"Неверное значение \"Кол-во кабелей (провод) в одной группе\" в цепи {system.CircuitNumber}");
            var r = dr / 1000 * length / n;
            var x = dx / 1000 * length / n;
            return (r, x);
        }
    }
}
