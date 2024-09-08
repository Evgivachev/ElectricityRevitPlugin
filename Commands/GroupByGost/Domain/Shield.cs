namespace GroupByGost.Domain;

using System;
using System.Collections.Generic;
using System.Linq;

public class Shield
{
    private Shield()
    {
    }


    private string Prefix { get; init; }
    private string Separator { get; init; }

    public static Shield Create(string prefix, string separator, IEnumerable<ElectricalCircuit> electricalCircuits)
    {
        return new Shield()
        {
            Prefix = prefix,
            Separator = separator,
            _electricalCircuits = electricalCircuits.ToArray()
        };
    }

    private ElectricalCircuit[] _electricalCircuits;

    public void SortCircuits()
    {
        Array.Sort(_electricalCircuits);
        var number = 1;
        foreach (var circuit in _electricalCircuits)
        {
            circuit.SetName(Prefix, Separator, number++);
        }
    }
    public IEnumerable<ElectricalCircuit> GetCircuits()
    {
        return _electricalCircuits.ToArray();
    }
}
