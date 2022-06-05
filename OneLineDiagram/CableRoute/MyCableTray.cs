namespace Diagrams.CableRoute
{
    using System;
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

        private Line Line => (_cableTray.Location as LocationCurve)?.Curve as Line;
        private XYZ FirstPointOfLine => Line.Origin;
        private XYZ Direction => Line.Direction;
        public HashSet<ICableTray> IncidentTrays { get; } = new HashSet<ICableTray>();

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
            var ids = owners.Select(o => o.Id.IntegerValue).ToArray();
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

                var flag = result - min < 0.00001;
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

        public XYZ GetNearestPoint(XYZ otherPoint)
        {
            var ps = GetPoints().Take(2).ToArray();
            var p1 = ps[0];
            var p2 = ps[1];
            var scalar1 = (otherPoint - p1).DotProduct(p2 - p1);
            var scalar2 = (otherPoint - p2).DotProduct(p1 - p2);
            if (scalar1 < 0) return p1;
            if (scalar2 < 0) return p2;
            var lengthH = (p1 - otherPoint).CrossProduct(p2 - otherPoint).GetLength() / (p1 - p2).GetLength();
            var l = Math.Sqrt(Math.Pow((p1 - otherPoint).GetLength(), 2) - lengthH * lengthH);
            var ab = p2 - p1;
            var abl = ab.GetLength();
            var result = p1 + ab / abl * l;

            //check
            var q1 = result.DotProduct(ab) < 0.00001;
            var q2 = (result - p1).GetLength() + (result - p2).GetLength() - ab.GetLength() < 0.001;
            //
            return result;
        }

        public XYZ GetNearestPoint(MyCableTray otherMcTf)
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

        public double GetLengthToPoint(XYZ point)
        {
            return Line.Distance(point);
        }

        public override bool Equals(object obj)
        {
            if (obj is ICableTray ct)
            {
                return GetHashCode() == ct.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _cableTray.Id.IntegerValue;
        }
    }
}
