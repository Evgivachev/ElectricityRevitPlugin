using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace BimRenRes.QuickSelection;

using ParameterFunctions;

class MyElementParameterFilter :MyFilter
{
    private ParameterGetter _parameterGetter;
    private ParameterFunction _parameterFunction;
    public  ValueClass ValueObject { get; set; }
    public MyElementParameterFilter(ParameterGetter parameterGetter, ParameterFunction parameterFunction, string valueObject)
    {
            _parameterGetter = parameterGetter;
            _parameterFunction = parameterFunction;
            ValueObject = valueObject;
        }
    public MyElementParameterFilter(ParameterGetter parameterGetter, ParameterFunction parameterFunction, ValueClass valueObject)
    {
            _parameterGetter = parameterGetter;
            _parameterFunction = parameterFunction;
            ValueObject = valueObject;
        }

    public ElementId GetParameterElementId => _parameterGetter.Id;

    public string GetFuncName => _parameterFunction.Name;
    public string GetValueString => string.Copy(ValueObject.ToString());

    //Возможно неиспользуемый
    public override bool PassesFilter(Element element)
    {       
            var doc = element.Document;
            var elementFilter = ConvertToElementFilter();
            return elementFilter.PassesFilter(doc, element.Id);
        }

    public override string Name => $"{_parameterGetter.ParameterName} {_parameterFunction.Name.ToLower()} \"{ValueObject}\"";
    public override ElementFilter ConvertToElementFilter()
    {
            var parameterId = _parameterGetter.Id;
            var storageType = _parameterGetter.StorageType;
            var filterRule = _parameterFunction.GetFilterRule(storageType, parameterId, ValueObject.GetValueInternalUnits());
            var parameterFilter = new ElementParameterFilter(filterRule);
            return parameterFilter;
        }

        
}