namespace ElectricityRevitPlugin.UpdateModels
{
    using System.IO;
    using System.Linq;
    using System.Windows;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Events;

    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class UpTo21Version : DefaultExternalCommand
    {
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            try
            {
                //UiApp.DialogBoxShowing += UiApp_DialogBoxShowing;
                var dir = new DirectoryInfo(@"\\drive\RENESSANS\TEMP\temp__models");
                var models = dir.GetFiles().Take(5);
                var updater = new ModelUpdater
                {
                    TargetDirectory = @"\\drive\RENESSANS\TEMP\temp__models\2021"
                };
                foreach (var model in models)
                {
                    var flag = updater.TryUpdateModel(App, model);
                    MessageBox.Show(flag ? "true" : "false");
                }

                return Result.Succeeded;
            }
            finally
            {
                //UiApp.DialogBoxShowing -= UiApp_DialogBoxShowing;
            }
        }

        private void UiApp_DialogBoxShowing(object sender, DialogBoxShowingEventArgs e)
        {
            e.OverrideResult(1);
        }
    }
}
