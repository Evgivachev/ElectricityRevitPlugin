namespace MarkingElectricalSystems.Models;

using System.Collections.Generic;
using Autodesk.Revit.DB;

internal class ElementEqualityComparer<T> : IEqualityComparer<T> where T : Element
{
    public bool Equals(T x, T y)
    {
        return x.Id.IntegerValue == y.Id.IntegerValue;
    }

    public int GetHashCode(T obj)
    {
        return obj.UniqueId.GetHashCode();
    }
}
