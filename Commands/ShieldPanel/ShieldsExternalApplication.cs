/*
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ShieldPanel.SelectModelOfShield;
using VCRevitRibbonUtilCustom;

namespace ShieldPanel
{

    public sealed partial class ShieldExternalApplication : IExternalApplication
    {
        private static IUpdater _updater;
        Result IExternalApplication.OnStartup(UIControlledApplication uicApp)
        {
            var result = Result.Succeeded;
            try
            {
                var assemblyDir = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;

                MyRibbon.GetApplicationRibbon(uicApp).Tab("ЭОМ")
                    .Panel("Щиты")
                    .CreateButton<SelectModelOdShieldExternalCommand>("Щиты1", "Обработать",
                        x =>
                        {
                            x.SetLargeImage(Resource1.icons8_супермен_32)
                                .SetLongDescription<MyButton>(
                                    "Подобрать оболочку щита и обновить внешний вид");
                        }
                    );

                //DMU
                var updater = new ShieldParametersIUpdater(Guid.NewGuid());
                _updater = updater;
                UpdaterRegistry.RegisterUpdater(updater, false);
                var filter = new ElementCategoryFilter(BuiltInCategory.OST_ElectricalEquipment);
                var parameterW = new ElementId(20699077);
                UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), filter, Element.GetChangeTypeParameter(parameterW) );
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
            UpdaterRegistry.RemoveAllTriggers(_updater.GetUpdaterId());
            UpdaterRegistry.UnregisterUpdater(_updater.GetUpdaterId());
            return Result.Succeeded;
        }
    }
}
*/



