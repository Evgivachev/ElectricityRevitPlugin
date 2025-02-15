using System;
using Autodesk.Revit.DB;

namespace BimRenRes.QuickSelection.ParameterFunctions;

class Equals :ParameterFunction
{
    public override string Name => "Равно";
    public override FilterRule GetFilterRule(StorageType storageType, ElementId parameterId, string stringValue)
    {
            FilterRule filterRule = null;
            switch (storageType)
            {
                case StorageType.Integer:
                {
                    var intValue = Int32.Parse(stringValue);
                    filterRule = ParameterFilterRuleFactory.CreateEqualsRule(parameterId, intValue);
                    break;
                }
                case StorageType.String:
                    filterRule = ParameterFilterRuleFactory.CreateEqualsRule(parameterId, stringValue, CaseSensitive);
                    break;
                case StorageType.Double:
                {
                    var doubleValue = double.Parse(stringValue);
                    filterRule = ParameterFilterRuleFactory.CreateEqualsRule(parameterId, doubleValue, Epsilon);
                    break;
                }
                case StorageType.ElementId:
                {
                    var intValue = Int32.Parse(stringValue);
                    var elemId = new ElementId(intValue);
                    filterRule = ParameterFilterRuleFactory.CreateEqualsRule(parameterId, elemId);
                    break;
                }
            }

            return filterRule;
        }
}