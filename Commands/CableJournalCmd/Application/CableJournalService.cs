namespace CableJournalCmd.Application;

using Domain;

public class CableJournalService(ICableRepository cableRepository) : ICableJournalService
{
    
    public void CreateCableJournal()
    {
        var cables = cableRepository.GetAll();
        var cableJournal = new CableJournal();
        foreach (var cable in cables)
            cableJournal.Add(cable);
        cableJournal.SortCables();
        using var tr = cableRepository.StartTransaction("Обновление кабельного журнала");
        cableRepository.UpdateCables(cableJournal);
        tr.Commit();
    }
}