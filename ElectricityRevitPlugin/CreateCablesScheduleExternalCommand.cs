namespace ElectricityRevitPlugin
{
    using System.Linq;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;
    using Autodesk.Revit.UI;
    using CommonUtils.Helpers;
    using Extensions;

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class CreateCablesScheduleExternalCommand : DefaultExternalCommand
    {
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            var circuitsGroup = new FilteredElementCollector(Doc)
                .OfCategory(BuiltInCategory.OST_ElectricalCircuit)
                .Cast<ElectricalSystem>()
                .GroupBy(x => x.get_Parameter(SharedParametersFile.Razdel_Proektirovaniya).AsString());
            using (var tr = new Transaction(Doc, "CableSchedule"))
            {
                tr.Start();
                foreach (var group in circuitsGroup)
                {
                    var pathName = group.Key;
                    var number = 1;
                    foreach (var electricalSystem in group.OrderBy(x => x.PanelName)
                                 .ThenBy(x => x.LookupParameter("Номер QF").AsString()))
                    {
                        var nameInCableScheduleParameter =
                            electricalSystem.get_Parameter(SharedParametersFile.Oboznachenie_Kabelya_V_KZH);
                        var nameTubeInCableScheduleParameter =
                            electricalSystem.get_Parameter(SharedParametersFile.Oboznachenie_Dlya_Trub_V_KZH);
                        var groupByGost = electricalSystem.get_Parameter(SharedParametersFile.Nomer_Gruppy_Po_GOST)
                            .AsString();
                        var isNotInSchedule = IsNotInSchedule(electricalSystem);
                        if (isNotInSchedule)
                        {
                            nameInCableScheduleParameter.SetEmptyValue();
                            nameTubeInCableScheduleParameter.SetEmptyValue();
                            continue;
                        }

                        var nameInCableSchedule = $"{groupByGost}-{number}";
                        nameInCableScheduleParameter.Set(nameInCableSchedule);
                        nameTubeInCableScheduleParameter.Set($"T-{number}");
                        number++;
                    }
                }

                tr.Commit();
            }

            return Result.Succeeded;
        }

        private bool IsNotInSchedule(ElectricalSystem es)
        {
            var isReserve = es.get_Parameter(SharedParametersFile.Rezervnaya_Gruppa).AsInteger() > 0;
            var isControl = es.get_Parameter(SharedParametersFile.Kontrolnye_TSepi).AsInteger() > 0;
            //var voltage = es.get_Parameter(BuiltInParameter.RBS_ELEC_VOLTAGE).AsDouble();
            //voltage = UnitUtils.ConvertFromInternalUnits(voltage, DisplayUnitType.DUT_VOLTS);
            return isReserve || isControl;
        }
    }
}
