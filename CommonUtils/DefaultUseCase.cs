using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace CommonUtils;

public abstract class DefaultUseCase : ICmdUseCase
{
    /// <inheritdoc />
    public abstract Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements);
}
