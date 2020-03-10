using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using VCRevitRibbonUtilCustom;

namespace DuctSystemRevitPlugin
{
    class DuctSystemExternalApp : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication uicApp)
        {
            var result = Result.Succeeded;
            try
            {
                MyRibbon.GetApplicationRibbon(uicApp)
                    .Tab("ОВиК")
                    .Panel("Виды")
                    .CreateButton<CreateViewsExternalCommand>("Создание видов для вентиляционных систем", "Создание видов",
                        bt =>
                            bt.SetLargeImage(Resource1.icons8_гомер_симпсон_32)
                                .SetLongDescription<MyButton>("Создание видов для вентиляционных систем")
                                .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                                    "https://www.revitapidocs.com/2019/"));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n" + e.StackTrace);
                return Result.Failed;
            }
            return result;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
