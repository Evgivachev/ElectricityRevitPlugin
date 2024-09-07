namespace ElectricityRevitPlugin.UpdateParametersInCircuits;

using System;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using MoreLinq;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
class SetParametersOfElSystemsCurrentPowerSystemAndType : IExternalCommand, IUpdaterParameters<ElectricalSystem>
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiApp = commandData.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        var result = Result.Succeeded;
        var electricalSystems = new FilteredElementCollector(doc)
            .OfClass(typeof(ElectricalSystem))
            .WhereElementIsNotElementType()
            .OfType<ElectricalSystem>();
        try
        {
            using (var tr = new Transaction(doc))
            {
                tr.Start("UpdateParametersOfElectricalSystem");
                foreach (var el in electricalSystems)
                    UpdateParameters(el);
                tr.Commit();
            }
        }
        catch (Exception e)
        {
            message += e.Message + '\n' + e.StackTrace;
            result = Result.Failed;
        }
        finally
        {
        }

        return result;
    }

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
