/*
namespace ElectricalLoadsExportToExcel
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using Autodesk.Revit.UI;
    using VCRevitRibbonUtilCustom;

    /// <summary>
    /// Revit external application.
    /// </summary>  
    public sealed partial class ExternalApplication : IExternalApplication
    {
        //Install-Package Revit-2017.1.1-x64.Base -Version 2.0.0
        private static readonly string ExecutingAssemblyPath = Assembly.GetExecutingAssembly().Location;

        Result IExternalApplication.OnStartup(UIControlledApplication uicApp)
        {
            var result = Result.Succeeded;
            try
            {
                var assemblyDir = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;

                MyRibbon.GetApplicationRibbon(uicApp).Tab("ЭОМ")
                    .Panel("Нагрузки")
                    .CreateButton<ExternalCommand>("Экспорт в Excel", "Экспорт в Excel",
                        x =>
                        {
                            x.SetLongDescription<MyButton>($"Экспорт электрических нагрузок в таблицу Excel");
                            x.SetLargeImage(Resource1.icons8);
                        }
                    );
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n" + e.StackTrace);
                return Result.Failed;
            }

            return result;
        }

        Result IExternalApplication.OnShutdown(UIControlledApplication uicApp)
        {
            return Result.Succeeded;
        }
    }
}
*/


