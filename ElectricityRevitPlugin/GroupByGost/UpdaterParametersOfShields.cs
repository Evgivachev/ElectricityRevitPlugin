namespace UpdateNameSpace
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;
    using Autodesk.Revit.UI;

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class UpdaterParametersOfShields : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var uiApp = commandData?.Application;
                var uiDoc = uiApp?.ActiveUIDocument;
                var app = uiApp?.Application;
                var doc = uiDoc?.Document;
                var shields = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                    .OfClass(typeof(FamilyInstance))
                    .OfType<FamilyInstance>()
                    .Where(x =>
                    {
                        var uString = x.LookupParameter("Напряжение в щите")?.AsValueString()?.Split(' ')[0];
                        if (uString is null) return false;
                        var uFlag = double.TryParse(uString, NumberStyles.AllowDecimalPoint,
                            CultureInfo.InvariantCulture,
                            out var u);
                        if (!uFlag || u < 200)
                            return false;
                        var name = x.Name;
                        if (name.Contains("ВРУ") || name.Contains("ППУ"))
                            return false;
                        return true;
                    });
                using (var tr = new Transaction(doc))
                {
                    tr.Start("Установка параметров в щитах");
                    foreach (var shield in shields)
                    {
                        var (maxCurrent, countOfModuls) = GetValuesFromShield(shield);
                        var maxCurrentParameter = shield.LookupParameter("Максимальный ток ОУ на группах в щитах");
                        var countOfModulsParameter = shield.LookupParameter("Количество модулей в щитах");
                        if (maxCurrentParameter is null || countOfModulsParameter is null)
                        {
                            message =
                                $"Отсутствуют параметры \"Максимальный ток ОУ на группах в щитах\" или \"Количество модулей в щитах\"";
                            return Result.Failed;
                        }

                        if (!maxCurrentParameter.Set(maxCurrent) || !countOfModulsParameter.Set(countOfModuls))
                        {
                            message =
                                $"Не удалось установить параметры \"Максимальный ток ОУ на группах в щитах\" или \"Количество модулей в щитах\" в щите {shield.Name}";
                            return Result.Failed;
                        }
                    }

                    tr.Commit();
                }

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                message += $"{e.Message}\n{e.StackTrace}";
                return Result.Failed;
            }
        }

        private (double, int) GetValuesFromShield(FamilyInstance shield)
        {
            var result = (MaxCurrent: 0.0, CountOfModuls: 0);
            //Количество модулей вводного устройства
            var countOfModulsInpupDevice = shield.LookupParameter("Количество модулей вводного устройства")?.AsDouble();
            if (countOfModulsInpupDevice.HasValue)
                result.CountOfModuls += (int)countOfModulsInpupDevice;
            var assignedSystems = shield?.MEPModel?.GetAssignedElectricalSystems();
            if (assignedSystems is null)
                return result;
            foreach (ElectricalSystem system in assignedSystems)
            {
                //Считаем количество модулей
                var countOfmoduls = new[]
                {
                    system.LookupParameter("Количество модулей ОУ1")?.AsDouble(),
                    system.LookupParameter("Количество модулей ОУ2")?.AsDouble()
                };
                foreach (var countOfmodul in countOfmoduls)
                {
                    if (countOfmodul.HasValue)
                        result.CountOfModuls += (int)countOfmodul;
                }

                //Определяем максимальный ток
                var current = system.LookupParameter("Номинальный ток")?.AsDouble();
                if (current.HasValue && current > result.MaxCurrent)
                    result.MaxCurrent = current.Value;
            }

            return result;
        }
    }
}
