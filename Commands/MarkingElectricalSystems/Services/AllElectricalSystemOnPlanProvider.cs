namespace MarkingElectricalSystems.Services;

using System.Collections.Generic;
using System.Linq;
using Abstractions;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;

public class AllElectricalSystemOnPlanProvider : IElSystemsProvider
{
    public IEnumerable<ElectricalSystem> GetElectricalSystems()
    {
        var uiApp = ShiftElectricalCircuits.ExternalCommandData.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        var plan = doc.ActiveView;
        var ess = new FilteredElementCollector(doc, plan.Id).OfCategory(BuiltInCategory.OST_ElectricalCircuit)
                .OfType<ElectricalSystem>()
            ;
        return ess;
    }
}
