using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace BimRenRes.QuickSelection.ParameterFunctions;

class EndsWith : ParameterFunction
{
    public override string Name => "Заканчивается на";

    public override FilterRule GetFilterRule(StorageType storageType, ElementId parameterId, string value)
    {
            var filterRule = ParameterFilterRuleFactory.CreateEndsWithRule(parameterId, value, CaseSensitive);
            return filterRule;
        }
}