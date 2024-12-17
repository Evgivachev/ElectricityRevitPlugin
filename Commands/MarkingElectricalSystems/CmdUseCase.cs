namespace MarkingElectricalSystems;

using Application;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommonUtils;

/// <inheritdoc />
public class CmdUseCase(IMarkElectricalSystemsService electricalSystemsService) : ICmdUseCase
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        electricalSystemsService.DoSomething();
        return Result.Succeeded;
    }
}
