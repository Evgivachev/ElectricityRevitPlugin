namespace CommonUpdateCmd.Infrastructure.UpdateElectricalSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

public sealed class UpdaterParameters<T> where T : Element
{
    private readonly Document _document;
    private readonly List<Func<T, string>> _actions = new();
    private readonly ICollection<BuiltInCategory> _categories;

    public UpdaterParameters(Document document, params BuiltInCategory[] categories)
    {
        _document = document;
        _categories = categories;
    }


    private IEnumerable<T> GetElements()
    {
        if (_categories.Count == 1)
        {
            var fec = new FilteredElementCollector(_document)
                .OfCategory(_categories.Single())
                .OfClass(typeof(T))
                .WhereElementIsNotElementType()
                .OfType<T>();
            return fec;
        }
        else
        {
            var mf = new ElementMulticategoryFilter(_categories);
            var fec = new FilteredElementCollector(_document)
                .OfClass(typeof(T))
                .WhereElementIsNotElementType()
                .WherePasses(mf)
                .OfType<T>();
            return fec;
        }
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
