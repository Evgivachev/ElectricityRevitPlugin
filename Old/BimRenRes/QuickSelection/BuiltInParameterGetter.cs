using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace BimRenRes.QuickSelection;

class BuiltInParameterGetter :ParameterGetter
{
    private readonly BuiltInParameter _builtInParameter;
    private readonly Document _doc;
    internal BuiltInParameterGetter(Document doc, BuiltInParameter builtInParameter)
    {
            _doc = doc;
            _builtInParameter = builtInParameter;
        }
    public override string ParameterName => LabelUtils.GetLabelFor(_builtInParameter);

    public override Parameter GetParameter(Element element)
    {
            return element.get_Parameter(_builtInParameter);
        }

    public override ElementId Id => new ElementId(_builtInParameter);

    public override StorageType StorageType => _doc.get_TypeOfStorage(_builtInParameter);
}