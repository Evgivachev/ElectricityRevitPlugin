using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace BimRenRes.QuickSelection.ParameterFunctions;

class NotEquals : ParameterFunction
{
    public override string Name => "Не равно";

    public override FilterRule GetFilterRule(StorageType storageType, ElementId parameterId, string value)
    {
            FilterRule filterRule = null;
            switch (storageType)
            {
                case StorageType.Integer:
                {
                    var intValue = Int32.Parse(value);
                    filterRule = ParameterFilterRuleFactory.CreateNotEqualsRule(parameterId, intValue);
                    break;
                }
                case StorageType.String:
                    filterRule = ParameterFilterRuleFactory.CreateNotEqualsRule(parameterId, value, CaseSensitive);
                    break;
                case StorageType.Double:
                {
                    var doubleValue = double.Parse(value);
                    filterRule = ParameterFilterRuleFactory.CreateNotEqualsRule(parameterId, doubleValue, Epsilon);
                    break;
                }
                case StorageType.ElementId:
                {
                    var intValue = Int32.Parse(value);
                    var elemId = new ElementId(intValue);
                    filterRule = ParameterFilterRuleFactory.CreateNotEqualsRule(parameterId, elemId);
                    break;
                }
            }

            return filterRule;
        }
}