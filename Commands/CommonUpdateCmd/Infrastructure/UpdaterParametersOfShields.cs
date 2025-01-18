namespace CommonUpdateCmd.Infrastructure;

using System.Globalization;
using Autodesk.Revit.DB;
using IExternalCommand = Application.IExternalCommand;

public class UpdaterParametersOfShields(Document doc) : IExternalCommand
{
    public void Execute()
    {

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
        using var tr = new Transaction(doc);
        tr.Start("Установка параметров в щитах");
        foreach (var shield in shields)
        {
            var (maxCurrent, countOfModuls) = GetValuesFromShield(shield);
            var maxCurrentParameter = shield.LookupParameter("Максимальный ток ОУ на группах в щитах");
            var countOfModulsParameter = shield.LookupParameter("Количество модулей в щитах");
            if (maxCurrentParameter is null || countOfModulsParameter is null)
            {
                throw new Exception(
                    $"Отсутствуют параметры \"Максимальный ток ОУ на группах в щитах\" или \"Количество модулей в щитах\"");
            }

            if (!maxCurrentParameter.Set(maxCurrent) || !countOfModulsParameter.Set(countOfModuls))
            {
                throw new Exception(
                    $"Не удалось установить параметры \"Максимальный ток ОУ на группах в щитах\" или \"Количество модулей в щитах\" в щите {shield.Name}");
            }
        }

        tr.Commit();

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
        foreach (var system in assignedSystems)
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
