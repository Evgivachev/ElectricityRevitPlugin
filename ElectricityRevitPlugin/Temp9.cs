using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class Temp9 : DefaultExternalCommand
    {
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            var activeView = Doc.ActiveView as View3D;
            var bb = activeView.get_BoundingBox(activeView);
            var transform = bb.Transform;

            var eyePosition = new XYZ(-1, 1, 1);
            var upDirection = new XYZ(1,1,2);
            var forwardDirection =new XYZ(1,1,-1);
            var viewOrientation = new ViewOrientation3D(eyePosition, upDirection, forwardDirection);


            try
            {
                using (var tr = new Transaction(Doc))
                {
                    tr.Start("Temp");
                    activeView.SetOrientation(viewOrientation);
                    tr.Commit();
                }


            }

            catch (OperationCanceledException e)
            {
                return Result.Failed;
            }

            return Result.Succeeded;
        }
    }
}