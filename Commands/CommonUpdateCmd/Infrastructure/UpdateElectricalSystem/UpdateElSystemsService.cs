namespace CommonUpdateCmd.Infrastructure.UpdateElectricalSystem;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using CommonUpdateCmd.Application;

public class UpdateElSystemsService(UIApplication uiApplication) : IUpdateElSystemsService
{
    public void Execute()
    {
        var doc = uiApplication.ActiveUIDocument.Document;
        var parameterUpdater = new UpdaterParameters<ElectricalSystem>(doc, BuiltInCategory.OST_ElectricalCircuit);

        //Длина труб для спецификации
        parameterUpdater.AddAction(new SetLengthForElectricalSystemsExternalCommand());

        ////Режим траектории электрической цепи
        //parameterUpdater.AddAction(new SetModeOfElectricalSystemToAllElementsExternalCommand());

        //Тип вводного автомата Уставка вводного автомата
        parameterUpdater.AddAction(new SetParametersOfElSystemsCurrentPowerSystemAndType());
        //Обновление параметра Марка кабелей для выносок
        parameterUpdater.AddAction(new UpdateCablesMarkExternalCommand());
        //способ прокладки для схем - переписывает параметр из ключевой спецификации в параметр цепи
        parameterUpdater.AddAction(new UpdateCableManagementMethodExternalCommand());
        parameterUpdater.Execute();

        var categories = new[]
        {
            BuiltInCategory.OST_ElectricalEquipment, // эл оборудование
            BuiltInCategory.OST_ElectricalFixtures, // эл приборы
            BuiltInCategory.OST_LightingFixtures, // светильники
            BuiltInCategory.OST_LightingDevices, // выключатели
        };
        
        foreach (var category in categories)
        {
            var elementParameterUpdater = new UpdaterParameters<FamilyInstance>(doc, category);
            elementParameterUpdater.AddAction(new UpdateCablesMarkExternalCommand());
            elementParameterUpdater.Execute();
        }
    }
}
