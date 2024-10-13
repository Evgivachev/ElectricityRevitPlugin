namespace GroupByGost.Domain;

using System;
using System.Collections.Generic;
using System.Linq;

public class Shield
{
    private readonly ElectricalCircuit[] _electricalCircuits;
    
    public Shield(string prefix, string separator, ElectricalCircuit[] electricalCircuits)
    {
        Prefix = prefix;
        Separator = separator;
        _electricalCircuits = electricalCircuits;
    }


    private string Prefix { get; }
    private string Separator { get; }

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
