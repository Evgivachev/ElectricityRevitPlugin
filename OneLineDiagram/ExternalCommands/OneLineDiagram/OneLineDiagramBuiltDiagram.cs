#region Namespaces

#endregion

namespace Diagrams.ExternalCommands.OneLineDiagram
{
    using System;
    using System.Windows.Forms;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Diagrams.View;
    using Diagrams.ViewContext;
    using Services;

    /// <summary>
    /// Revit external command.
    /// </summary>	
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public sealed partial class OneLineDiagramBuiltDiagram : IExternalCommand
    {
        internal static ExternalCommandData? CommandData;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandData">An ExternalCommandData object which contains reference to Application and View needed by external command.</param>
        /// <param name="message">Error message can be returned by external command. This will be displayed only if the command status was "Failed". There is a limit of 1023 characters for this message; strings longer than this will be truncated.</param>
        /// <param name="elements">Element set indicating problem elements to display in the failure dialog. This will be used only if the command status was "Failed".</param>
        /// <returns></returns>
        Result IExternalCommand.Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            CommandData = commandData;
            var result = Result.Failed;
            try
            {
                var uiApp = commandData.Application;
                var uiDoc = uiApp?.ActiveUIDocument;
                var app = uiApp?.Application;
                var doc = uiDoc?.Document;
                var drawer = new DiagramsDrawer(uiApp);
                var updater = new DiagramsUpdater(uiApp, drawer);
                var dc = new BuildDiagramsContext(new ShieldProvider(uiApp!), uiApp, drawer, updater);
                var view = new BuildDiagramsView(dc);
                view.ShowDialog();
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
