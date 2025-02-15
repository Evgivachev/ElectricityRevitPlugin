namespace Print.View.ViewModel;

using System.IO;
using System.Windows.Input;
using Application.Application;
using Application.Domain;
using ElectricityRevitPlugin.UI;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;

public class BaseViewModel : MainViewModelBase
{
    private string _folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    private readonly IUIDispatcher _uiDispatcher;
    private readonly PrintViewModel _printViewModel;
    private readonly ExportViewModel _exportViewModel;
    private readonly ISheetsRepository _sheetsRepository;
    private List<CheckableItem> _sheetsTreeCollectionOfCheckableItems = new();

    public BaseViewModel(
        IUIDispatcher uiDispatcher,
        PrintViewModel printViewModel,
        ExportViewModel exportViewModel,
        ISheetsRepository sheetsRepository) : base("Печать")
    {
        _uiDispatcher = uiDispatcher;
        _printViewModel = printViewModel;
        _exportViewModel = exportViewModel;
        _sheetsRepository = sheetsRepository;
        PrintViewModel = printViewModel;
        ExportViewModel = exportViewModel;
        InitializeCommand = new RelayAsyncCommand<ICloseable>(Initialize);
        SheetsTreeCollectionOfCheckableItems = [];
        BrowseFolderCommand = new RelayCommand(BrowseFolder);
        OkButtonCommand = new RelayAsyncCommand<IHideable>(Execute);
        CheckstateChangedCommand = new RelayCommand<CheckableItem>(CheckstateChanged);
    }

    public RelayAsyncCommand<IHideable> OkButtonCommand { get; set; }

    public string Folder
    {
        get => _folder;
        set => Set(ref _folder, value);
    }

    public PrintViewModel PrintViewModel { get; }
    public ExportViewModel ExportViewModel { get; }

    public List<CheckableItem> SheetsTreeCollectionOfCheckableItems
    {
        get => _sheetsTreeCollectionOfCheckableItems;
        private set => Set(ref _sheetsTreeCollectionOfCheckableItems, value);
    }

    public ICommand BrowseFolderCommand { get; }

    public ICommand CheckstateChangedCommand { get; set; }

    private void CheckstateChanged(CheckableItem checkableItem)
    {
        foreach (var item in SheetsTreeCollectionOfCheckableItems)
            UpdateIsCheckstate(item);
    }

    public void UpdateIsCheckstate(CheckableItem checkableItem)
    {
        foreach (var itemChild in checkableItem.Children)
            UpdateIsCheckstate(itemChild);
        if (checkableItem.Children.Count == 0)
            return;

        checkableItem.IsChecked = checkableItem.Children.All(x => x.IsChecked == true) ? true
            : checkableItem.Children.All(x => x.IsChecked == false) ? false : null;
    }

    private void BrowseFolder()
    {
        var openFileDialog = new OpenFileDialog();
        openFileDialog.InitialDirectory = Folder;
        openFileDialog.ValidateNames = false;
        openFileDialog.CheckFileExists = false;
        openFileDialog.FileName = "Выберите папку";
        openFileDialog.CheckPathExists = true;
        if (openFileDialog.ShowDialog() == true)
            Folder = Path.GetDirectoryName(openFileDialog.FileName) ?? string.Empty;
    }

    private async Task Initialize(ICloseable arg)
    {
        await _printViewModel.Initialize();
        await _exportViewModel.Initialize();
        var sheets = (await _sheetsRepository.GetSheets()).ToArray();
        var items = sheets
            .ToDictionary(x => x.Id, x => new CheckableItem() { Name = x.Name, Item = x });
        foreach (var sheet in sheets)
        {
            if (sheet.ParentId is not null)
                items[sheet.ParentId.Value].Children.Add(items[sheet.Id]);
        }

        SheetsTreeCollectionOfCheckableItems = sheets.Where(x => x.ParentId is null).Select(x => items[x.Id]).ToList();
    }

    private async Task Execute(IHideable arg)
    {
        arg.Hide();
        var sheetIds = GetSelectedSheets();
        try
        {
            if (ExportViewModel.IsExportSelected)
                await ExportViewModel.Export(sheetIds, Folder);
            else
                await PrintViewModel.Print(sheetIds, Folder);
        }
        finally
        {
            await _uiDispatcher.InvokeAsync(arg.Close);
        }
    }

    private int[] GetSelectedSheets()
    {
        var selected = SheetsTreeCollectionOfCheckableItems
            .SelectMany(x => x.GetSelectedCheckableItems()
                .Select(ci => ci?.Item)
                .Where(obj => obj != null))
            .OfType<Sheet>()
            .Select(x => x.Id)
            .Where(x => x > 0)
            .ToArray();
        return selected;
    }
}
