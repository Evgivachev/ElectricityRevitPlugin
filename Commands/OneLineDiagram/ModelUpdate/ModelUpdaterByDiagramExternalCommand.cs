namespace Diagrams.ModelUpdate
{
    using System;
    using System.Linq;
    using System.Windows;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Services;

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public sealed class ModelUpdaterByDiagramExternalCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var result = Result.Failed;
            try
            {
                var uiApp = commandData?.Application;
                var uiDoc = uiApp?.ActiveUIDocument;
                var app = uiApp?.Application;
                var doc = uiDoc?.Document;
                var schedules = new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewSchedule))
                    .Cast<ViewSchedule>()
                    .ToDictionary(x => x.Name);
                //Спецификации
                var sh1 = schedules["* Отключающее устройство 1"];
                var sh2 = schedules["* Отключающее устройство 2"];
                var sh3 = schedules["* Вводное отключающее устройство"];
                var cables = schedules["* Кабели"];
                var mUpdater = new ModelUpdater(doc);
                mUpdater.SetDevice1FromView(sh1);
                mUpdater.SetDevice2FromView(sh2);
                mUpdater.SetImputDeviceFromView(sh3);
                mUpdater.SetCablesFromView(cables);
                mUpdater.UpdateModelByHead();
                mUpdater.UpdateModelByLines();
                var updater = new DiagramsUpdater(uiApp, new DiagramsDrawer(uiApp));
                updater.UpdateDiagram(doc.ActiveView);
                result = Result.Succeeded;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + '\n' + ex.StackTrace, "Error");
                result = Result.Failed;
            }

            return result;
        }
    }
}
