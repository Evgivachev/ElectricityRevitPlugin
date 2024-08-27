namespace ElectricityRevitPlugin.UpdateParametersInCircuits;

using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class SetModeOfElectricalSystemToAllElementsExternalCommand : IExternalCommand, IUpdaterParameters<ElectricalSystem>
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiApp = commandData.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        var app = uiApp.Application;
        var result = Result.Succeeded;
        try
        {
            using (var tr = new Transaction(doc))
            {
                tr.Start("Установка режима траектории электрической цепи на все устройства");
                var electricalSystems = new FilteredElementCollector(doc)
                    .OfClass(typeof(ElectricalSystem))
                    .WhereElementIsNotElementType()
                    .OfType<ElectricalSystem>();
                SetModeOfElectricalSystem(electricalSystems);
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

    public string UpdateParameters(ElectricalSystem el)
    {
        SetModeOfElectricalSystem(el);
        return null;
    }

    private void SetModeOfElectricalSystem(IEnumerable<ElectricalSystem> electricalSystems)
    {
        foreach (var system in electricalSystems)
        {
            var mode = system.CircuitPathMode;
            if (mode == ElectricalCircuitPathMode.FarthestDevice)
                system.CircuitPathMode = ElectricalCircuitPathMode.AllDevices;
        }
    }

    private void SetModeOfElectricalSystem(ElectricalSystem system)
    {
        var mode = system.CircuitPathMode;
        if (mode == ElectricalCircuitPathMode.FarthestDevice)
            system.CircuitPathMode = ElectricalCircuitPathMode.AllDevices;
    }
}
