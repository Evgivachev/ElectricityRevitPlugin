namespace CommonUpdateCmd.Application;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommonUtils;

public class UseCase(
    ITransactionsService transactionsService,
    IUpdateElSystemsService updateElSystemsService,
    IEnumerable<IExternalCommand> externalCommands) : ICmdUseCase
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        using var _ = transactionsService.StartTransaction("Обновление параметров");
        updateElSystemsService.Execute();
        foreach (var externalCommand in externalCommands)
        {
            externalCommand.Execute();
        }
        transactionsService.Commit();
        return Result.Succeeded;
    }
}
