namespace CableJournalCmd;

using Application;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommonUtils;

public class UseCase(ICableJournalService cableJournalService) : ICmdUseCase
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        cableJournalService.CreateCableJournal();
        return Result.Succeeded;
    }
}
