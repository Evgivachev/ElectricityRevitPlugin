namespace ElectricityRevitPlugin.Updaters;

using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

public class UpdateLocker : IDisposable
{
    private static UpdateLocker _updateLocker;
    private bool _isLocked;

    private UpdateLocker()
    {
        ElementsIds = new List<Tuple<ElementId, ChangeType>>();
    }

    public ICollection<Tuple<ElementId, ChangeType>> ElementsIds { get; set; }


    public void Dispose()
    {
        _isLocked = false;
        ElementsIds = null!;
    }

    public static UpdateLocker GetUpdateLocker()
    {
        if (_updateLocker is null)
            _updateLocker = new UpdateLocker();
        return _updateLocker;
    }

    public UpdateLocker Lock()
    {
        _isLocked = true;
        return this;
    }

    public bool IsLocked(Document doc)
    {
        if (_isLocked)
            return true;
        if (null == ElementsIds)
            return false;
        return ElementsIds.Any(t =>
        {
            var id = t.Item1;
            var el = doc.GetElement(id);
            return el != null && el.IsValidObject;
        });
    }
}
