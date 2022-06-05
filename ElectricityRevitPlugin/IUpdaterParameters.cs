namespace ElectricityRevitPlugin
{
    using Autodesk.Revit.DB;

    public interface IUpdaterParameters<in T> where T : Element
    {
        string UpdateParameters(T el);
    }
}
