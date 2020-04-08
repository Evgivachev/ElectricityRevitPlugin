using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace ElectricityRevitPlugin.GeneralSubject
{
    public class GeneralSubjectViewModel
    {
        private Document _doc;
        private UIDocument _uiDoc;
        public bool IsHideExistingElements = false;
        public FamilySymbol SelectedFamilySymbol { get; set; }
        //public IList<TreeNode> TreeNodes => GetTreeView(SelectedFamilySymbol);
        public GeneralSubjectViewModel(UIDocument uiDocument)
        {
            _uiDoc = uiDocument;
            _doc = uiDocument.Document;
        }

        public IList<Element> GetAvailableFamilySymbols()
        {
            var sharedParameterApplicableRule = new[]
            {
               // new SharedParameterApplicableRule("ID связанного элемента"),
                new SharedParameterApplicableRule("ReflectionClassName")
            };
            //var elementParameterFilter = new ElementParameterFilter(sharedParameterApplicableRule);
            var elementParameterFilter = new ElementParameterFilter(new SharedParameterApplicableRule("ReflectionClassName"));
            var allElements = new FilteredElementCollector(_doc)
              .OfClass(typeof(FamilySymbol))
              .WhereElementIsElementType()
              //.WherePasses(elementParameterFilter)
              .OfType<FamilySymbol>()
               .Where(x =>
              {
                  var updaterClassName = x.get_Parameter(ParameterUpdater.ReflectionClassNameGuid)?.AsString();
                  return !string.IsNullOrEmpty(updaterClassName);
                  return x.FamilyName.Contains("ВРУ");
              })
                .ToArray();
            return allElements;
        }

        public MyCollectionOfCheckableItems GetTreeView(FamilySymbol familySymbol)
        {
            var currentAssembly = Assembly.GetCallingAssembly();
            var updaterClassName = familySymbol.get_Parameter(ParameterUpdater.ReflectionClassNameGuid).AsString();
            var parameterUpdater = (ParameterUpdater)currentAssembly.CreateInstance(updaterClassName, false,
                BindingFlags.CreateInstance, null, null, CultureInfo.InvariantCulture, null);
            var validateElements = parameterUpdater
                .GetValidateElements(_doc);

            return validateElements;
        }

        public List<FamilyInstance> InsertInstances(IEnumerable<Element> selectedElements)
        {
            var insertedElement = new List<FamilyInstance>();
            var currentAssembly = Assembly.GetCallingAssembly();
            var fs = SelectedFamilySymbol;
            var updaterClassName = fs.get_Parameter(ParameterUpdater.ReflectionClassNameGuid).AsString();
            
            using (var tr = new Transaction(_doc,"Вставка элементов схемы ВРУ"))
            {
                tr.Start();
                foreach (var element in selectedElements)
                {
                    var point = PickPoint();
                    var parameterUpdater = (ParameterUpdater)currentAssembly.CreateInstance(updaterClassName, false,
                        BindingFlags.CreateInstance, null, new object[] { element }, CultureInfo.InvariantCulture, null);
                    var instance = parameterUpdater.InsertInstance(fs, point);
                    insertedElement.Add(instance);
                    parameterUpdater.SetParameters(instance);
                    _doc.Regenerate();
                }
                tr.Commit();
            }
            

            return insertedElement;
            // var line = Line.CreateBound(points[0], points[1]);
            //  var instance = doc.Create.NewFamilyInstance(line, familySymbol, Doc.ActiveView);
        }

        private XYZ PickPoint()
        {
            ObjectSnapTypes snapTypes = ObjectSnapTypes.Endpoints | ObjectSnapTypes.Intersections;
            XYZ point = _uiDoc.Selection.PickPoint(snapTypes, "Select an end point or intersection");
            return point;
        }
    }
}
