namespace Diagrams.CableRoute
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;
    using MoreLinq.Extensions;

    public class SystemOfTrays
    {
        //private Document _document;
        private readonly Dictionary<int, ICableTray> _cableTrays = new Dictionary<int, ICableTray>();

        public SystemOfTrays(CableTray ct)
        {
            //_document = ct.Document;
            var queue = new Queue<ICableTray>();
            var myCableTray = new MyCableTray(ct);
            queue.Enqueue(myCableTray);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var cid = current.GetHashCode();
                _cableTrays[current.GetHashCode()] = current;
                foreach (var incident in current.GetIncidentElements())
                {
                    ICableTray incT = null;
                    if (incident is CableTray ict)
                    {
                        incT = new MyCableTray(ict);
                    }
                    else if (incident is FamilyInstance fi)
                    {
                        incT = new MyCableTrayFitting(fi);
                    }
                    else
                    {
                        throw new ArgumentException();
                    }

                    var id = incT.GetHashCode();
                    current.IncidentTrays.Add(incT);
                    if (_cableTrays.ContainsKey(id)) continue;
                    _cableTrays[id] = incT;
                    queue.Enqueue(incT);
                }

                if (queue.Count > 100) break;
            }
        }

        public ICableTray this[int id]
        {
            get
            {
                if (_cableTrays.ContainsKey(id))
                    return _cableTrays[id];
                return null;
            }
        }

        public static CableTray GetNearestCableTray(ElectricalSystem es)
        {
            var doc = es.Document;
            var allTrays = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_CableTray)
                .OfClass(typeof(CableTray))
                .OfType<CableTray>();
            var minTray = allTrays.MinBy(tr =>
            {
                var line = (tr.Location as LocationCurve)?.Curve as Line;
                var point = es.BaseEquipment.MEPModel.ConnectorManager.Connectors.OfType<Connector>()
                    .First(c => c.ConnectorType is ConnectorType.End)?.Origin;
                return line?.Distance(point);
            }).First();
            if (minTray is null)
            {
            }

            return minTray;
        }

        public ICableTray GetNearestCableTray(ICableTray es)
        {
            ICableTray nearestCableTray = null;
            var minD = double.MaxValue;
            var otherP = es.GetPoints();
            foreach (var tray in _cableTrays.Values)
            {
                var myP = tray.GetPoints();
                foreach (var mp in myP)
                {
                    foreach (var oP in otherP)
                    {
                        var d = oP.DistanceTo(mp);
                        if (d < minD)
                        {
                            minD = d;
                            nearestCableTray = tray;
                        }
                    }
                }
            }

            return nearestCableTray;
        }

        public IEnumerable<ICableTray> GetCableTrays()
        {
            return _cableTrays.Values;
        }
    }
}
