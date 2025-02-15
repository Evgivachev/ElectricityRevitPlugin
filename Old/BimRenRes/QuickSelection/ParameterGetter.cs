using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace BimRenRes.QuickSelection;

public abstract class ParameterGetter
{
    public abstract string ParameterName { get; }
    public abstract Parameter GetParameter(Element element);
    public abstract ElementId Id { get; }
    public abstract StorageType StorageType { get; }

    public static ParameterGetter Create(Document doc, ElementId parameterId)
    {

            if (Enum.IsDefined(typeof(BuiltInParameter), parameterId.IntegerValue))
            {
                return new BuiltInParameterGetter(doc,(BuiltInParameter)parameterId.IntegerValue);
            }
            else
            {
                return new SharedAndGlobalParameterGetter(doc,parameterId);
            }

        }
}