namespace Diagrams.CableRoute
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Mechanical;

    public class MyCableTrayFitting : ICableTray
    {
        private readonly FamilyInstance _familyInstance;

        public readonly int Id0;

        public MyCableTrayFitting(FamilyInstance fi)
        {
            var category = (BuiltInCategory)fi.Category.Id.IntegerValue;
            if (!(category is BuiltInCategory.OST_CableTrayFitting
                  || category is BuiltInCategory.OST_ElectricalEquipment
                  || category is BuiltInCategory.OST_LightingFixtures
                  || category is BuiltInCategory.OST_LightingDevices))
            {
                //throw new System.ArgumentException();
            }

            _familyInstance = fi;
            Id0 = Id;
        }

        public IList<XYZ> GetPoints()
        {
            return
                new List<XYZ>()
                {
                    (_familyInstance.Location as LocationPoint)?.Point
                };
            //return _familyInstance.MEPModel
            //    .ConnectorManager.Connectors
            //    .OfType<Connector>()
            //    .Where(c => c.ConnectorType == ConnectorType.End)
            //    .Select(c => c.Origin)
            //    .ToList();
        }

        public HashSet<ICableTray> IncidentTrays { get; } = new HashSet<ICableTray>();

        public IEnumerable<Element> GetIncidentElements()
        {
            var connectors = _familyInstance
                .MEPModel
                .ConnectorManager
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

        //Todo  исправить!!
        public double DistanceTo(ICableTray ct)
        {
            if (ct is MyCableTrayFitting mctf)
            {
                if (!(mctf.GetBuiltInCategory() is BuiltInCategory.OST_CableTray))
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
                    //Проверка
                    //foreach (var myP in GetPoints())
                    //{
                    //    foreach (var point in ct.GetPoints())
                    //    {
                    //        var d = myP.DistanceTo(point);
                    //        if (d < min)
                    //            min = d;
                    //    }
                    //}
                    //var flag = result - min < 0.00001;
                    return result;
                }
            }

            return ct.DistanceTo(this);
        }

        public int Id => _familyInstance.Id.IntegerValue;

        public IEnumerable<XYZ> GetPointsToICableTray(ICableTray otherCableTray)
        {
            if (otherCableTray is MyCableTrayFitting myCableTrayFitting)
            {
                yield return myCableTrayFitting.GetPoints().First();
            }
            else
            {
                var myCableTray = otherCableTray as MyCableTray;
                var nearestPoint = GetNearestPoint(myCableTray);
                yield return nearestPoint;
            }
        }

        public Space GetSpace()
        {
            return _familyInstance.Space;
        }

        public BuiltInCategory GetBuiltInCategory()
        {
            return (BuiltInCategory)_familyInstance.Category.Id.IntegerValue;
        }

        public XYZ GetNearestPoint(MyCableTray mCt)
        {
            var otherPoint = GetPoints().First();
            var ps = mCt.GetPoints().Take(2).ToArray();
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

        public override bool Equals(object obj)
        {
            if (obj is MyCableTrayFitting mcTf)
            {
                return mcTf.GetHashCode() == GetHashCode();
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _familyInstance.Id.IntegerValue;
        }
    }
}
