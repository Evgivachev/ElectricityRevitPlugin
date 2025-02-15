namespace BimRenRes.QuickSelection.ParameterFunctions;

using Autodesk.Revit.DB;

public abstract class ParameterFunction
{
    public abstract string Name { get; }
    public abstract FilterRule GetFilterRule(StorageType storageType, ElementId parameterId, string value);
    public double Epsilon => 0.00001;
    public bool CaseSensitive => false;
}