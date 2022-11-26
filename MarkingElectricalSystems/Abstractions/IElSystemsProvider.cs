namespace MarkingElectricalSystems.Abstractions;

using System.Collections.Generic;
using Autodesk.Revit.DB.Electrical;

public interface IElSystemsProvider
{
    IEnumerable<ElectricalSystem> GetElectricalSystems();
}
