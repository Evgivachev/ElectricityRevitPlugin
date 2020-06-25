using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;

namespace ElectricityRevitPlugin.UpdateParametersInCircuits
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class TestUpdate : DefaultExternalCommand

    {
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            using (var tr = new Transaction(Doc))
            {
                tr.Start("Test");
                var updater = new SetLengthForElectricalSystemsExternalCommand();
                var systemId = new ElementId(24437301);
                var system = Doc.GetElement(systemId) as ElectricalSystem;

                updater.UpdateParameters(system);
                tr.Commit();
            }
            return Result.Succeeded;
        }
    }
}
