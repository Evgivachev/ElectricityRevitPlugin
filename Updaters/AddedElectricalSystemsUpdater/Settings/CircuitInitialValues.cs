namespace AddedElectricalSystemsUpdater.Settings;

using System.Collections.Generic;
using JetBrains.Annotations;

[UsedImplicitly]
public class CircuitInitialValues
{
    public Dictionary<string, int> FromKeyScheduleValues { get; set; } = new();
    public Dictionary<string, object> FromSharedParameters { get; set; } = new();
    public Dictionary<string, object> FromBuiltInParameters { get; set; } = new();
}
