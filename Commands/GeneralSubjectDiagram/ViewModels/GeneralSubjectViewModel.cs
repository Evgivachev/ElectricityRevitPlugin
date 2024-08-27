namespace GeneralSubjectDiagram.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using CommonUtils.Extensions;
using CommonUtils.Helpers;
using PikTools.Ui.Abstractions;
using PikTools.Ui.Commands;
using PikTools.Ui.ViewModels;
using Services.ParametersUpdaters;

/// <inheritdoc />
public class GeneralSubjectViewModel : MainViewModelBase
{
    private readonly IUIDispatcher _uiDispatcher;
    private readonly RevitTask _revitTask;
    private readonly Document _doc;
    private readonly UIDocument _uiDoc;
    private bool _isHideExistingElementsCheckBox;
    private ParameterUpdater? _selectedUpdater;
    private ParameterUpdater[] _availableFamilies = Array.Empty<ParameterUpdater>();
    private ObservableCollection<CheckableItem> _treeCollectionOfCheckableItems = new();
    private Dictionary<string, Family> _familiesDict = new();

    /// <inheritdoc />
    public GeneralSubjectViewModel(
        UIApplication uiApplication,
        IEnumerable<ParameterUpdater> updaters,
        IUIDispatcher uiDispatcher,
        RevitTask revitTask)
        : base("Схема ВРУ")
    {
        _uiDispatcher = uiDispatcher;
        _revitTask = revitTask;
        _uiDoc = uiApplication.ActiveUIDocument;
        _doc = _uiDoc.Document;
        InitializeCommand = new RelayAsyncCommand<IClosable>(Initialize);
        AvailableFamilies = updaters.ToArray();
    }

    /// <summary>
    /// Создание семейств
    /// </summary>
    public ICommand ExecuteCommand => new RelayAsyncCommand<IHidable>(Execute);

    /// <summary>
    /// Выбранное семейство для вставки
    /// </summary>
    public ParameterUpdater? SelectedUpdater
    {
        get => _selectedUpdater;
        set
        {
            _selectedUpdater = value;
            RaisePropertyChanged();
            Task.Run(UpdateTreeCollectionOfCheckableItems);
        }
    }

    /// <summary>
    /// Доступные семейства для выбора
    /// </summary>
    public ParameterUpdater[] AvailableFamilies
    {
        get => _availableFamilies;
        private set => Set(ref _availableFamilies, value);
    }

    /// <summary>
    /// Скрыть существующие элементы
    /// </summary>
    public bool IsHideExistingElementsCheckBox
    {
        get => _isHideExistingElementsCheckBox;
        set
        {
            if (value == _isHideExistingElementsCheckBox)
                return;
            Task.Run(UpdateTreeCollectionOfCheckableItems);
            Set(ref _isHideExistingElementsCheckBox, value);
        }
    }

    /// <summary>
    /// Коллекция элементов
    /// </summary>
    public ObservableCollection<CheckableItem> TreeCollectionOfCheckableItems
    {
        get => _treeCollectionOfCheckableItems;
        private set => Set(ref _treeCollectionOfCheckableItems, value);
    }

    private async Task Initialize(IClosable arg)
    {
        var familyNames = AvailableFamilies.Select(x => x.FamilyNameToInsert).ToHashSet();
        var families = await _revitTask
            .Run(uiApp =>
            {
                var q = new FilteredElementCollector(uiApp.ActiveUIDocument.Document)
                    .OfClass<Family>()
                    .Where(f => familyNames.Contains(f.Name))
                    .ToArray();
                return q;
            });
        _familiesDict = families.ToDictionary(f => f.Name);
        SelectedUpdater = AvailableFamilies.FirstOrDefault();
    }

    private async Task Execute(IHidable closable)
    {
        try
        {
            await _uiDispatcher.InvokeAsync(closable.Hide);
            if (SelectedUpdater is null)
                return;
            var selectedItems = TreeCollectionOfCheckableItems
                .SelectMany(x => x.GetSelectedCheckableItems())
                .Where(x => x.Item is Element and not null)
                .Select(x => (Element)x.Item!)
                .ToArray();
            await _revitTask.Run(application =>
            {
                using var tr = new Transaction(_doc, "Вставка элементов схемы ВРУ");
                var familySymbol =
                    _doc.GetElement(_familiesDict[SelectedUpdater.FamilyNameToInsert].GetFamilySymbolIds().First()) as FamilySymbol;
                tr.Start();
                foreach (var baseElement in selectedItems)
                {
                    var point = PickPoint();
                    if (point is null)
                        break;
                    var instance = SelectedUpdater.InsertInstance(familySymbol, point);
                    SelectedUpdater.SetParameters(instance, baseElement);
                    _doc.Regenerate();
                }

                tr.Commit();
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            await _uiDispatcher.InvokeAsync(closable.Show);
        }
    }

    private async Task UpdateTreeCollectionOfCheckableItems()
    {
        if (SelectedUpdater is null)
        {
            TreeCollectionOfCheckableItems = new ObservableCollection<CheckableItem>();
            return;
        }

        var elements = await _revitTask.Run(application => SelectedUpdater.GetValidateElements(application.ActiveUIDocument.Document));
        if (IsHideExistingElementsCheckBox)
        {
            var elementsOnCurrentView = await _revitTask.Run(application =>
            {
                var activeView = _uiDoc.Document.ActiveView as ViewDrafting;
                if (activeView is null)
                    return new HashSet<int>();
                return new FilteredElementCollector(_doc, activeView.Id).ToElements()
                    .Select(e => int.TryParse(e.get_Parameter(SharedParametersFile.ID_Svyazannogo_Elementa)?.AsString(), out var id)
                        ? id
                        : -1)
                    .Where(id => id > 0)
                    .ToHashSet();
            });
            var arr = new List<CheckableItem>();
            foreach (var checkableItem in elements)
            {
                var children = checkableItem.Children
                    .Where(c => c.Item is Element)
                    .Where(c => !elementsOnCurrentView.Contains((c.Item as Element)!.Id.IntegerValue))
                    .ToArray();
                if (children.Length <= 0)
                    continue;
                var parent = new CheckableItem();
                parent.Item = checkableItem.Item;
                parent.Name = checkableItem.Name;
                foreach (var child in children)
                    parent.Children.Add(new CheckableItem(parent) { Name = child.Name, Item = child.Item, IsChecked = child.IsChecked });

                arr.Add(parent);
            }

            elements = new ObservableCollection<CheckableItem>(arr);
        }

        await _uiDispatcher.InvokeAsync(() => TreeCollectionOfCheckableItems = elements);
    }

    private XYZ? PickPoint()
    {
        try
        {
            var snapTypes = ObjectSnapTypes.Endpoints | ObjectSnapTypes.Intersections;
            var point = _uiDoc.Selection.PickPoint(snapTypes, "Select an end point or intersection");
            return point;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
