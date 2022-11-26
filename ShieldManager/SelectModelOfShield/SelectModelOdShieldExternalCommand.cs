namespace ShieldManager.SelectModelOfShield
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;

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
            try
            {
              
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
