using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using ArgumentException = System.ArgumentException;

namespace BimRenRes.QuickSelection;

public class ValueClass : IComparable
{
    public ValueClass(StorageType storageType, DisplayUnitType displayUnitType, string stringValue)
    {
            _storageType = storageType;
            DisplayUnitType = displayUnitType;
            if (storageType is StorageType.String)
                Value = stringValue;
            else if (storageType is StorageType.Double)
            {
                var isParsing = double.TryParse(stringValue, out var dValue);
                Value = isParsing ? dValue : 0D;
            }
            else if (storageType is StorageType.ElementId)
            {
                var isParsing = int.TryParse(stringValue, out var iValue);
                Value = isParsing ? new ElementId(iValue) : ElementId.InvalidElementId;
            }
            else if (storageType is StorageType.Integer)
            {
                var isParsing = int.TryParse(stringValue, out var iValue);
                Value = isParsing ? iValue : 0;
            }
        }

    public ValueClass(Parameter parameter)
    {
            _storageType = parameter.StorageType;
            var def = parameter.Definition;
            if (_storageType is StorageType.String)
                DisplayUnitType = DisplayUnitType.DUT_GENERAL;
            else
                DisplayUnitType = parameter.DisplayUnitType;
            _value = parameter.GetValueDynamic();
        }



    public bool HasValue { get; set; }

    //public void UpdateValue(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    //{
    //    var propertyName = propertyChangedEventArgs.PropertyName;
    //    if (propertyName != FilterCreatorViewModel.SelectedValuePropertyName)
    //        return;
    //    var viewModel = sender as FilterCreatorViewModel;
    //    if (viewModel is null)
    //        throw new NullReferenceException();
    //    var parameterGetter = viewModel.SelectedParameterGetter;
    //    var parameterGetterStorageType = parameterGetter.StorageType;
    //    StorageType = parameterGetterStorageType;
    //    var parameterDisplayUnitType = viewModel.GetDisplayUnitType(parameterGetter);
    //    DisplayUnitType = parameterDisplayUnitType;

    //}
    public static implicit operator ValueClass(string param)
    {
            return new ValueClass(StorageType.String, DisplayUnitType.DUT_GENERAL, param);
        }

    private StorageType _storageType;

    public StorageType StorageType
    {
        get => _storageType;
        set
        {

                if (_storageType == value)
                    return;

                if (_storageType == StorageType.String)
                    switch (value)
                    {
                        case StorageType.String:
                            break;
                        case StorageType.Double:
                            {
                                var tryParse = double.TryParse(Value.ToString(), out var dValue);
                                if (tryParse)
                                    Value = dValue;
                                else
                                    ClearValue(value);
                                break;
                            }
                        case (StorageType.ElementId):
                            {
                                var tryParse = int.TryParse(Value.ToString(), out var iValue);
                                if (tryParse)
                                    Value = new ElementId(iValue);
                                else
                                    ClearValue(value);
                                break;
                            }
                        case StorageType.Integer:
                            {
                                var tryParse = int.TryParse(Value.ToString(), out var iValue);
                                if (tryParse)
                                    Value = new ElementId(iValue);
                                else
                                    ClearValue(value);
                                break;
                            }
                    }
                else if (_storageType == StorageType.Integer)
                {
                    if (value is StorageType.String)
                    {
                        Value = Value.ToString();
                    }
                    else if (value is StorageType.Double)
                    {
                        Value = (double)(int)Value;
                    }
                    else if (value is StorageType.ElementId)
                    {
                        Value = new ElementId((int)Value);
                    }
                }
                else if (_storageType == StorageType.Double)
                {
                    if (value is StorageType.String)
                    {
                        Value = Value.ToString();
                    }
                    else if (value is StorageType.Integer)
                    {
                        Value = (int)(double)Value;
                    }
                    else if (value is StorageType.ElementId)
                    {
                        Value = new ElementId((int)(double)Value);
                    }
                }
                else if (_storageType == StorageType.ElementId)
                {
                    if (value is StorageType.String)
                    {
                        Value = Value.ToString();
                    }
                    else if (value is StorageType.Integer)
                    {
                        Value = ((ElementId)Value).IntegerValue;
                    }
                    else if (value is StorageType.Double)
                    {
                        var elId = (ElementId)Value;
                        Value = (double)elId.IntegerValue;
                    }
                }
                _storageType = value;
            }
    }

    private void ClearValue(StorageType newStorageType)
    {
            HasValue = false;
            switch (newStorageType)
            {
                case StorageType.String:
                    Value = default(string);
                    break;
                case StorageType.Double:
                    Value = default(double);
                    break;
                default:
                    Value = default(int);
                    break;
            }
        }

    public DisplayUnitType DisplayUnitType { get; set; }

    public string DisplayUnitTypeLabel
    {
        get
        {
                var formatOption = new FormatOptions(DisplayUnitType);
                var unitSymbol = formatOption.UnitSymbol;
                var unitSymbolType = formatOption.GetValidUnitSymbols().FirstOrDefault(x => x != UnitSymbolType.UST_NONE);
                return (unitSymbolType is UnitSymbolType.UST_NONE) ? "" : LabelUtils.GetLabelFor(DisplayUnitType);
            }
    }

    private object _value;
    private double _tolerance = 1e-6;

    public object Value
    {
        get
        {
                return _value;
            }
        set
        {
                _value = value;
            }
    }

    public string ValueString
    {
        get
        {
                return Value.ToString();
            }
        set
        {
                switch (StorageType)
                {
                    case StorageType.String:
                        Value = value;
                        break;
                    case StorageType.Double:
                        {
                            var isParsingOk = double.TryParse(value, out var dValue);
                            if (isParsingOk)
                                Value = dValue;
                            else
                            {
                                ClearValue(StorageType);
                            }
                            break;
                        }
                    case StorageType.Integer:
                        {
                            var isParsingOk = int.TryParse(value, out var iValue);
                            if (isParsingOk)
                                Value = iValue;
                            else
                            {
                                ClearValue(StorageType);
                            }
                            break;
                        }
                    case StorageType.ElementId:
                        {
                            var isParsingOk = int.TryParse(value, out var iValue);
                            if (isParsingOk)
                                Value = new ElementId(iValue);
                            else
                            {
                                ClearValue(StorageType);
                            }
                            break;
                        }

                }
            }
    }

    public dynamic GetValue(bool isInternalUnits)
    {
            if (StorageType is StorageType.String)
                return Value;
            if (isInternalUnits)
                return Value;
            return UnitUtils.ConvertFromInternalUnits((double)Value, DisplayUnitType);
        }

    public override string ToString()
    {
            var result = GetValue(false);
            if (result is string)
                return result;
            else
            {
                return result.ToString();
            }
        }
    public override bool Equals(object obj)
    {
            if (!(obj is ValueClass v2))
                return false;
            if (StorageType is StorageType.String)
            {
                return (string)Value == (string)v2.Value;
            }
            else if (StorageType is StorageType.Double)
                return Math.Abs(AsDouble() - v2.AsDouble()) < _tolerance;
            else if (StorageType is StorageType.ElementId)
                return AsElementId() == v2.AsElementId();
            else if (StorageType is StorageType.Integer)
                return AsInteger() == v2.AsInteger();
            return Value == v2.GetValue(true);
        }

    public override int GetHashCode()
    {
            unchecked
            {
                var hashCode = (int)_storageType;
                hashCode = (hashCode * 397) ^ (_value != null ? _value.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ HasValue.GetHashCode();
                return hashCode;
            }
        }

    public double AsDouble()
    {
            if (StorageType != StorageType.Double)
                return 0;
            return (double)Value;

        }
    public int AsInteger()
    {
            if (StorageType != StorageType.Integer)
                return 0;
            return (int)Value;

        }

    public ElementId AsElementId()
    {
            if (StorageType != StorageType.ElementId)
                return null;
            return (ElementId)Value;
        }
    public int CompareTo(object obj)
    {
            var v2 = obj as ValueClass;
            if (v2 is null)
                throw new ArgumentException();
            if (StorageType != v2.StorageType)
            {
                throw new ArgumentException();
            }
            if (StorageType is StorageType.String)
                return String.Compare(ValueString, v2.ValueString, StringComparison.Ordinal);
            else if (StorageType is StorageType.ElementId)
            {
                var elId = (ElementId)Value;
                var elId2 = (ElementId)v2.Value;
                return elId.IntegerValue.CompareTo(elId2.IntegerValue);
            }
            else if (StorageType is StorageType.Double)
            {
                return ((double)Value).CompareTo((double)v2.Value);

            }
            else if (StorageType is StorageType.Integer)
            {
                return ((int)Value).CompareTo((int)v2.Value);
            }
            else
            {
                return Value.ToString().CompareTo(v2._value);
            }
        }

    public string GetValueInternalUnits()
    {
            return ValueString;
            double dValue;
            if (StorageType is StorageType.ElementId)
            {
                var elId = (ElementId)Value;
                dValue = (double)elId.IntegerValue;
            }
            else
                dValue = (double)Value;

            var result = UnitUtils.ConvertToInternalUnits(dValue, DisplayUnitType);
            return result.ToString(CultureInfo.InvariantCulture);
        }
}