namespace ShortCircuits.Services;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommonUtils;

/// <inheritdoc />
public class CommonShortCircuitsCmdUseCase : ICmdUseCase
{
    private readonly OneShortCircuitsService _oneShortCircuitsService;
    private readonly ShortCircuitsService _shortCircuitsService;

    public CommonShortCircuitsCmdUseCase(OneShortCircuitsService oneShortCircuitsService, ShortCircuitsService shortCircuitsService)
    {
        _oneShortCircuitsService = oneShortCircuitsService;
        _shortCircuitsService = shortCircuitsService;
    }
    /// <inheritdoc />
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var result = _oneShortCircuitsService.Execute(commandData, ref message, elements);
        if (result != Result.Succeeded)
            return result;
        return _shortCircuitsService.Execute(commandData, ref message, elements);
    }
}
