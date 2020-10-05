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
using RevitParametersCodeGenerator;
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
                        var tmbDParameter = system.LookupParameter("ТЭ Д");
                        var tmbPParameter = system.LookupParameter("ТЭ П");


                        var first = 4;
                        if (first==1)
                        {
                            var d = system.get_Parameter(new Guid("387ba243-768e-45cf-9c22-ce1b5650fe3d")).AsDouble();
                            var isParsing = double.TryParse(
                                system.get_Parameter(new Guid("b4954a6d-3d42-44ff-b700-e308cf0fcc46")).AsString(),
                                out var du);
                            tmbDParameter.Set(UnitUtils.ConvertToInternalUnits(d, DisplayUnitType.DUT_METERS));
                            tmbPParameter.Set(du);
                        }
                        else if(first ==2)
                        {
                            var dParameter = system.get_Parameter(new Guid("387ba243-768e-45cf-9c22-ce1b5650fe3d"));
                            var duParameter = system.get_Parameter(new Guid("b4954a6d-3d42-44ff-b700-e308cf0fcc46"));
                            dParameter.Set(tmbDParameter.AsDouble());
                            duParameter.Set(tmbPParameter.AsDouble());

                        }
                        else if (first == 3)
                        {
                            var reserve0 = system.GetParameters("Резервная группа").First(p => !p.IsShared);
                            var control0 = system.GetParameters("Контрольные цепи").First(p => !p.IsShared);
                            var reserve = system.get_Parameter(new Guid("cd2dc469-276a-40f4-bd34-c6ab2ae05348"));
                            var control = system.get_Parameter(new Guid("0f13e1e5-71bb-4b0f-b3dc-18054c25e1ee"));

                            reserve.Set(reserve0.AsInteger());
                            control.Set(control0.AsInteger());
                        }
                        else if (first == 4)
                        {
                            //Запретить изменение
                            var dontChange = system.get_Parameter(new Guid("be64f474-c030-40cf-9975-6eaebe087a84"))
                                .Set(0);
                            //Запретить изменение наименование нагрузки
                            var dontChangeLoadName = system.get_Parameter(new Guid("5de14719-6968-4655-9457-94825e70b623")).Set(0);

                        }





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
