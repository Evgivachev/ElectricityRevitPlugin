using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MoreLinq;

namespace ShieldPanel.SelectModelOfShield
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SelectModelOdShieldExternalCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var result = Result.Succeeded;
            var application = commandData?.Application;
            var activeUiDocument = application?.ActiveUIDocument;
            var document = activeUiDocument?.Document;
            var app = application?.Application;

            //var updsters = UpdaterRegistry.GetRegisteredUpdaterInfos();
            //foreach (var u in updsters)
            //{
            //    MessageBox.Show($"{u.AdditionalInformation}\n{u.ApplicationName}\n{u.UpdaterName}");
            //}

            try
            {
                using (var transactionGroup = new TransactionGroup(document, "trGrName"))
                {
                    if (TransactionStatus.Started == transactionGroup.Start())
                    {
                        var shieldGetter = new ShieldGetter(commandData);
                        var shields = shieldGetter
                            .GetShields()
                            .OrderBy(x=>x.Name)
                            .ToArray();

                        var schedule = new FilteredElementCollector(document)
                            .OfCategory(BuiltInCategory.OST_Schedules)
                            .First(x => x.Name == "* Оболочки щитов") as ViewSchedule;
                        if (schedule is null)
                        {
                            throw new Exception("Не удалось найти спецификацию \"*Оболочки щитов\"");
                        }

                        var catalog = new FilteredElementCollector(document, schedule.Id)
                                //    .OfType<Element>()
                                .ToArray()
                            ;
                        var selection = activeUiDocument?.Selection.GetElementIds();
                        var selectionElements = selection.Select(x => x.IntegerValue).ToHashSet();
                        var wpf = new SelectModelOfShieldWPF(shields, catalog,selectionElements);
                        wpf.ShowDialog();
                    }
                    transactionGroup.Assimilate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error");
                result = Result.Failed;
            }
            return result;
        }
    }
}
