namespace CommonUtils.Extensions;

using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

public static class FilteredElementCollectorExtensions
{
    public static IEnumerable<T> OfClass<T>(this FilteredElementCollector collector)
    {
        return collector.OfClass(typeof(T)).Cast<T>();
    }
}
