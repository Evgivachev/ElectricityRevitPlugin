namespace MarkingElectricalSystems.Models;

using System.Collections.Generic;
using Autodesk.Revit.DB;

/// <inheritdoc />
public class ElementComparer<T> : IEqualityComparer<T>
    where T : Element
{
    /// <inheritdoc />
    public bool Equals(T x, T y)
    {
        if (x == null && y == null) return true;
        if (x == null || y == null) return false;
        return x.Id.IntegerValue == y.Id.IntegerValue;
    }

    /// <inheritdoc />
    public int GetHashCode(T obj)
    {
        if (obj is null)
            return 0;
        return obj.Id.IntegerValue;
    }
}
