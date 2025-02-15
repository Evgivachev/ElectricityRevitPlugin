using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace BimRenRes.QuickSelection.ParameterFunctions;

class Contains : ParameterFunction
{
    public override string Name => "Содержит";

    public override FilterRule GetFilterRule(StorageType storageType, ElementId parameterId, string value)
    {
            FilterRule filterRule = ParameterFilterRuleFactory.CreateContainsRule(parameterId, value, CaseSensitive);
            return filterRule;
        }
}