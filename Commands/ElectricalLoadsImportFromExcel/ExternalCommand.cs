// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SpecificationRevitToExcel
 * ExternalCommand.cs
 * https://revit-addins.blogspot.ru
 * © EvgIv, 2018
 *
 * The external command.
 */

#region Namespaces

#endregion

namespace ElectricalLoadsImportFromExcel
{
    using System;
    using System.Windows;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;

    /// <summary>
    /// Revit external command.
    /// </summary>	
    [Transaction(TransactionMode.Manual)]
    public sealed partial class ExternalCommand : IExternalCommand, IExternalCommandAvailability
    {
        Result IExternalCommand.Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //var resMng = new ResourceManager(GetType());
            //var defResMng = new ResourceManager(typeof(Properties.Resources));
            var result = Result.Failed;
            try
            {
                var uiApp = commandData?.Application;
                var uiDoc = uiApp?.ActiveUIDocument;
                var doc = uiDoc?.Document;
                using (var trGr = new TransactionGroup(doc, "trGrName"))
                {
                    if (TransactionStatus.Started == trGr.Start())
                    {
                        /* Here do your work or the set of
                         * works... */
                        if (DoWork(commandData, ref message,
                                elements))
                        {
                            if (TransactionStatus.Committed == trGr.Assimilate())
                                result = Result.Succeeded;
                        }
                        else
                            trGr.RollBack();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + '\n' + ex.StackTrace, "Error");
                result = Result.Failed;
            }
            finally
            {
                //resMng.ReleaseAllResources();
                //defResMng.ReleaseAllResources();
            }

            return result;
        }

        /// <inheritdoc />
        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            return applicationData.ActiveUIDocument?.Document is not null;
        }
    }
}
