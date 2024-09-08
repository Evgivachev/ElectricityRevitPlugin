namespace GroupByGost.Domain;

using System;

public class ElectricalCircuit : IComparable<ElectricalCircuit>
{
    public int Id { get; init; }
    public bool IsReserve { get; init; }
    public bool IsDisableChange { get; init; }
    public int StartSlot { get; init; }
    public bool IsControlCircuit { get; init; }
    public string GroupByGost { get; private set; } = string.Empty;
    public string QFNumber { get; private set; } = string.Empty;

    public int CompareTo(ElectricalCircuit? other)
    {
        if (ReferenceEquals(this, other))
            return 0;
        if (ReferenceEquals(null, other))
            return 1;
        var isReserveComparison = IsReserve.CompareTo(other.IsReserve);
        if (isReserveComparison != 0)
            return isReserveComparison;
        var isControlCircuitComparison = IsControlCircuit.CompareTo(other.IsControlCircuit);
        if (isControlCircuitComparison != 0)
            return isControlCircuitComparison;
        return StartSlot.CompareTo(other.StartSlot);
    }

    public void SetName(string prefix, string separator, int number)
    {
        GroupByGost = $"{prefix}{separator}{number}";
        QFNumber = $"QF{number}";
    }
}
