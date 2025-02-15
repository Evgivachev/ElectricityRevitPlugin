using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace BimRenRes.QuickSelection.ParameterFunctions;

class NotBeginsWith : ParameterFunction
{
    public override string Name => "Не начинается с";
    public override FilterRule GetFilterRule(StorageType storageType, ElementId parameterId, string value)
    {
            return ParameterFilterRuleFactory.CreateNotBeginsWithRule(parameterId, value, CaseSensitive);
        }
}