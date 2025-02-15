using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.UI;
using BimRenRes.AddParametersToFamilyInstance;
using BimRenRes.QuickSelection;
using BimRenRes.Sheets;
using VCRevitRibbonUtilCustom;

namespace BimRenRes;

class CommonPluginsExternalApplication : IExternalApplication
{
    public Result OnStartup(UIControlledApplication uicApp)
    {
            var result = Result.Succeeded;
            try
            {
                MyRibbon.GetApplicationRibbon(uicApp)
                    .Tab("REN_Общие")
                    .Panel("Выделение").CreateButton<QuickSelection.ExternalCommand>("Фильтр",
                        "Фильтр",
                        bt =>
                            bt.SetLargeImage(Resource1.icons8_фильтр_32)
                                .SetLongDescription<MyButton>("Фильтр выделения элементов")
                                .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                                    "https://www.revitapidocs.com/2019/"));


                MyRibbon.GetApplicationRibbon(uicApp)
                    .Tab("REN_Общие")
                    .Panel("Листы")
                    .CreateButton<PrintAndExport.ExternalCommand>("ПечатьRenRes", "Экспорт и печать",
                        bt =>
                        {
                            bt.SetLargeImage(Resource1.icons8_отправить_на_принтер_32)
                                .SetLongDescription<MyButton>("Экспорт и печать")
                                .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                                    "https://docs.google.com/document/d/1bJfD8KL2IskJyKKjoUVWxbimqIFLGUmh7mbLAluRVVU/edit?usp=sharing");
                        })
                    .CreateSplitButton("SB Листы1 RenRes", " SB Листы1",
                        sb =>
                        {
                            sb.CreateButton<SortSheets>("Сортировка листов RenRes", "Сортировка листов",
                                bt =>
                                    bt.SetLargeImage(Resource1.icons8_futurama_professor_farnsworth_32)
                                        .SetLongDescription<MyButton>("Сортировка листов")
                                        .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                                            "https://www.revitapidocs.com/2019/"));
                            sb.CreateButton<SelectFramesFromSelectedSheets>("Выбрать рамки RenRes", "Выбрать рамки",
                                bt => bt.SetLargeImage(Resource1.icons8_futurama_fry_32)
                                    .SetLongDescription<MyButton>("Выбрать семейства основной надписи на листах")
                                    .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                                        "https://www.revitapidocs.com/2019/"));

                        })
                    .CreateSplitButton("SB Листы2 RenRes", " SB Листы2",
                        sb =>
                        {
                            sb.CreateButton<SelectSheetFormatAndAlignExternalCommand>("Подобрать рамки RenRes", "Подобрать рамки",
                                bt =>
                                    bt.SetLargeImage(Resource1.icons8_futurama_bender_32)
                                        .SetLongDescription<MyButton>("Подобрать рамки")
                                        .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                                            "https://www.revitapidocs.com/2019/"));
                            sb.CreateButton<SelectSheetFormatAndAlignOnDiagramExternalCommand>("Подобрать рамки для схем RenRes", "Подобрать рамки для схем",
                                bt => bt.SetLargeImage(Resource1.icons8_futurama_zoidberg_32)
                                    .SetLongDescription<MyButton>("Подобрать рамки для схем")
                                    .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                                        "https://www.revitapidocs.com/2019/"));
                        })
                    .CreateButton<DuplicateSheets>("Дубликатор листов","Дубликатор листов", 
                        bt=> bt.SetLargeImage(Resource1.icons8_futurama_zapp_brannigan_32)
                            .SetLongDescription<MyButton>("Дубликатор листов")
                            .SetContextualHelp<MyButton>(ContextualHelpType.Url, "https://docs.google.com/document/d/1KLIH1oNn6nLQZ6SW6IL7AJHMlMtvZ9ngFY3jxKkBwQU/edit?usp=sharing")
                        )
                    ;
                MyRibbon.GetApplicationRibbon(uicApp)
                    .Tab("REN_Общие")
                    .Panel("Семейства")
                    .CreateButton<AddingParametersToFamilyExternalCommand>("Добавить параметры", "Добавить параметры",
                        bt => bt.SetLargeImage(Resource1.icons8_vomiting_unicorn_32)
                            .SetLongDescription<MyButton>("Добавить группу параметров из файла общих параметров")
                            .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                                "https://docs.google.com/document/d/1JPkZ0wvUon4Uni0CLzT-Gfr0yrckLpD8z07f2GO34_8/edit?usp=sharing")
                    );
                MyRibbon.GetApplicationRibbon(uicApp)
                    .Tab("REN_Общие")
                    .Panel("Разное")
                    .CreateButton<MarkingOpeningExternalCommand>("Промаркировать отверстия", "Промаркировать отверстия",
                        bt => bt.SetLargeImage(Resource1.icons8_edvard_munch_32)
                            .SetLongDescription<MyButton>("Маркировка отметок снизу и сверху для отверстий")
                            .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                                "https://docs.google.com/document/d/1-C3cP9Ap674xPhFGqV6NOwI1nouc-KcouNiTwjNjyxM/edit?usp=sharing")
                    );
                MyRibbon.GetApplicationRibbon(uicApp)
                    .Tab("REN_Общие")
                    .Panel("Разное")
                    .CreateButton<SchedulesToCsv>("Экспорт в *CSV", "Экспорт в *CSV",
                        bt => bt.SetLargeImage(Resource1.icons8_txt_32)
                            .SetLongDescription<MyButton>("Выгрузить выделенные спецификации в *csv")
                    );
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

    public static CommonPluginsExternalApplication GetApplication(UIApplication uiApp)
    {
            var apps = uiApp.LoadedApplications;
            foreach (IExternalApplication app in apps)
            {
                if (app is CommonPluginsExternalApplication myApp)
                    return myApp;
            }
            return null;
        }
}