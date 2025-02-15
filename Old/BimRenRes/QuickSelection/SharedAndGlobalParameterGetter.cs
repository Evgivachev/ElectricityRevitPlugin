using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace BimRenRes.QuickSelection;

class SharedAndGlobalParameterGetter : ParameterGetter
{
    private readonly ParameterElement _parameterElement;
    private readonly Document _doc;
    public SharedAndGlobalParameterGetter(Document doc, ElementId elementId)
    {
            _parameterElement = (ParameterElement)doc.GetElement(elementId);
            _doc = doc;
        }

    public InternalDefinition GetDefinition()
    {
            return _parameterElement.GetDefinition();
        }
    public override string ParameterName => _parameterElement.Name;
    public override Parameter GetParameter(Element element)
    {
            return element.get_Parameter(_parameterElement.GetDefinition());
        }

    public override ElementId Id => _parameterElement.Id;

    public override StorageType StorageType =>
        ParameterTypeToStorageType(_parameterElement.GetDefinition().ParameterType);

    private static StorageType ParameterTypeToStorageType(ParameterType parameterType)
    {
            switch (parameterType)
            {
                case ParameterType.Integer:
                    return StorageType.Integer;
                case ParameterType.YesNo:
                    return StorageType.Integer;
                case ParameterType.Material:
                    return StorageType.ElementId;
                case ParameterType.FamilyType:
                    return StorageType.ElementId;
                case ParameterType.Text:
                    return StorageType.String;
                case ParameterType.URL:
                    return StorageType.String;
                case ParameterType.Invalid:
                    return StorageType.None;
                default: return StorageType.Double;
            }
        }
}