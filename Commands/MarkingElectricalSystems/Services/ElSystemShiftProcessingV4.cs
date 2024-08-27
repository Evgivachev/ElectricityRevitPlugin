namespace MarkingElectricalSystems.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Abstractions;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using MoreLinq;

public class ElSystemShiftProcessingV4 : ElSystemShiftProcessing
{
    public override Result Process(IEnumerable<ElectricalSystem> electricalSystems, double shift)
    {
        var uiApp = ShiftElectricalCircuits.ExternalCommandData.Application;
        var app = uiApp.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        foreach (var electricalSystem in electricalSystems)
        {
            var baseElement = electricalSystem.BaseEquipment;
            var devices = electricalSystem.Elements
                .OfType<FamilyInstance>()
                .ToArray();
            var circuitPath = new PathOfCircuits();
            circuitPath.Add(baseElement);
            var unconnectedDevices = new List<FamilyInstance>(devices);
            var previous = baseElement;
            var errorsStringBuilder = new StringBuilder();
            while (unconnectedDevices.Count > 0)
            {
                var previousLevelId = previous.LevelId;
                var devicesOnThisLevel = unconnectedDevices
                    .Where(d => d.LevelId.IntegerValue == previousLevelId.IntegerValue || d.LevelId.IntegerValue == -1);
                FamilyInstance nearest = null;
                if (devicesOnThisLevel.Any())
                    nearest = devicesOnThisLevel.MinBy(x => GetDistance(x, previous)).FirstOrDefault();
                else
                    nearest = unconnectedDevices.MinBy(x => GetDistance(x, previous)).FirstOrDefault();

                var nearestDeviceLevelId = nearest.LevelId;
                if (nearestDeviceLevelId.IntegerValue == -1)
                    nearestDeviceLevelId =
                        nearest.get_Parameter(BuiltInParameter.INSTANCE_SCHEDULE_ONLY_LEVEL_PARAM)
                            .AsElementId();

                var nearestDeviceLevel = (Level)doc.GetElement(nearestDeviceLevelId);
                if (nearestDeviceLevel is null)
                {
                    var message = $"У элемента {nearest.Id.IntegerValue}-{nearest.Name} следует определить уровень";
                    // throw new Exception($"У элемента {nearest.Id.IntegerValue}-{nearest.Name} следует определить уровень");
                    errorsStringBuilder.AppendLine(message);
                    break;
                }

                circuitPath.Add(nearest, nearestDeviceLevel.Elevation + shift);
                unconnectedDevices.Remove(nearest);
                previous = nearest;
            }

            if (electricalSystem.IsCircuitPathValid(circuitPath.Points))
                using (var tr = new Transaction(doc))
                {
                    tr.Start("Установка траектории цепи");
                    electricalSystem.SetCircuitPath(circuitPath.Points);
                    tr.Commit();
                }
            else
            {
                var count = circuitPath.Points.Count;
                while (count > 0 && !electricalSystem.IsCircuitPathValid(circuitPath.Points.Take(count).ToList()))
                {
                    count--;
                }

                MessageBox.Show($@"{count}Ошибка в установлении траектории цепи {electricalSystem?.Id?.IntegerValue}");
            }
        }

        return Result.Succeeded;
    }

    private double GetDistance(FamilyInstance f1, FamilyInstance f2)
    {
        var p1 = (f1.Location as LocationPoint)?.Point;
        var p2 = (f2.Location as LocationPoint)?.Point;
        if (p2 is null || p1 is null)
            throw new NullReferenceException("Не удалось определить позицию элемента");
        return p2.Subtract(p1).GetLength();
    }
}
