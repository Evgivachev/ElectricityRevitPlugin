namespace MarkingElectricalSystems.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;

public abstract class ElSystemShiftProcessing
{
    protected double tolerance = 1.0e-09;

    public static void MakeAllDevicesInElectricalSystems(IEnumerable<ElectricalSystem> electricalSystems)
    {
        var uiApp = ShiftElectricalCircuits.ExternalCommandData.Application;
        var app = uiApp.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        using (var tr = new Transaction(doc))
        {
            tr.Start("Все устройства для электрической цепи");
            foreach (var es in electricalSystems)
                es.CircuitPathMode = ElectricalCircuitPathMode.AllDevices;

            tr.Commit();
        }
    }

    public static void MakeAllDevicesInElectricalSystems(ElectricalSystem electricalSystems, bool openTransaction = false)
    {
        var uiApp = ShiftElectricalCircuits.ExternalCommandData.Application;
        var app = uiApp.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        if (openTransaction)
            using (var tr = new Transaction(doc))
            {
                tr.Start("Все устройства для электрической цепи");
                electricalSystems.CircuitPathMode = ElectricalCircuitPathMode.AllDevices;
                tr.Commit();
            }
        else
        {
            electricalSystems.CircuitPathMode = ElectricalCircuitPathMode.AllDevices;
            doc.Regenerate();
        }
    }

    public virtual Result Process(IEnumerable<ElectricalSystem> electricalSystems, double shift)
    {
        var uiApp = ShiftElectricalCircuits.ExternalCommandData.Application;
        var app = uiApp.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        MakeAllDevicesInElectricalSystems(electricalSystems);
        using (var tr = new Transaction(doc))
        {
            tr.Start("Задание смещения электрических цепей");
            foreach (var s in electricalSystems)
            {
                //var devicesPoints = s.Elements.OfType<FamilyInstance>()
                //    .Select(f => f.Location as LocationPoint)
                //    .Where(x => x != null)
                //    .Select(x => x.Point)
                //    .ToArray();
                var coordZ = 3100;
                //level.Elevation + shift / 1000 / 0.3048;
                var points = s.GetCircuitPath();
                var myPoints = new List<XYZ>();
                if (points.Count < 2)
                    continue;
                var previos = points.First();
                myPoints.Add(previos);
                for (var i = 1; i < points.Count; ++i)
                {
                    var current = points[i];
                    if (ZEquals(previos, current))
                    {
                        var z = previos.Z;
                        var dlength = current.Subtract(previos).GetLength();
                        if (dlength > 1)
                            if (Math.Abs(z - coordZ) > tolerance)
                            {
                                myPoints.Add(new XYZ(previos.X, previos.Y, coordZ));
                                myPoints.Add(new XYZ(current.X, current.Y, coordZ));
                            }
                    }

                    // if(devicesPoints.Contains(current))
                    myPoints.Add(current);
                    previos = current;
                }

                if (s.IsCircuitPathValid(myPoints))
                    s.SetCircuitPath(myPoints);
            }

            tr.Commit();
        }

        return Result.Succeeded;
    }

    protected bool ZEquals(XYZ p1, XYZ p2)
    {
        return Math.Abs(p1.Z - p2.Z) < tolerance;
    }
}
