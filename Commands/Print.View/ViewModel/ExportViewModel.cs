namespace Print.View.ViewModel;

using System.Windows;
using System.Windows.Input;
using Application.Application;
using Application.Domain;
using GalaSoft.MvvmLight;

public class ExportViewModel(ISettingsService settingsService, IPrintService printService) : ViewModelBase
{
    private IList<string> _availableDwgExportOptions;
    private string? _selectedAvailableDwgExportOption;
    private ICommand _exportCommand;

    public IList<string> AvailableDwgExportOptions
    {
        get => _availableDwgExportOptions;
        private set => Set(ref _availableDwgExportOptions, value);
    }

    public string? SelectedAvailableDwgExportOption
    {
        get => _selectedAvailableDwgExportOption;
        set => Set(ref _selectedAvailableDwgExportOption, value);
    }

    public string FilePrefix { get; set; } = "";

    public bool IsExportSelected { get; set; } = true;
    public string NameToolTip =>
        "Either the name of a single file or a prefix for a set of files. If empty, automatic naming will be used. If a null reference ( Nothing in Visual Basic) , throw ArgumentException.";

    public async Task Initialize()
    {
        var settings = await settingsService.GetSettings();
        AvailableDwgExportOptions = settings.DwgSettings.ToArray();
        SelectedAvailableDwgExportOption = AvailableDwgExportOptions.FirstOrDefault();
    }

    public async Task Export(IReadOnlyCollection<int> sheets, string folder)
    {
        if (string.IsNullOrEmpty(SelectedAvailableDwgExportOption))
        {
            MessageBox.Show("Выберите настройки экспорта!");
            return;
        }

        var exportSettings = new ExportSettingsOut()
        {
            DwgExportOption = SelectedAvailableDwgExportOption ?? string.Empty,
            ViewSheetIds = sheets,
            FilePrefix = " FilePrefix,",
            Folder = folder,
        };
        await printService.Export(exportSettings);
    }
}
