using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace ElectricityRevitPlugin.GeneralSubject
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class TestPastleAnnotation : DefaultExternalCommand
    {

        protected override Result DoWork(ref string message, ElementSet elements)
        {
            var famId = new ElementId(24793778);
            var family = (Family)Doc.GetElement(famId);
            var familySymbol = (FamilySymbol)Doc.GetElement(family.GetFamilySymbolIds().First());

            using (var tr = new Transaction(Doc, "test1"))
            {
                var point = new XYZ();
                var line = Line.CreateBound(point, point + new XYZ(0, 0.1, 0));
                tr.Start();
               
                var instance = Doc.Create.NewFamilyInstance(line, familySymbol, Doc.ActiveView);
                UiDoc.Selection.SetElementIds(new List<ElementId>{instance.Id});
                tr.Commit();
            }
            return Result.Succeeded;
        }

       
    }
}
