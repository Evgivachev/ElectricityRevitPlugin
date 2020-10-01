using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class Temp8 : DefaultExternalCommand
    {
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            try
            {
                var els = new FilteredElementCollector(Doc)
                    .OfCategory(BuiltInCategory.OST_ElectricalCircuit)
                    .OfType<ElectricalSystem>();
                using (var tr = new Transaction(Doc))
                {
                    tr.Start("sdsdsds");
                    foreach (var system in els)
                    {
                        var tmbDParameter = system.LookupParameter("ТМБ Д");
                        var tmbPParameter = system.LookupParameter("ТМБ П");

                       // var d = system.get_Parameter(new Guid("387ba243-768e-45cf-9c22-ce1b5650fe3d")).AsDouble();
                       //var isParsing =  double.TryParse(
                       //     system.get_Parameter(new Guid("b4954a6d-3d42-44ff-b700-e308cf0fcc46")).AsString(),
                       //     out var du);

                       // tmbDParameter.Set(UnitUtils.ConvertToInternalUnits(d, DisplayUnitType.DUT_METERS));
                       // tmbPParameter.Set(du);
                       var dParameter = system.get_Parameter(new Guid("387ba243-768e-45cf-9c22-ce1b5650fe3d"));
                       var duParameter = system.get_Parameter(new Guid("b4954a6d-3d42-44ff-b700-e308cf0fcc46"));

                        dParameter.Set(tmbDParameter.AsDouble());
                        duParameter.Set(tmbPParameter.AsDouble());

                    }
                    tr.Commit();
                }

                
            }

            catch (Exception e)
            {


                MessageBox.Show(e.Message + "\n" + e.StackTrace);
            }
            return Result.Succeeded;
        }
    }
}
