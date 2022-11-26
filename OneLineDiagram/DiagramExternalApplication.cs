/*
namespace Diagrams
{
    using System;
    using System.Reflection;
    using System.Windows.Forms;
    using Autodesk.Revit.UI;
    using ExternalCommands;
    using ExternalCommands.OneLineDiagram;
    using Microsoft.Xaml.Behaviors;
    using ModelUpdate;
    using VCRevitRibbonUtilCustom;

    /// <summary>
    /// Revit external application.
    /// </summary>  
    public sealed partial class DiagramExternalApplication : IExternalApplication
    {
        Result IExternalApplication.OnStartup(UIControlledApplication uicApp)
        {
            var result = Result.Succeeded;
            Assembly.Load(typeof(Behavior).Assembly.Location);
            try
            {
                MyRibbon.GetApplicationRibbon(uicApp).Tab("ЭОМ")
                    .Panel("Схемы")
                    .CreateSplitButton("SB Схемы щитов", " SB Схемы щитов",
                        sb =>
                        {
                            sb.CreateButton<OneLineDiagramBuiltDiagram>("Схемы щитов", "Схемы щитов",
                                bt =>
                                    bt.SetLargeImage(Resource1.icons8_yoda_32)
                                        //TODO не работает
                                        //bt.SetSmallImage(Resource1.icons8_yoda_16) 
                                        .SetLongDescription<MyButton>("Создание однолинейных схем щитов")
                                        .SetContextualHelp<MyButton>(ContextualHelpType.Url, "https://www.revitapidocs.com/2019/")
                                        .SetHelpUrl<MyButton>("www.werfau.ru"));
                            sb.CreateButton<OneLineDiagramUpdateDiagram>("Обновить схему", "Обновить схему",
                                bt => bt.SetLargeImage(Resource1.icons8_c_3po_32)
                                    .SetLongDescription<MyButton>("Обновить однолинейные схемы щитов")
                                    .SetContextualHelp<MyButton>(ContextualHelpType.Url, "www.werfau.ru")
                                    .SetHelpUrl<MyButton>("https://www.revitapidocs.com/2019/"));
                            sb.CreateButton<ModelUpdaterByDiagramExternalCommand>("Обновить модель", "Обновить модель",
                                bt => bt.SetLargeImage(Resource1.iconfinder_clone_old_32)
                                    .SetLongDescription<MyButton>("Обновить модель по схеме")
                                    .SetContextualHelp<MyButton>(ContextualHelpType.Url, "https://www.revitapidocs.com/2019/")
                                    .SetHelpUrl<MyButton>("https://www.revitapidocs.com/2019/"));
                        }
                    )
                    .CreateButton<PhaseDistributionExternalCommand>("Распределение по фазам", "Распределение по фазам",
                        bt => bt.SetLargeImage(Resource1.icons8_death_star_32)
                            .SetLongDescription<MyButton>("Распределение   по фазам в щитах")
                            .SetContextualHelp<MyButton>(ContextualHelpType.Url, "https://www.revitapidocs.com/2019/")
                            .SetHelpUrl<MyButton>("https://www.revitapidocs.com/2019/"));
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
