namespace CommonUtils.Comparer;

using System;
using System.Collections.Generic;
using Autodesk.Revit.DB.Electrical;

/// <inheritdoc />
public class ElectricalSystemsComparer : IEqualityComparer<ElectricalSystem>
{
    /// <inheritdoc />
    public bool Equals(ElectricalSystem x, ElectricalSystem y)
    {
        return x.IsValidObject == y.IsValidObject
               && x.Id.IntegerValue == y.Id.IntegerValue
               && x.Document.Title.Equals(y.Document.Title, StringComparison.InvariantCulture)
               && x.PanelName.Equals(y.PanelName, StringComparison.InvariantCulture)
               && x.LoadName.Equals(y.PanelName, StringComparison.InvariantCulture);
    }

    /// <inheritdoc />
    public int GetHashCode(ElectricalSystem obj)
    {
        return obj.IsValidObject ? obj.Id.IntegerValue.GetHashCode() : -1;
    }
}
