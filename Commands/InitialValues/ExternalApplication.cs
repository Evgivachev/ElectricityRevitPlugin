/*
using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using Autodesk.Revit.UI;
using System.Resources;
using System.Reflection;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using VCRevitRibbonUtil;

namespace InitialValues
{

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

                Ribbon.GetApplicationRibbon(uicApp).Tab("ЭОМ")
                    .Panel("Схемы")
                    .CreateButton<ExternalCommand>("Установить начальные значения", "Уст-ть начальные значения",
                        x =>
                        {
                            x.SetLargeImage(Resource1.icons8);
                            x.SetLongDescription("Установить начальные значения для всех электрических цепей");
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

