using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitParametersCodeGenerator;

namespace ElectricityRevitPlugin.GroupByGost
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class GeneralSubjectGroupByGost : DefaultExternalCommand
    {
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            var elementsOnCurrentView = new FilteredElementCollector(Doc,Doc.ActiveView.Id)
                .WherePasses(new ElementParameterFilter(ParameterFilterRuleFactory.CreateSharedParameterApplicableRule("Номер группы по ГОСТ")))
                .OfType<FamilyInstance>();
            using (var tr = new Transaction(Doc, "groupByGost"))
            {
                tr.Start();
                foreach (var el in elementsOnCurrentView)
                {
                    var parentElementId = el.get_Parameter(SharedParametersFile.ID_Svyazannogo_Elementa)?.AsString();
                    if (int.TryParse(parentElementId, out var parameterElementId))
                    {
                        var parentElement = Doc.GetElement(new ElementId(parameterElementId));
                        if(parentElement is null)
                            continue;
                        var groupByGost = parentElement.get_Parameter(SharedParametersFile.Nomer_Gruppy_Po_GOST)?.AsString();
                        if (string.IsNullOrEmpty(groupByGost))
                            groupByGost = "&&&";
                        var flag = el.get_Parameter(SharedParametersFile.Nomer_Gruppy_Po_GOST).Set(groupByGost);
                    }
                }
                tr.Commit();
            }
            return Result.Succeeded;
        }
    }
}
