using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ElectricityRevitPlugin.GeneralSubject
{

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class Test1 : DefaultExternalCommand
    {
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            var sharedParameterApplicableRule = new[]
            {
                new SharedParameterApplicableRule("ID связанного элемента"),
                new SharedParameterApplicableRule("ReflectionClassName")
            };
            var elementParameterFilter = new ElementParameterFilter(sharedParameterApplicableRule);

            var allElements = new FilteredElementCollector(Doc)
                .OfClass(typeof(FamilyInstance))
                .WherePasses(elementParameterFilter)
                .WhereElementIsNotElementType()
                .Where(x => x.OwnerViewId.IntegerValue == Doc.ActiveView.Id.IntegerValue);






            using (var tr = new Transaction(Doc, "test1"))
            {
                tr.Start();
                foreach (var toElement in allElements)
                {
                    var toElementType = Doc.GetElement(toElement.GetTypeId());

                    var id = toElement.Id.IntegerValue;
                    //24751880
                    var linkId = toElement.get_Parameter(new Guid("dca1fe51-4090-4178-9f12-a83aa5986266")).AsString()
                        .Trim();
                    var updaterClassName = toElementType.get_Parameter(new Guid("6c36d5e8-7863-4efb-accf-894a5aa95cc1")).AsString();
                    var currentAssembly = Assembly.GetCallingAssembly();
                   

                    if (string.IsNullOrEmpty(linkId))
                        continue;
                    var fromElemId = new ElementId(int.Parse(linkId));

                    var fromElement = Doc.GetElement(fromElemId);
                    if(fromElement is null)
                        continue;

                    var parameterUpdater = (ParameterUpdater) currentAssembly.CreateInstance(updaterClassName, false,
                        BindingFlags.CreateInstance, null, new[] {fromElement}, CultureInfo.InvariantCulture, null);
                  //  parameterUpdater = new CableParameterUpdater(fromElement);
                  Debug.Assert(parameterUpdater != null, nameof(parameterUpdater) + " != null");
                  //parameterUpdater.SetSameParameters(toElement);
                  //  Doc.Regenerate();
                    parameterUpdater.SetParameters(toElement);

                }



                tr.Commit();
            }

            return Result.Succeeded;
        }
    }
}
