namespace MarkingElectricalSystems.Services;

using System.Collections.Generic;
using System.Linq;
using Abstractions;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;

public class AllElectricalSystemProvider : IElSystemsProvider
{
    public IEnumerable<ElectricalSystem> GetElectricalSystems()
    {
        var uiApp = ShiftElectricalCircuits.ExternalCommandData.Application;
        var app = uiApp.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        var ess = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_ElectricalCircuit)
                .OfType<ElectricalSystem>()
            ;
        return ess;
    }
}
