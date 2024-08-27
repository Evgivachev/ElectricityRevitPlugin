/*
namespace GeneralSubjectDiagram
{
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Selection;

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class TestPasteElectricalSystems : DefaultExternalCommand
    {
        protected ElementId famId =
            //new ElementId(24793778);
            new ElementId(24795719);

        protected override Result DoWork(ref string message, ElementSet elements)
        {
            var family = (Family)Doc.GetElement(famId);
            var familySymbol = (FamilySymbol)Doc.GetElement(family.GetFamilySymbolIds().First());
            using (var tr = new Transaction(Doc, "test1"))
            {
                tr.Start();
                var currentAssembly = Assembly.GetCallingAssembly();
                var updaterClassName = familySymbol.get_Parameter(ParameterUpdater.ReflectionClassNameGuid).AsString();
                var parameterUpdater = (ParameterUpdater)currentAssembly.CreateInstance(updaterClassName, false,
                    BindingFlags.CreateInstance, null, null, CultureInfo.InvariantCulture, null);
                var validateElements = parameterUpdater
                    .GetValidateElements(Doc);
                // .First(x=>x.Name == "ППУ");

                //foreach (Element treeNode in validateElements)
                //{
                //    var es =(ElectricalSystem) treeNode;
                //    var doc = es.Document;
                //    var points = new[]
                //    {
                //        PickPoint(),
                //       // PickPoint()
                //    };
                //   // var line = Line.CreateBound(points[0], points[1]);

                //    parameterUpdater = (ParameterUpdater)currentAssembly.CreateInstance(updaterClassName, false,
                //        BindingFlags.CreateInstance, null, new []{es}, CultureInfo.InvariantCulture, null);

                //    //  var instance = doc.Create.NewFamilyInstance(line, familySymbol, Doc.ActiveView);
                //    var instance = parameterUpdater.InsertInstance(familySymbol,points[0]);
                //        //doc.Create.NewFamilyInstance(points[0], familySymbol, doc.ActiveView);

                //    parameterUpdater.SetParameters(instance);
                //}
                tr.Commit();
            }

            return Result.Succeeded;
        }

        private XYZ PickPoint()
        {
            ObjectSnapTypes snapTypes = ObjectSnapTypes.Endpoints | ObjectSnapTypes.Intersections;
            XYZ point = UiDoc.Selection.PickPoint(snapTypes, "Select an end point or intersection");
            return point;
        }
    }
}
*/



