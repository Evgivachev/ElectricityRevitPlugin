namespace BimRenRes.QuickSelection;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ParameterFunctions;

public sealed class FilterCreatorViewModel : INotifyPropertyChanged, IFilterCreator
{
    internal static readonly string SelectedValuePropertyName = nameof(SelectedValueObject);

    private readonly Func<ElementId[]> _getCategoriesIds;
    private ElementId[] _categoriesIds;
    private ElementId[] CategoriesIds
    {
        get
        {
                _categoriesIds = _categoriesIds ?? _getCategoriesIds.Invoke();
                return _categoriesIds;
            }
        set
        {
                _categoriesIds = value;
                OnPropertyChanged(nameof(CategoriesIds));
                OnPropertyChanged(nameof(AvailableParameters));
                OnPropertyChanged(nameof(AvailableParameterValues));
            }
    }
    private IEnumerable<Element> AvailableElements => GetAvailableElements();

    private readonly Document _doc;

    internal DisplayUnitType GetDisplayUnitType(ParameterGetter parameterGetter)
    {
            InternalDefinition definition = null;
            if (parameterGetter is SharedAndGlobalParameterGetter sharedAndGlobalParameterGetter)
                definition = sharedAndGlobalParameterGetter.GetDefinition();
            else
            {
                var sampleElement = GetAvailableElements()
                    .FirstOrDefault();
                if (sampleElement is null)
                    return Autodesk.Revit.DB.DisplayUnitType.DUT_GENERAL;
                var parameter = parameterGetter.GetParameter(sampleElement);
                if (parameter is null)
                {
                    var sampleElementTypeId = sampleElement.GetTypeId();
                    var sampleElementType = _doc.GetElement(sampleElementTypeId);
                    parameter = parameterGetter.GetParameter(sampleElementType);
                    if (parameter is null)
                        throw new NullReferenceException();
                }

                definition = (InternalDefinition)parameter.Definition;
            }
            var unitType = definition.UnitType;
            var units = _doc.GetUnits();
            var formatOption = units.GetFormatOptions(unitType);
            var displayUnits = formatOption.DisplayUnits;
            return displayUnits;
        }
       
    public FilterCreatorViewModel(UIDocument uiDoc, Func<ElementId[]> categories)
    {
            _doc = uiDoc.Document;
            _getCategoriesIds = categories;
            //PropertyChanged += _selectedValueObject.UpdateValue;
        }

    internal FilterCreatorViewModel(MyElementParameterFilter filter, UIDocument uiDoc, Category[] categories)
    {
            _doc = uiDoc.Document;
            _categoriesIds = categories.Select(x => x.Id).ToArray();
            SelectedParameterGetter = AvailableParameters
                .First(x => x.Id == filter.GetParameterElementId);
            SelectedParameterFunction = Functions
                .First(x => x.Name == filter.GetFuncName);
            SelectedValueObject = filter.ValueObject;

            //PropertyChanged += _selectedValueObject.UpdateValue;
        }

    public MyFilter ShowDialogAndCreateFilter()
    {
            var window = new FilterCreatorWindow(this);
            var flag = window.ShowDialog();
            if (flag != true)
                return null;
            return CreateFilter();
        }

    private MyFilter CreateFilter()
    {
            var func = SelectedParameterFunction;
            return new MyElementParameterFilter(SelectedParameterGetter, func, SelectedValueObject);
        }

    private List<ParameterGetter> _availableParameters;
    public List<ParameterGetter> AvailableParameters
    {
        get
        {
                if (_availableParameters is null)
                    AvailableParameters = ParameterFilterUtilities
                        .GetFilterableParametersInCommon(_doc, CategoriesIds)
                        .Select(x => ParameterGetter.Create(_doc, x))
                        .OrderBy(x => x.ParameterName)
                        .ToList();
                return _availableParameters;
            }
        set
        {
                _availableParameters = value;
                OnPropertyChanged(nameof(AvailableParameters));
            }
    }

    private ParameterGetter _selectedParameterGetter;
    public ParameterGetter SelectedParameterGetter
    {
        get => _selectedParameterGetter;
        set
        {
                _selectedParameterGetter = value;
                OnPropertyChanged(nameof(SelectedParameterGetter));
                OnPropertyChanged(nameof(AvailableParameterValues));
                OnPropertyChanged(nameof(SelectedValueObject));

            }
    }

    public List<ParameterFunction> Functions { get; } = new List<ParameterFunction>
    {
        new Equals(),
        new NotEquals(),
        new Greater(),
        new GreaterOrEqual(),
        new Less(),
        new LessOrEqual(),
        new Contains(),
        new NotContains(),
        new BeginsWith(),
        new NotBeginsWith(),
        new EndsWith(),
        new NotEndsWith()
    };

    private ValueClass _selectedValueObject = new ValueClass(StorageType.String, DisplayUnitType.DUT_GENERAL, string.Empty);
    public ValueClass SelectedValueObject
    {
        get
        {
                if(SelectedParameterGetter.StorageType == _selectedValueObject.StorageType)
                    return _selectedValueObject;
                else
                {
                    _selectedValueObject = new ValueClass(SelectedParameterGetter.StorageType,GetDisplayUnitType(SelectedParameterGetter),"");
                    return _selectedValueObject;
                }
            }
        set
        {
                _selectedValueObject = value;
                //PropertyChanged += value.UpdateValue;
            }
    }

    private ParameterFunction _selectedParameterFunction;
    public ParameterFunction SelectedParameterFunction
    {
        get => _selectedParameterFunction;
        set
        {
                _selectedParameterFunction = value;
                OnPropertyChanged(nameof(SelectedParameterFunction));
            }
    }


    private IEnumerable<Element> GetAvailableElements()
    {
            foreach (var categoriesId in CategoriesIds)
            {
                var fec = new FilteredElementCollector(_doc)
                    .OfCategoryId(categoriesId)
                    .WhereElementIsNotElementType();
                foreach (var element in fec.ToElements())
                {
                    yield return element;

                }
            }
        }

    public IList<ValueClass> AvailableParameterValues
    {
        get
        {
                var selectedParameter = SelectedParameterGetter;
                if (selectedParameter is null)
                    return new List<ValueClass>();
                if (AvailableElements is null)
                    throw new NullReferenceException();

                var availableValues = AvailableElements.Select(
                        el => selectedParameter.GetParameter(el))
                    .Where(x => x != null)
                    .Select(parameter =>
                    {
                        return new ValueClass(parameter);
                        //var valueString = (string)UnitUtils.ConvertFromInternalUnits(value, SelectedValueObject.DisplayUnitType).ToString();
                        //return valueString;
                    })
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();
                return availableValues;
            }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

}