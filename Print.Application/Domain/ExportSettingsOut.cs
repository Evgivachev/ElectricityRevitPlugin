namespace Print.Application.Domain;

public class ExportSettingsOut
{
    public string DwgExportOption { get; init; }

    public IReadOnlyCollection<int> ViewSheetIds { get; init; }
    
    public string Folder { get; init; }

    public string FilePrefix { get; init; }
}
