namespace MarkingElectricalSystems;

using System;
using Abstractions;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Services;
using Views;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class ShiftElectricalCircuits : IExternalCommand
{
    public static ExternalCommandData ExternalCommandData;
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        if (ExternalCommandData is null)
            ExternalCommandData = commandData;
        try
        {
            var uiApp = commandData.Application;
            var app = uiApp.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var doc = uiDoc.Document;

            var tuple = GetParameters();
            var shift = tuple.Item1;
            var flag = tuple.Item2;
            if (!shift.HasValue || !flag.HasValue)
                return Result.Cancelled;
            IElSystemsProvider elSystemsProvider;
            if(flag.Value)
                elSystemsProvider = new AllElectricalSystemProvider();
            else
            {
                elSystemsProvider = new SelectionElSystemsProvider();
            }
            var elSystems = elSystemsProvider.GetElectricalSystems();
            var processing = new ElSystemShiftProcessingV4();
            processing.Process(elSystems,shift.Value);


            return Result.Succeeded;
        }
        catch (Exception ex)
        {
            message += $"{ex.Message}\n{ex.StackTrace}";
            return Result.Failed;
        }

    }

    private Tuple<double?,bool?> GetParameters()
    {
        var window = new GetShiftView();
        window.ShowDialog();
        var sh =(double?) window?.TextBox?.Tag;
        var shift = sh / 1000 / 0.3048;
        return new Tuple<double?, bool?>(shift,window?.AllElementsRadioButton?.IsChecked);
    }
}