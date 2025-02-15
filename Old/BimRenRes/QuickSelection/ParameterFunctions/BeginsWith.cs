using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace BimRenRes.QuickSelection.ParameterFunctions;

class BeginsWith : ParameterFunction
{
    public override string Name => "Начинается с";
    public override FilterRule GetFilterRule(StorageType storageType, ElementId parameterId, string value)
    {
            FilterRule filterRule = ParameterFilterRuleFactory.CreateBeginsWithRule(parameterId,value,CaseSensitive);
            return filterRule;
        }
}