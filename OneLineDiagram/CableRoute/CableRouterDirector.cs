namespace Diagrams.CableRoute
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using MoreLinq.Extensions;

    public class CableRouterDirector
    {
        public ElectricalSystemsFinder ElectricalSystemsFinder { get; set; }

        public Result DoWork(ExternalCommandData commandData)
        {
            var doc = commandData.Application.ActiveUIDocument.Document;
            var result = Result.Succeeded;
            var ess = ElectricalSystemsFinder.GetElectricalSystems(commandData, null);
            foreach (var es in ess)
            {
                Debug.WriteLine(es.Id.IntegerValue);
                try
                {
                    var nearestTray = SystemOfTrays.GetNearestCableTray(es);
                    var systemOfTrays = new SystemOfTrays(nearestTray);
                    var fixtures = es.Elements
                        .OfType<FamilyInstance>()
                        .ToList();
                    var shield = es.BaseEquipment;
                    var track = new List<ICableTray>();
                    var start = new MyCableTrayFitting(shield);
                    start.IncidentTrays.Add(systemOfTrays[nearestTray.Id.IntegerValue]);
                    var leftFixtures = fixtures
                        .Select(f => new MyCableTrayFitting(f))
                        .ToList();
                    track.Add(start);
                    while (leftFixtures.Count > 0)
                    {
                        //var tracks = leftFixtures
                        //    .Select(f => Dijkstra(systemOfTrays, start, f)).ToArray();
                        var shortTrack = leftFixtures
                            .Select(f => Dijkstra(systemOfTrays, start, f))
                            .MinBy(tr => tr.Price)
                            .First();
                        track.AddRange(shortTrack.Track.Skip(1));
                        var lastFixtures = track.Last() as MyCableTrayFitting;
                        start = lastFixtures;
                        //добавить ближайший лоток
                        start.IncidentTrays.Add(systemOfTrays.GetNearestCableTray(start));
                        leftFixtures.Remove(lastFixtures);
                    }

                    var ids = track.Select(l => l.Id).ToArray();
                    Debug.Print("track:\n");
                    foreach (var id in ids)
                    {
                        Debug.Print(id.ToString());
                    }

                    var elSystemTr = new ElSystemTransformer(track, es);
                    var points = elSystemTr.GetPoints();
                    Debug.Print("Points:\n");
                    foreach (var p in points)
                    {
                        Debug.Print(p.ToString());
                    }

                    var array = es.GetCircuitPath().Take(5).ToArray();
                    using (var tr = new Transaction(doc))
                    {
                        tr.Start("jfjf");
                        es.SetCircuitPath(points);
                        //es.SetCircuitPath(points.Take(4).ToArray());
                        //es.SetCircuitPath(array);
                        tr.Commit();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }

            return result;
        }

        public static (List<ICableTray> Track, double Price) Dijkstra(SystemOfTrays systemOfTrays, ICableTray start, ICableTray end)
        {
            if (end.Id == 19969580)
            {
            }

            var priceGetter = new PriceGetter();
            //не посесещенные вершины
            var notVisited = new List<ICableTray>(systemOfTrays.GetCableTrays());
            notVisited.Add(start);
            notVisited.Add(end);
            // путь отслеживает предыдущий и вес вершины
            var track = new Dictionary<ICableTray, (ICableTray Previous, double Price)>();
            track[start] = (null, 0);
            track[end] = (start, priceGetter.GetPrice(start, end) * 10);
            var price0 = track[end].Price;
            while (true)
            {
                ICableTray toOpen = null;
                var bestPrice = double.PositiveInfinity;
                foreach (var e in notVisited)
                {
                    if (track.ContainsKey(e) && track[e].Price < bestPrice)
                    {
                        bestPrice = track[e].Price;
                        toOpen = e;
                    }
                }

                if (toOpen == null) return (null, double.PositiveInfinity);
                if (Equals(toOpen, end)) break;
                var currentPrice = track[toOpen].Price + priceGetter.GetPrice(toOpen, end) * 10;
                if (!track.ContainsKey(end) || track[end].Price > currentPrice)
                {
                    track[end] = (toOpen, currentPrice);
                }

                foreach (var nextNode in toOpen.IncidentTrays)
                {
                    currentPrice = track[toOpen].Price + priceGetter.GetPrice(toOpen, nextNode);
                    if (!track.ContainsKey(nextNode) || track[nextNode].Price > currentPrice)
                    {
                        track[nextNode] = (toOpen, currentPrice);
                    }
                }

                notVisited.Remove(toOpen);
            }

            var result = new List<ICableTray>();
            var price = track[end].Price;
            while (end != null)
            {
                result.Add(end);
                end = track[end].Previous;
            }

            result.Reverse();
            return (result, price);
        }
    }
}
