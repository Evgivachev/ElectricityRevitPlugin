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
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using VCRevitRibbonUtilCustom;
using MessageBox = System.Windows.Forms.MessageBox;

namespace MarkingElectricalSystems
{
    public class CircuitPanelExternalApplication : IExternalApplication
    {
        Result IExternalApplication.OnStartup(UIControlledApplication uicApp)
        {
            var result = Result.Succeeded;
            try
            {
                MyRibbon.GetApplicationRibbon(uicApp).Tab("ЭОМ")
                    .Panel("Цепи")
                    .CreateButton<ShiftElectricalCircuits>("ShiftCircuits", "Установить смещение", b =>
                    {
                        b.SetLargeImage(Resource1.icons8_агент_смит_32)
                            .SetLongDescription<MyButton>(
                                "Установить траекторию всех цепей через все устройства и смещение");
                    })
                    .CreateSplitButton("SB Маркировка цепей", "SB Маркировка цепей", sb =>
                    {
                        sb.CreateButton<MarkingOfCircuitsExternalCommand>("MarkingOfCircuitsExternalCommand", "Маркировка цепей", b =>
                         {
                             b.SetLargeImage(Resource1.icons8_нео_32)
                                 .SetLongDescription<MyButton>("Маркировка групп цепей");
                         })
                     .CreateButton<UpdatingMarkingOfCircuitsExternalCommand>("UpdatingMarkingOfCircuitsExternalCommand", "Обновление маркировок цепей", b =>
                     {
                         b.SetLargeImage(Resource1.icons8_морфеус_32)
                             .SetLongDescription<MyButton>("Обновление маркировок цепей");
                     });
                    });

                    ;

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



