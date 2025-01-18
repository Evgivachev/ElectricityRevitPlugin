namespace CommonUpdateCmd.Infrastructure.UpdateElectricalSystem;

using System;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using MoreLinq;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class SetParametersOfElSystemsCurrentPowerSystemAndType : IUpdaterParameters<ElectricalSystem>
{
    public string UpdateParameters(ElectricalSystem els)
    {
        //Подключенные щиты
        var connectedShields = els
            .Elements
            .OfType<FamilyInstance>()
            .Where(el => el.Category.Id.IntegerValue == (int)BuiltInCategory.OST_ElectricalEquipment);
        var selectedPanel = connectedShields
            .MaxBy(sh =>
            {
                var i = sh.LookupParameter("Уставка вводного устроуства").AsDouble();
                return i;
            }).FirstOrDefault();

        //Тип вводеного автомата String
        var typeOfInputDeviceParam = els.LookupParameter("Тип вводного автомата");
        //Double
        var currentOfInputDevice = els.LookupParameter("Уставка вводного устроуства");

        //ElementID
        var typeOfInputDeviceParamValue = selectedPanel?.LookupParameter("Вводное отключающее устройство").AsValueString() ?? "";
        //Double
        var currentOfInputDeviceValue = selectedPanel?.LookupParameter("Уставка вводного устроуства").AsDouble() ?? 0;
        typeOfInputDeviceParam.Set(typeOfInputDeviceParamValue);
        currentOfInputDevice.Set(currentOfInputDeviceValue);
        return null;
    }
}
