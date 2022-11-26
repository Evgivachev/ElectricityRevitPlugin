namespace MarkingElectricalSystems.Models;

using System.Collections.Generic;
using Autodesk.Revit.DB;

public class ElementComparer : IEqualityComparer<Element>
{
    public bool Equals(Element x, Element y)
    {
        if (x == null && y == null) return true;
        if (x == null || y == null) return false;
        return x.Id.IntegerValue == y.Id.IntegerValue;
    }

    public int GetHashCode(Element obj)
    {
        if (obj is null)
            return 0;
        return obj.Id.IntegerValue;
    }
}
