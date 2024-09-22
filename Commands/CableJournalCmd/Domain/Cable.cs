namespace CableJournalCmd.Domain;

public record Cable(int id, string Chapter, string PanelName, string qf, string groupGost, bool isReserved, bool isControl)
{
    public bool ShouldBeAddedToJournal() => !isReserved && !isControl;
    
    public string InJournalName { get; private set;} = string.Empty;

    public string InTubeJournalName { get; private set; } = string.Empty;

    public void ClearName()
    {
        InTubeJournalName = string.Empty;
        InJournalName = string.Empty;
    }

    public void SetNames(string gostName, int number)
    {
        var nameInCableSchedule = $"{gostName}-{number}";
        InJournalName = nameInCableSchedule;
        InTubeJournalName = $"T-{number}";
    }
}
