namespace Diagrams.CableRoute
{
    using System.Collections.Generic;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Mechanical;

    public interface ICableTray
    {
        HashSet<ICableTray> IncidentTrays { get; }
        int Id { get; }
        IList<XYZ> GetPoints();
        IEnumerable<Element> GetIncidentElements();
        double DistanceTo(ICableTray ct);
        IEnumerable<XYZ> GetPointsToICableTray(ICableTray otherCableTray);
        Space GetSpace();
    }
}
