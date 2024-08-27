/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using MoreLinq;

namespace MarkingElectricalSystems
{
    public class ElSystemShiftProcessingV3 : ElSystemShiftProcessing
    {
        public override Result Process(IEnumerable<ElectricalSystem> electricalSystems,  double shift)
        {
            var uiApp = ShiftElectricalCircuits.ExternalCommandData.Application;
            var app = uiApp.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var doc = uiDoc.Document;
            using var tr = new Transaction(doc);
            tr.Start("Задание смещения электрических цепей");
            foreach (var s in electricalSystems)
            {
                var devices = s.Elements.OfType<FamilyInstance>()
                    .ToList();
                //var devicesPoints = s.Elements.OfType<FamilyInstance>()
                //    .Select(f => f.Location as LocationPoint)
                //    .Where(x => x != null)
                //    .Select(x => x.Point)
                //    .ToArray();

                var coordZ =  shift / 1000 / 0.3048;
                var myPoints = new PathOfCircuits();
                var firstPoint = (s.BaseEquipment?
                        .Location as LocationPoint)?
                    .Point;
                if (firstPoint is null)
                    return Result.Cancelled;
                myPoints.Add(firstPoint,coordZ);
                var previosPoint = firstPoint;
                while (devices.Count > 0)
                {
                    var nearestDevice = devices.MinBy(d =>
                    {
                        var pd = (d.Location as LocationPoint)?.Point;
                        if (pd is null)
                            return double.MaxValue;
                        var l = pd.Subtract(previosPoint).GetLength();
                        return l;
                    }).FirstOrDefault();
                    var nextPoint = (nearestDevice.Location as LocationPoint).Point;
                    myPoints.Add(nextPoint,coordZ);
                    previosPoint = nextPoint;
                    devices.Remove(nearestDevice);
                }
                if (s.IsCircuitPathValid(myPoints.Points))
                {
                    s.SetCircuitPath(myPoints.Points);
                }
                else
                {
                    MessageBox.Show("Error");

                }



            }
            tr.Commit();
            return Result.Succeeded;
        }
    }
}
*/



