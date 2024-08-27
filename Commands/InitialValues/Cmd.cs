namespace InitialValues;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

/// <inheritdoc cref="Autodesk.Revit.UI.IExternalCommand" />
public class Cmd : IExternalCommand, IExternalCommandAvailability
{
    /// <inheritdoc />
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
    {
        throw new System.NotImplementedException();
    }
}
