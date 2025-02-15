using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace BimRenRes.QuickSelection.ParameterFunctions;

class Less : ParameterFunction
{
    public override string Name => "Меньше";
    public override FilterRule GetFilterRule(StorageType storageType, ElementId parameterId, string value)
    {
            FilterRule filterRule = null;
            switch (storageType)
            {
                case StorageType.Integer:
                {
                    var intValue = Int32.Parse(value);
                    filterRule = ParameterFilterRuleFactory.CreateLessRule(parameterId, intValue);
                    break;
                }
                case StorageType.String:
                    filterRule = ParameterFilterRuleFactory.CreateLessRule(parameterId, value, CaseSensitive);
                    break;
                case StorageType.Double:
                {
                    var doubleValue = double.Parse(value);
                    filterRule = ParameterFilterRuleFactory.CreateLessRule(parameterId, doubleValue, Epsilon);
                    break;
                }
                case StorageType.ElementId:
                {
                    var intValue = Int32.Parse(value);
                    var elemId = new ElementId(intValue);
                    filterRule = ParameterFilterRuleFactory.CreateLessRule(parameterId, elemId);
                    break;
                }
            }
            return filterRule;
        }
}