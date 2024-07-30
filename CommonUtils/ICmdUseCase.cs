using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using JetBrains.Annotations;

namespace CommonUtils;

[PublicAPI]
public interface ICmdUseCase
{
    Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements);
}