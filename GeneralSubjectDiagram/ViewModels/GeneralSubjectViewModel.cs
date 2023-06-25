namespace GeneralSubjectDiagram.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Selection;
    using GalaSoft.MvvmLight;
    using PikTools.Ui.Abstractions;
    using PikTools.Ui.Commands;
    using Services.ParametersUpdaters;

    /// <inheritdoc />
    public class GeneralSubjectViewModel : ViewModelBase
    {
        private readonly Document _doc;
        private readonly UIDocument _uiDoc;
        private bool _isHideExistingElementsCheckBox;
        private FamilySymbol? _selectedFamilySymbol;
        private FamilySymbol[] _availableFamilySymbols = Array.Empty<FamilySymbol>();
        private CollectionOfCheckableItems _treeCollectionOfCheckableItems = new();

        /// <inheritdoc />
        public GeneralSubjectViewModel(UIApplication uiApplication)
        {
            _uiDoc = uiApplication.ActiveUIDocument;
            _doc = _uiDoc.Document;
        }

        /// <summary>
        /// Создание семейств
        /// </summary>
        public ICommand ExecuteCommand => new RelayAsyncCommand<IClosable>(Execute);

        /// <summary>
        /// Инициализация
        /// </summary>
        public ICommand InitializeCommand => new RelayAsyncCommand<IClosable>(Initialize);


        /// <summary>
        /// Выбранное семейство для вставки
        /// </summary>
        public FamilySymbol? SelectedFamilySymbol
        {
            get => _selectedFamilySymbol;
            set
            {
                _selectedFamilySymbol = value;
                RaisePropertyChanged();
                UpdateTreeCollectionOfCheckableItems();
            }
        }

        /// <summary>
        /// Доступные семейства для выбора
        /// </summary>
        public FamilySymbol[] AvailableFamilySymbols
        {
            get => _availableFamilySymbols;
            private set => Set(ref _availableFamilySymbols, value);
        }

        /// <summary>
        /// Скрыть существующие элементы
        /// </summary>
        public bool IsHideExistingElementsCheckBox
        {
            get => _isHideExistingElementsCheckBox;
            set => Set(ref _isHideExistingElementsCheckBox, value);
        }

        /// <summary>
        /// Коллекция элементов
        /// </summary>
        public CollectionOfCheckableItems TreeCollectionOfCheckableItems
        {
            get => _treeCollectionOfCheckableItems;
            private set => Set(ref _treeCollectionOfCheckableItems, value);
        }

        private Task Initialize(IClosable arg)
        {
            var elementParameterFilter =
                new ElementParameterFilter(new SharedParameterApplicableRule("ReflectionClassName"));
            AvailableFamilySymbols = new FilteredElementCollector(_doc)
                .OfClass(typeof(FamilySymbol))
                .WhereElementIsElementType()
                //.WherePasses(elementParameterFilter)
                .OfType<FamilySymbol>()
                .Where(x =>
                {
                    var updaterClassName = x.get_Parameter(ParameterUpdater.ReflectionClassNameGuid)?.AsString();
                    return !string.IsNullOrEmpty(updaterClassName);
                })
                .ToArray();
            return Task.CompletedTask;
        }

        private Task Execute(IClosable closable)
        {
            var selectedItems = TreeCollectionOfCheckableItems
                .SelectMany(x => x.GetSelectedCheckableItems())
                .Where(x => x.Item is Element)
                .Select(x => (Element)x.Item);
            var insertedElement = new List<FamilyInstance>();
            var currentAssembly = Assembly.GetCallingAssembly();
            var fs = SelectedFamilySymbol;
            var updaterClassName = fs.get_Parameter(ParameterUpdater.ReflectionClassNameGuid).AsString();
            using var tr = new Transaction(_doc, "Вставка элементов схемы ВРУ");
            tr.Start();
            foreach (var element in selectedItems)
            {
                var point = PickPoint();
                var parameterUpdater = (ParameterUpdater)currentAssembly.CreateInstance(
                    updaterClassName,
                    false,
                    BindingFlags.CreateInstance,
                    null,
                    new object[] { element }, CultureInfo.InvariantCulture, null)!;
                var instance = parameterUpdater.InsertInstance(fs, point);
                insertedElement.Add(instance);
                parameterUpdater.SetParameters(instance);
                _doc.Regenerate();
            }

            tr.Commit();
            closable.Close();
            return Task.CompletedTask;
        }

        private void UpdateTreeCollectionOfCheckableItems()
        {
            if (SelectedFamilySymbol is null)
            {
                TreeCollectionOfCheckableItems = null;
                return;
            }

            var currentAssembly = Assembly.GetCallingAssembly();
            var updaterClassName = SelectedFamilySymbol.get_Parameter(ParameterUpdater.ReflectionClassNameGuid).AsString();
            var parameterUpdater = (ParameterUpdater)currentAssembly.CreateInstance(updaterClassName, false,
                BindingFlags.CreateInstance, null, null, CultureInfo.InvariantCulture, null)!;
            var validateElements = parameterUpdater?.GetValidateElements(_doc);
            TreeCollectionOfCheckableItems = validateElements;
        }

        private XYZ PickPoint()
        {
            var snapTypes = ObjectSnapTypes.Endpoints | ObjectSnapTypes.Intersections;
            var point = _uiDoc.Selection.PickPoint(snapTypes, "Select an end point or intersection");
            return point;
        }
    }
}
