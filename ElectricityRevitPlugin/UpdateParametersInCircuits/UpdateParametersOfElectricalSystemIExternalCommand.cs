﻿namespace ElectricityRevitPlugin.UpdateParametersInCircuits;

using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
//Тринити
class UpdateParametersOfElectricalSystemIExternalCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiApp = commandData.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        var result = Result.Succeeded;
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
        try
        {
            using (var tr = new Transaction(doc))
            {
                tr.Start("UpdateParametersOfElectricalSystem");
                parameterUpdater.Execute();
                tr.Commit();
            }
        }
        catch (Exception e)
        {
            message += e.Message + '\n' + e.StackTrace;
            result = Result.Failed;
        }

        return result;
    }
}
