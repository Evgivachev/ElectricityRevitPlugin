using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace BimRenRes.QuickSelection.ParameterFunctions;

class Greater : ParameterFunction
{
    public override string Name => "Больше";
    public override FilterRule GetFilterRule(StorageType storageType, ElementId parameterId, string value)
    {
            FilterRule filterRule = null;
            switch (storageType)
            {
                case StorageType.Integer:
                {
                    var intValue = int.Parse(value);
                    filterRule = ParameterFilterRuleFactory.CreateGreaterRule(parameterId, intValue);
                    break;
                }
                case StorageType.String:
                    filterRule = ParameterFilterRuleFactory.CreateGreaterRule(parameterId, value, CaseSensitive);
                    break;
                case StorageType.Double:
                {
                    var doubleValue = double.Parse(value);
                    filterRule = ParameterFilterRuleFactory.CreateGreaterRule(parameterId, doubleValue, Epsilon);
                    break;
                }
                case StorageType.ElementId:
                {
                    var intValue = int.Parse(value);
                    var elemId = new ElementId(intValue);
                    filterRule = ParameterFilterRuleFactory.CreateGreaterRule(parameterId, elemId);
                    break;
                }
            }

            return filterRule;
        }
}