namespace Diagrams.CableRoute
{
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;
    using Autodesk.Revit.DB.Mechanical;

    public class MyCableTray : ICableTray
    {
        private readonly CableTray _cableTray;

        public MyCableTray(CableTray ct)
        {
            _cableTray = ct;
        }

        public HashSet<ICableTray> IncidentTrays { get; } = new();

        public IEnumerable<Element> GetIncidentElements()
        {
            var connectors = _cableTray.ConnectorManager
                .Connectors
                .OfType<Connector>()
                .SelectMany(c => c.AllRefs
                    .OfType<Connector>()
                    .Where(x => x.Owner.Id.IntegerValue != GetHashCode())
                );
            var owners = connectors
                .Select(c => c.Owner)
                .Distinct();
            return owners;
        }

        //TODO исправить данный метод!!!
        public double DistanceTo(ICableTray ct)
        {
            if (ct is MyCableTray)
            {
                var min = double.MaxValue;
                var result = GetPoints()
                    .SelectMany(
                        mp => ct
                            .GetPoints()
                            .Select(
                                op => op
                                    .DistanceTo(mp))
                    )
                    .Min();
                foreach (var myP in GetPoints())
                {
                    foreach (var point in ct.GetPoints())
                    {
                        var d = myP.DistanceTo(point);
                        if (d < min)
                            min = d;
                    }
                }

                return result;
            }

            var ds = ct.GetPoints()
                .Select(DistanceToPoint);
            return ds.Min();
        }

        public int Id => _cableTray.Id.IntegerValue;

        public IEnumerable<XYZ> GetPointsToICableTray(ICableTray otherCableTray)
        {
            if (otherCableTray is MyCableTrayFitting myCableTrayFitting)
            {
                var nearestPoint = myCableTrayFitting.GetNearestPoint(this);
                yield return nearestPoint;
                yield return myCableTrayFitting.GetPoints().First();
            }
            else
            {
                var otherTray = otherCableTray as MyCableTray;
                yield return GetNearestPoint(otherTray);
            }
        }


        Space ICableTray.GetSpace()
        {
            var doc = _cableTray.Document;
            var space = doc.GetSpaceAtPoint(GetPoints().First());
            return space;
        }

        public IList<XYZ> GetPoints()
        {
            return _cableTray
                .ConnectorManager.Connectors
                .OfType<Connector>()
                .Where(c => c.ConnectorType == ConnectorType.End)
                .Select(c => c.Origin)
                .ToList();
        }

        public double DistanceToPoint(XYZ point)
        {
            var ps = GetPoints().Take(2).ToArray();
            var p1 = ps[0];
            var p2 = ps[1];
            var scalar1 = (point - p1).DotProduct(p2 - p1);
            var scalar2 = (point - p2).DotProduct(p1 - p2);
            if (scalar1 < 0) return (p1 - point).GetLength();
            if (scalar2 < 0) return (p2 - point).GetLength();
            return (p1 - point).CrossProduct(p2 - point).GetLength() / (p1 - p2).GetLength();
        }

        private XYZ GetNearestPoint(MyCableTray otherMcTf)
        {
            var otherPoints = otherMcTf.GetPoints().Take(2);
            var myPoints = GetPoints().Take(2);
            XYZ result = null;
            var minD = double.MaxValue;
            foreach (var myP in myPoints)
            {
                foreach (var point in otherPoints)
                {
                    var d = myP.DistanceTo(point);
                    if (d < minD)
                    {
                        minD = d;
                        result = point;
                    }
                }
            }

            return result;
        }

        public ICollection<Space> GetSpace()
        {
            var doc = _cableTray.Document;
            var spaces = GetPoints().Select(p => doc.GetSpaceAtPoint(p)).ToArray();
            return spaces;
        }

        public override bool Equals(object obj)
        {
            if (obj is ICableTray ct)
                return GetHashCode() == ct.GetHashCode();

            return false;
        }

        public override int GetHashCode()
        {
            return _cableTray.Id.IntegerValue;
        }
    }
}
