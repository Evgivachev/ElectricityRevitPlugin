namespace GroupByGost;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Bll;
using CommonUtils;

public class UseCase : ICmdUseCase
{
    private readonly IGroupByGostService _groupByGostService;
    public UseCase(IGroupByGostService groupByGostService)
    {
        _groupByGostService = groupByGostService;
    }

    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        _groupByGostService.Execute();
        return Result.Succeeded;
    }
}
