using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace BimRenRes.QuickSelection.ParameterFunctions;

class NotContains :ParameterFunction
{
    public override string Name => "Не содержит";

    public override FilterRule GetFilterRule(StorageType storageType, ElementId parameterId, string value)
    {
            FilterRule filterRule = ParameterFilterRuleFactory.CreateNotContainsRule(parameterId, value, CaseSensitive);
            return filterRule;
        }
}