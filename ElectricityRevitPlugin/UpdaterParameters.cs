namespace ElectricityRevitPlugin;

using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

public sealed class UpdaterParameters<T> where T : Element
{
    private readonly List<Func<T, string>> _actions = new();
    private readonly BuiltInCategory _category;
    private readonly Document _doc;

    public UpdaterParameters(Document document, BuiltInCategory category)
    {
        _doc = document;
        _category = category;
    }

    private IEnumerable<T> GetElements()
    {
        var fec = new FilteredElementCollector(_doc)
            .OfCategory(_category)
            .OfClass(typeof(T))
            .WhereElementIsNotElementType()
            .OfType<T>();
        return fec;
    }

    public void AddAction(IUpdaterParameters<T> updater)
    {
        _actions.Add(updater.UpdateParameters);
    }

    public void Execute()
    {
        var els = GetElements();
        foreach (var el in els)
        {
            foreach (var a in _actions)
                a.Invoke(el);
        }
    }
}
