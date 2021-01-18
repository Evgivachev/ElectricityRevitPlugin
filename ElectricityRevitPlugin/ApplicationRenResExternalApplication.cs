using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using ElectricityRevitPlugin.UpdateParametersInCircuits;
using ElectricityRevitPlugin.Updaters;
using RevitParametersCodeGenerator;
using UpdateNameSpace;
using VCRevitRibbonUtilCustom;
using Application = Autodesk.Revit.ApplicationServices.Application;

namespace ElectricityRevitPlugin
{
    class ApplicationRenResExternalApplication : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication uicApp)
        {
            var result = Result.Succeeded;
            try
            {
                MyRibbon.GetApplicationRibbon(uicApp)
                    .Tab("ЭОМ")
                    .Panel("Общая")
                    .CreateStackedItems(si => si.CreateButton<SetElementCoordinatesExternalCommand>("Задать координаты", "Задать координаты",
                                bt =>
                                    bt.SetSmallImage(Resource1.icons8_капитан_америка_16)
                                        .SetLongDescription<MyButton>("Задать координаты")
                                        .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                                            "https://www.revitapidocs.com/2019/"))
                    .CreateButton<SetInstallationHeightExternalRelativeToLevelExternalCommand>("Высота установки", "Высота установки",
                                bt =>
                                    bt.SetSmallImage(Resource1.icons8_халк_16)
                                        .SetLongDescription<MyButton>("Высота установки")
                                        .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                                            "https://www.revitapidocs.com/2019/"))
                    );
                MyRibbon.GetApplicationRibbon(uicApp)
                .Tab("ЭОМ")
                .Panel("Обновление")
                .CreateButton<UpdateParametersOfElectricalSystemIExernalCommand>("Обновить цепи", "Обновить цепи",
                            bt =>

                                bt.SetLargeImage(Resource1.icons8_тринити_32)
                                    .SetLongDescription<MyButton>("Обновить цепи")
                                    .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                                        "https://www.revitapidocs.com/2019/")
                    );
                MyRibbon.GetApplicationRibbon(uicApp)
                    .Tab("REN_ЭОМ")
                    .Panel("Обновление")
                    .CreateButton<UpdaterParametersOfShields>("Обновить параметры в щитах", "Обновить параметры в щитах", b =>
                    {
                        b.SetContextualHelp<MyButton>(ContextualHelpType.Url, "https://docs.google.com/document/d/1fyB0MJm0YujIILterNRl4a_bm4gCdKuK8uN3iU6MpMw/edit?usp=sharing")
                            .SetHelpUrl<MyButton>("www.werfau.ru")
                            .SetLongDescription<MyButton>("Обновление параметров \"Максимальный ток ОУ на группах в щитах\" и \"Количество модулей в щитах\"")
                            .SetLargeImage(Resource1.icons8_house_stark_32);
                    });

                uicApp.ControlledApplication.DocumentOpened += RegisterUpdaters;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n" + e.StackTrace);
                return Result.Failed;
            }
            return result;
        }
        
        private void RegisterUpdaters(object sender, DocumentOpenedEventArgs e)
        {
            try
            {
                var doc = e.Document;
                var uicApp = sender as Application;

                var sharedParameters = new FilteredElementCollector(doc)
                    .OfClass(typeof(SharedParameterElement))
                    .Cast<SharedParameterElement>()
                    .ToDictionary(p => p.GuidValue);

                var projectParameters = new FilteredElementCollector(doc)
                    .OfClass(typeof(ParameterElement))
                    .Cast<ParameterElement>()
                    .Where(p => !string.IsNullOrEmpty(p?.Name))
                    .Where(p =>
                    {
                        if (p is SharedParameterElement)
                            return false;
                        return true;
                    })
                    .Distinct(new ProjectParameterNameComparer())
                    .ToDictionary(x=>x.Name);

                #region Группы по ГОСТ
                //обновление групп по ГОСТ
                var groupByGostUpdater = new Updaters.GroupByGost(uicApp.ActiveAddInId);
                groupByGostUpdater.RegisterUpdater(doc);
                //UpdaterRegistry.RegisterUpdater(groupByGostUpdater,doc,true);
                UpdaterRegistry.AddTrigger(groupByGostUpdater.GetUpdaterId(),
                    groupByGostUpdater.ElementFilter,
                    Element.GetChangeTypeAny());


                #endregion


                #region Потери напряжения
                var lossVoltageUpdater = new LossVoltage(uicApp.ActiveAddInId);
                lossVoltageUpdater.RegisterUpdater(doc);
                //UpdaterRegistry.RegisterUpdater(lossVoltageUpdater,  doc,true);
                UpdaterRegistry.AddTrigger(lossVoltageUpdater.GetUpdaterId(),
                    lossVoltageUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.RBS_ELEC_VOLTAGE)));
                //Полная установленная мощность
                UpdaterRegistry.AddTrigger(lossVoltageUpdater.GetUpdaterId(),
                    lossVoltageUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.RBS_ELEC_APPARENT_LOAD)));
                //Активная мощность в щитах
                UpdaterRegistry.AddTrigger(lossVoltageUpdater.GetUpdaterId(),
                    lossVoltageUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(sharedParameters[SharedParametersFile.Aktivnaya_Moshchnost_V_SHCHitakh].Id));
                //Реактивная мощность в щитах
                UpdaterRegistry.AddTrigger(lossVoltageUpdater.GetUpdaterId(),
                    lossVoltageUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(sharedParameters[SharedParametersFile.Reaktivnaya_Moshchnost_V_SHCHitakh].Id));
                //Коэффициент спроса в щитах
                UpdaterRegistry.AddTrigger(lossVoltageUpdater.GetUpdaterId(),
                    lossVoltageUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(sharedParameters[SharedParametersFile.Koeffitsient_Sprosa_V_SHCHitakh].Id));
                //Полная мощность в щитах
                UpdaterRegistry.AddTrigger(lossVoltageUpdater.GetUpdaterId(),
                    lossVoltageUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(sharedParameters[SharedParametersFile.Polnaya_Moshchnost_V_SHCHitakh].Id));
                //постоянный ток
                UpdaterRegistry.AddTrigger(lossVoltageUpdater.GetUpdaterId(),
                    lossVoltageUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.RBS_ELEC_TRUE_CURRENT_PARAM)));
                //Тип провода
                UpdaterRegistry.AddTrigger(lossVoltageUpdater.GetUpdaterId(),
                    lossVoltageUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(projectParameters["Марка кабеля"].Id));
                //cos F
                UpdaterRegistry.AddTrigger(lossVoltageUpdater.GetUpdaterId(),
                    lossVoltageUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(sharedParameters[SharedParametersFile.cos_F].Id));
                //Длина
                UpdaterRegistry.AddTrigger(lossVoltageUpdater.GetUpdaterId(),
                    lossVoltageUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.RBS_ELEC_CIRCUIT_LENGTH_PARAM)));
                //Количество пар кабелей в одной группе
                UpdaterRegistry.AddTrigger(lossVoltageUpdater.GetUpdaterId(),
                    lossVoltageUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(projectParameters["Кол-во кабелей (провод) в одной группе"].Id));
                //Полное сопротивление цепи фаза нуль
                UpdaterRegistry.AddTrigger(lossVoltageUpdater.GetUpdaterId(),
                    lossVoltageUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(projectParameters["Полное сопротивление петли фаза-нуль"].Id));
                //Способ расчета потерь напряжения в цепи
                UpdaterRegistry.AddTrigger(lossVoltageUpdater.GetUpdaterId(),
                    lossVoltageUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(projectParameters["Способ расчета потерь напряжения в цепи"].Id));
                //Запретить изменение
                UpdaterRegistry.AddTrigger(lossVoltageUpdater.GetUpdaterId(),
                    lossVoltageUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(sharedParameters[SharedParametersFile.Zapretit_Izmenenie].Id));
                #endregion

                #region Наименование нагрузки
                var loadNameUpdater = new LoadName(uicApp.ActiveAddInId);
                loadNameUpdater.RegisterUpdater(doc);
                //UpdaterRegistry.RegisterUpdater(loadNameUpdater,  doc,true);
                UpdaterRegistry.AddTrigger(loadNameUpdater.GetUpdaterId(),
                    loadNameUpdater.ElementFilter,
                    Element.GetChangeTypeElementAddition());
                //Системный параметр "Классификация нагрузок"
                UpdaterRegistry.AddTrigger(loadNameUpdater.GetUpdaterId(),
                    loadNameUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.CIRCUIT_LOAD_CLASSIFICATION_PARAM)));

                //Системный параметр "Количество элементов"
                UpdaterRegistry.AddTrigger(loadNameUpdater.GetUpdaterId(),
                    loadNameUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER_OF_ELEMENTS_PARAM)));

                //Системный параметр "Длина"
                UpdaterRegistry.AddTrigger(loadNameUpdater.GetUpdaterId(),
                    loadNameUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.RBS_ELEC_CIRCUIT_LENGTH_PARAM)));

                //Системный параметр "Полная мощность"
                UpdaterRegistry.AddTrigger(loadNameUpdater.GetUpdaterId(),
                    loadNameUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.RBS_ELEC_APPARENT_LOAD)));

                //Параметр Резервная группа
                UpdaterRegistry.AddTrigger(loadNameUpdater.GetUpdaterId(),
                    loadNameUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(sharedParameters[SharedParametersFile.Rezervnaya_Gruppa].Id));
                //Параметр Контрольные цепи
                UpdaterRegistry.AddTrigger(loadNameUpdater.GetUpdaterId(),
                    loadNameUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(sharedParameters[SharedParametersFile.Kontrolnye_TSepi].Id));
                //Запретить изменение наименования нагрузки
                UpdaterRegistry.AddTrigger(loadNameUpdater.GetUpdaterId(),
                    loadNameUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(sharedParameters[SharedParametersFile.Zapretit_Izmenenie_Naimenovaniya_Nagruzki].Id));

                #endregion
                ////Регистрация изменений для новых цепей. Установка значения bool-параметров в false
                var booleanParameterUpdaterForAddedElements = new UpdateAddedElectricalSystems(uicApp.ActiveAddInId);
                booleanParameterUpdaterForAddedElements.RegisterUpdater(doc);
                //UpdaterRegistry.RegisterUpdater(booleanParameterUpdaterForAddedElements,  doc,true);
                UpdaterRegistry.AddTrigger(booleanParameterUpdaterForAddedElements.GetUpdaterId(),
                    booleanParameterUpdaterForAddedElements.ElementFilter,
                    Element.GetChangeTypeElementAddition());
                #region Обновление длины цепи
                //Регистрация изменений Обновление длин кабелей
                var electricalSystemLengthUpdater = new LengthOfElectricalSystem(uicApp.ActiveAddInId);
                electricalSystemLengthUpdater.RegisterUpdater(doc);
                //UpdaterRegistry.RegisterUpdater(electricalSystemLengthUpdater,  doc,true);
                //Триггер на изменение параметра Длина для электрической цепи
                UpdaterRegistry.AddTrigger(electricalSystemLengthUpdater.GetUpdaterId(),
                    electricalSystemLengthUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.RBS_ELEC_CIRCUIT_LENGTH_PARAM)));
                //Триггер на изменение параметра "Способ расчета длины электрической цепи"
                UpdaterRegistry.AddTrigger(electricalSystemLengthUpdater.GetUpdaterId(),
                    electricalSystemLengthUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(projectParameters["Способ расчета длины"].Id));
                //Обновить длину при изменение параметра Запретить изменение
                UpdaterRegistry.AddTrigger(electricalSystemLengthUpdater.GetUpdaterId(),
                    electricalSystemLengthUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(sharedParameters[SharedParametersFile.Zapretit_Izmenenie].Id));
                //Параметр Смещение электрической цепи
                UpdaterRegistry.AddTrigger(electricalSystemLengthUpdater.GetUpdaterId(),
                    electricalSystemLengthUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(sharedParameters[SharedParametersFile.Smeshchenie_Dlya_Elektricheskoy_TSepi].Id));
                //Параметр Резервная группа
                UpdaterRegistry.AddTrigger(electricalSystemLengthUpdater.GetUpdaterId(),
                    electricalSystemLengthUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(sharedParameters[SharedParametersFile.Rezervnaya_Gruppa].Id)
                    );
                //Параметр Контрольные цепи
                UpdaterRegistry.AddTrigger(electricalSystemLengthUpdater.GetUpdaterId(),
                    electricalSystemLengthUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(sharedParameters[SharedParametersFile.Kontrolnye_TSepi].Id)
                );
                //Установка приоритетов
                UpdaterRegistry.SetExecutionOrder(electricalSystemLengthUpdater.GetUpdaterId(), lossVoltageUpdater.GetUpdaterId());

                #endregion
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\n" + exception.StackTrace);
            }
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            

            return Result.Succeeded;
        }

        public class ProjectParameterNameComparer : EqualityComparer<ParameterElement>
        {
            public override bool Equals(ParameterElement x, ParameterElement y)
            {
                if (x is null || y is null)
                    return false;
                return x.Name == y.Name;
            }

            public override int GetHashCode(ParameterElement obj)
            {
                return obj.Name.GetHashCode();
            }
        }
    }
}
