namespace MarkingElectricalSystems.Services;

using System.Collections.Generic;
using System.Linq;
using Abstractions;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Models;

public class SelectionElSystemsProvider : IElSystemsProvider
{
    public IEnumerable<ElectricalSystem> GetElectricalSystems()
    {
        var uiApp = ShiftElectricalCircuits.ExternalCommandData.Application;
        var app = uiApp.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        var selection = uiDoc.Selection;
        var selectedElements = selection.GetElementIds();
        var systems = selectedElements.SelectMany(elId =>
            {
                var el = doc.GetElement((ElementId)elId) as FamilyInstance;
                return el?.MEPModel?.GetElectricalSystems();
            })
            .Where(es => es is not null)
            .Distinct(new ElementComparer<ElectricalSystem>());
        return systems;
    }
}
