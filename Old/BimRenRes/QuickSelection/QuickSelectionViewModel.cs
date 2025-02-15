namespace BimRenRes.QuickSelection;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

public enum SelectionMode
{
    [Description("Весь документ")]
    AllDocument,
    [Description("Текущее выделение")]
    CurrentSelection,
    [Description("Текущий вид")]
    CurrentView
}
public class QuickSelectionViewModel : INotifyPropertyChanged, IDisposable, IFilterHolder
{
    public static QuickSelectionViewModel ThisQuickSelectionViewModel { get; private set; }
    private static QuickSelectionViewModel _lastLaunchedQuickSelectionViewModel;

    private UIDocument _uiDoc;
    private readonly UIApplication _uiApp;
    private readonly Document _doc;
    private QuickSelectionWindow _window;

    public QuickSelectionViewModel(UIApplication uiApp)
    {
            _uiApp = uiApp;
            _uiDoc = _uiApp.ActiveUIDocument;
            _doc = _uiDoc.Document;
            AllCategories = _doc
                .Settings
                .Categories
                .OfType<Category>()
                .OrderBy(x => x.Name)
                .ToArray();
            UpdateCommandAddFilter();
            //Работает, но выдаёт не все категории (электрических цепей нет)
            //AllCategories = ParameterFilterUtilities
            //    .GetAllFilterableCategories()
            //    .Select(x=>Category.GetCategory(_doc,x))
            //    .OrderBy(x => x?.Name)
            //    .ToArray();

            ThisQuickSelectionViewModel = this;
            CopySettingsFromLastLaunch();
        }

    private void UpdateCommandAddFilter()
    {
            AddFilterCommand = new AddFilterCommand(this, new FilterCreatorViewModel(_uiDoc, () =>
            {
                return new[] { SelectedCategory.Id };
            }));

        }

    private void CopySettingsFromLastLaunch()
    {

            if (_lastLaunchedQuickSelectionViewModel is null)
                return;
            try
            {
                this.SelectedCategory = AllCategories.First(x => x.Name == _lastLaunchedQuickSelectionViewModel.SelectedCategory.Name);
                this.SelectionMode = _lastLaunchedQuickSelectionViewModel.SelectionMode;
                this.IncludeInNewSet = _lastLaunchedQuickSelectionViewModel.IncludeInNewSet;
                this.AddToCurrentSet = _lastLaunchedQuickSelectionViewModel.AddToCurrentSet;
                Filters = _lastLaunchedQuickSelectionViewModel.Filters;
            }
            catch
            {
                // ignored
            }
        }

    public Category[] AllCategories { get; }
    private Category _selectedCategory;

    public Category SelectedCategory
    {
        get => _selectedCategory;
        set
        {
                if (_selectedCategory != null && _selectedCategory.Id == value.Id)
                    return;
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                UpdateCommandAddFilter();
                Filters = new ObservableCollection<MyFilter>();
            }
    }

    private ObservableCollection<MyFilter> _filters = new ObservableCollection<MyFilter>();
    public ObservableCollection<MyFilter> Filters
    {
        get => _filters;
        private set
        {
                _filters = value;
                OnPropertyChanged(nameof(Filters));
            }
    }

    private MyFilter _selectedFilter;
    public MyFilter SelectedFilter
    {
        get => _selectedFilter;
        set
        {
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                OnPropertyChanged(nameof(IsSelectedAnyFilter));
            }
    }

    public bool IsSelectedAnyFilter
    {
        get => SelectedFilter != null;
    }

    public AddFilterCommand AddFilterCommand { get; private set; }
    public List<SelectionMode> AvailableSelectionModes
    {
        get
        {
                var selection = _uiDoc.Selection.GetElementIds();
                if (!selection.Any() || AddToCurrentSet)
                {
                    if (SelectionMode == SelectionMode.CurrentSelection)
                        SelectionMode = SelectionMode.AllDocument;
                    OnPropertyChanged(nameof(SelectionMode));
                    return new List<SelectionMode>
                    {
                        SelectionMode.AllDocument,
                        SelectionMode.CurrentView
                    };
                }
                return new List<SelectionMode>
                {
                        SelectionMode.AllDocument,
                        SelectionMode.CurrentView,
                        SelectionMode.CurrentSelection
                };
            }
    }

    public SelectionMode SelectionMode { get; set; }

    public MyFilter AddFilter(MyFilter filter)
    {
            Filters.Add(filter);
            OnPropertyChanged(nameof(Filters));
            return filter;
        }
    public void RemoveFilter(MyFilter filter)
    {
            Filters.Remove(filter);
            OnPropertyChanged(nameof(Filters));
        }

    private bool _includeInNewSet = true;
    public bool IncludeInNewSet
    {
        get => _includeInNewSet;
        set
        {
                _includeInNewSet = value;
                OnPropertyChanged(nameof(IncludeInNewSet));
            }
    }
    public bool ExcludeFromNewSet => !_includeInNewSet;

    private bool _addToCurrentSet = false;
    public bool AddToCurrentSet
    {
        get => _addToCurrentSet;
        set
        {
                _addToCurrentSet = value;
                OnPropertyChanged(nameof(AvailableSelectionModes));
            }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    public void EditFilter()
    {
            //  var selectedFilter = Filters[FilterListBoxSelectedIndex] as MyElementParameterFilter;
            var selectedFilter = SelectedFilter as MyElementParameterFilter;
            var index = Filters.IndexOf(SelectedFilter);
            var filterCreatorViewModel = new FilterCreatorViewModel(selectedFilter, _uiDoc, new[] { SelectedCategory });
            var editedFilter = filterCreatorViewModel.ShowDialogAndCreateFilter();
            if (editedFilter != null)
            {
                Filters[index] = editedFilter;
                SelectedFilter = editedFilter;
            }

            OnPropertyChanged(nameof(Filters));
        }

    public void DoWork()
    {
            _window = new QuickSelectionWindow(this);
            var dialogResult = _window.ShowDialog();
            if (dialogResult != true)
                return;
            var filters = Filters;
            LogicalAndFilter commonFilter = null;
            if (Filters.Any())
                commonFilter = new LogicalAndFilter(filters
                     .Select(x => x.ConvertToElementFilter())
                                         .ToList());
            FilteredElementCollector filteredElementCollector = null;
            switch (SelectionMode)
            {
                case SelectionMode.AllDocument:
                    filteredElementCollector = new FilteredElementCollector(_doc);
                    break;
                case SelectionMode.CurrentSelection:
                    filteredElementCollector = new FilteredElementCollector(_doc, _uiDoc.Selection.GetElementIds());
                    break;
                case SelectionMode.CurrentView:
                    filteredElementCollector = new FilteredElementCollector(_doc, _doc.ActiveView.Id);
                    break;
            }
            Debug.Assert(filteredElementCollector != null, nameof(filteredElementCollector) + " != null");

            filteredElementCollector = filteredElementCollector
                .OfCategory((BuiltInCategory)SelectedCategory.Id.IntegerValue);
            if (commonFilter != null)
                filteredElementCollector = filteredElementCollector
           .WherePasses(commonFilter);
            var selection = _uiDoc.Selection;
            var commonSelection = filteredElementCollector.ToElementIds();
            if (AddToCurrentSet)
            {
                commonSelection = commonSelection.Concat(selection.GetElementIds()).ToArray();
            }
            selection.SetElementIds(commonSelection);

            //Сохранить фильтры в статическое поле для след запуска
            _lastLaunchedQuickSelectionViewModel = this;
        }

    public void Dispose()
    {
            _window?.Dispose();
        }
}