namespace CommonUtils.Extensions;

using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

/// <summary>
/// Методы расширения для <see cref="FilteredElementCollector"/>
/// </summary>
public static class FilteredElementCollectorExtensions
{
    /// <summary>
    /// Выполняет фильтрацию по классу.
    /// </summary>
    /// <param name="collector"></param>
    /// <typeparam name="T"></typeparam>
    public static IEnumerable<T> OfClass<T>(this FilteredElementCollector collector)
    {
        return collector.OfClass(typeof(T)).Cast<T>();
    }
}
