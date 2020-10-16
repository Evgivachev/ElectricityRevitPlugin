using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ElectricityRevitPlugin.GroupByGost;
using ElectricityRevitPlugin.UpdateParametersInCircuits;
using UpdateNameSpace;
using VCRevitRibbonUtilCustom;

namespace ElectricityRevitPlugin
{
    class ApplicationRenResExternalApplication : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication uicApp)
        {
            var result = Result.Succeeded;
            try
            {
                //MyRibbon.GetApplicationRibbon(uicApp)
                //    .Tab("ЭОМ")
                //    .Panel("Листы")
                //    .CreateSplitButton("SB Листы1Old", " SB Листы1Old",
                //        sb =>
                //        {
                //            sb.CreateButton<SortSheets>("Сортировка листовOld", "Сортировка листов",
                //                bt =>
                //                    bt.SetLargeImage(Resource1.icons8_futurama_professor_farnsworth_32)
                //                        .SetLongDescription<MyButton>("Сортировка листов")
                //                        .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                //                            "https://www.revitapidocs.com/2019/"));
                //            sb.CreateButton<SelectFramesFromSelectedSheets>("Выбрать рамкиOld", "Выбрать рамки",
                //                bt => bt.SetLargeImage(Resource1.icons8_futurama_fry_32)
                //                    .SetLongDescription<MyButton>("Выбрать семейства основной надписи на листах")
                //                    .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                //                        "https://www.revitapidocs.com/2019/"));

                //        })
                //    .CreateSplitButton("SB Листы2Old", " SB Листы2Old",
                //        sb =>
                //        {
                //            sb.CreateButton<SelectSheetFormatAndAlignExternalCommand>("Подобрать рамкиOld", "Подобрать рамки",
                //                bt =>
                //                    bt.SetLargeImage(Resource1.icons8_futurama_bender_32)
                //                        .SetLongDescription<MyButton>("Подобрать рамки")
                //                        .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                //                            "https://www.revitapidocs.com/2019/"));
                //            sb.CreateButton<SelectSheetFormatAndAlignOnDiagramExternalCommand>("Подобрать рамки для схемOld", "Подобрать рамки для схем",
                //                bt => bt.SetLargeImage(Resource1.icons8_futurama_zoidberg_32)
                //                    .SetLongDescription<MyButton>("Подобрать рамки для схем")
                //                    .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                //                        "https://www.revitapidocs.com/2019/"));
                //        })
                //    ;

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
                            .SetLongDescription<MyButton>($"Обновление параметров \"Максимальный ток ОУ на группах в щитах\" и \"Количество модулей в щитах\"")
                            .SetLargeImage(Resource1.icons8_house_stark_32);
                    });



                ////Регистрация изменений для новых цепей. Изменение траектории цепи на все элементы
                //var modeOfElectricalSystem = new SetModeOfElectricalSystemToAllElementsDynamicModelUpdater(uicApp.ActiveAddInId);
                //UpdaterRegistry.RegisterUpdater(modeOfElectricalSystem, true);
                //UpdaterRegistry.AddTrigger(modeOfElectricalSystem.GetUpdaterId(),
                //    modeOfElectricalSystem.GetElementFilter(),
                //    Element.GetChangeTypeElementAddition());

                //////Регистрация изменений для новых цепей. Установка значения Запретить изменение в false
                ////var isUnEditableDynamicModelUpdater = new UnEnableEditionSetFalseForAddedSystemsDynamicModelUpdater(uicApp.ActiveAddInId);
                ////UpdaterRegistry.RegisterUpdater(isUnEditableDynamicModelUpdater, true);
                ////UpdaterRegistry.AddTrigger(isUnEditableDynamicModelUpdater.GetUpdaterId(),
                ////    isUnEditableDynamicModelUpdater.GetElementFilter(),
                ////    Element.GetChangeTypeElementAddition());

                //////Регистрация изменений для новых цепей. Установка значения Резервная группа Контрольные цепи в false
                ////var isReserveCircuitDynamicModelUpdater = new ReserveAndControlCircuitsSetFalseForAddedSystemsDynamicModelUpdater(uicApp.ActiveAddInId);
                ////UpdaterRegistry.RegisterUpdater(isReserveCircuitDynamicModelUpdater, true);
                ////UpdaterRegistry.AddTrigger(isReserveCircuitDynamicModelUpdater.GetUpdaterId(),
                ////    isReserveCircuitDynamicModelUpdater.GetElementFilter(),
                ////    Element.GetChangeTypeElementAddition());

                ////Регистрация изменений для новых цепей. Установка значения bool-параметров в false
                //var booleanParameterUpdaterForAddedElements = new SetToFalseBooleanParametersForAddedElementsDynamicModelUpdater(uicApp.ActiveAddInId);
                //UpdaterRegistry.RegisterUpdater(booleanParameterUpdaterForAddedElements, true);
                //UpdaterRegistry.AddTrigger(booleanParameterUpdaterForAddedElements.GetUpdaterId(),
                //    booleanParameterUpdaterForAddedElements.GetElementFilter(),
                //    Element.GetChangeTypeElementAddition());

                ////Регистрация изменений Обновление длин кабелей
                //var electricalSystemLengthUpdater = new UpdateLengthOfElectricalSystemsDynamicModelUpdater(uicApp.ActiveAddInId);
                //UpdaterRegistry.RegisterUpdater(electricalSystemLengthUpdater, true);
                ////UpdaterRegistry.AddTrigger(electricalSystemLengthUpdater.GetUpdaterId(),
                ////    electricalSystemLengthUpdater.GetElementFilter(),
                ////    Element.GetChangeTypeAny());

                ////Триггер на изменение параметра Длина для электрической цепи
                //UpdaterRegistry.AddTrigger(electricalSystemLengthUpdater.GetUpdaterId(),
                //    electricalSystemLengthUpdater.GetElementFilter(),
                //    Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.RBS_ELEC_CIRCUIT_LENGTH_PARAM)));
                ////Триггер на изменение параметра "Способ расчета длины электрической цепи"
                ////var calculateLengthType = el.LookupParameter("Способ расчета длины").AsValueString();
                //UpdaterRegistry.AddTrigger(electricalSystemLengthUpdater.GetUpdaterId(),
                //    electricalSystemLengthUpdater.GetElementFilter(),
                //    Element.GetChangeTypeParameter(new ElementId(25296192)));

                ////Обновить длину при изменение параметра Запретить изменение
                //UpdaterRegistry.AddTrigger(electricalSystemLengthUpdater.GetUpdaterId(),
                //    electricalSystemLengthUpdater.GetElementFilter(),
                //    Element.GetChangeTypeParameter(new ElementId(24969484)));
                ////Параметр Смещение электрической цепи
                //UpdaterRegistry.AddTrigger(electricalSystemLengthUpdater.GetUpdaterId(),
                //    electricalSystemLengthUpdater.GetElementFilter(),
                //    Element.GetChangeTypeParameter(new ElementId(25296204)));

                ////Параметр Наименование нагрузки
                ////Регистрация изменений Обновление длин кабелей
                //var electricalSystemLoadNameUpdater = new SetLoadNameForElectricalSystemsDynamicModelUpdater(uicApp.ActiveAddInId);
                //UpdaterRegistry.RegisterUpdater(electricalSystemLoadNameUpdater, true);
                //UpdaterRegistry.AddTrigger(electricalSystemLoadNameUpdater.GetUpdaterId(),
                //    electricalSystemLoadNameUpdater.GetElementFilter(),
                //    Element.GetChangeTypeElementAddition());
                ////Системный параметр "Классификация нагрузок"
                //UpdaterRegistry.AddTrigger(electricalSystemLoadNameUpdater.GetUpdaterId(),
                //    electricalSystemLoadNameUpdater.GetElementFilter(),
                //    Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.CIRCUIT_LOAD_CLASSIFICATION_PARAM)));

                ////Системный параметр "Количество элементов"
                //UpdaterRegistry.AddTrigger(electricalSystemLoadNameUpdater.GetUpdaterId(),
                //    electricalSystemLoadNameUpdater.GetElementFilter(),
                //    Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER_OF_ELEMENTS_PARAM)));

                ////Системный параметр "Длина"
                //UpdaterRegistry.AddTrigger(electricalSystemLoadNameUpdater.GetUpdaterId(),
                //    electricalSystemLoadNameUpdater.GetElementFilter(),
                //    Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.RBS_ELEC_CIRCUIT_LENGTH_PARAM)));

                ////Системный параметр "Полная мощность"
                //UpdaterRegistry.AddTrigger(electricalSystemLoadNameUpdater.GetUpdaterId(),
                //    electricalSystemLoadNameUpdater.GetElementFilter(),
                //    Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.RBS_ELEC_APPARENT_LOAD)));


                //Регистрация изменений Обновление номера группы по ГОСТ


                //При отсоединении элемента или добавлении триггер не срабатывает, поэтому GetChanngeTypeAny

                //var groupNumberByGostUpdater = new GroupByGostDynamicUpdater(uicApp.ActiveAddInId);
                //UpdaterRegistry.RegisterUpdater(groupNumberByGostUpdater, true);

                //UpdaterRegistry.AddTrigger(groupNumberByGostUpdater.GetUpdaterId(),
                //    groupNumberByGostUpdater.GetElementFilter(),
                //    Element.GetChangeTypeAny());







                ////Обновление потерь напряжения в цепях
                //var lossVoltageUpdater = new UpdateLossVoltageOfElectricalCircuitsDynamicModelUpdater(uicApp.ActiveAddInId);
                //UpdaterRegistry.RegisterUpdater(lossVoltageUpdater, true);
                //UpdaterRegistry.AddTrigger(lossVoltageUpdater.GetUpdaterId(),
                //    lossVoltageUpdater.GetElementFilter(),
                //    Element.GetChangeTypeAny());

                //TODO обновление групп по ГОСТ
                var groupByGostUpdater = new Updaters.GroupByGost(uicApp.ActiveAddInId);
                UpdaterRegistry.RegisterUpdater(groupByGostUpdater, true);
                UpdaterRegistry.AddTrigger(groupByGostUpdater.GetUpdaterId(),
                    groupByGostUpdater.ElementFilter,
                    Element.GetChangeTypeAny());
                //TODO Потери напряжения
                var lossVoltageUpdater = new Updaters.LossVoltage(uicApp.ActiveAddInId);
                UpdaterRegistry.RegisterUpdater(lossVoltageUpdater, true);
                UpdaterRegistry.AddTrigger(lossVoltageUpdater.GetUpdaterId(),
                    lossVoltageUpdater.ElementFilter,
                    Element.GetChangeTypeAny());

                

                #region Наименование нагрузки



                var loadNameUpdater = new Updaters.LoadName(uicApp.ActiveAddInId);
                UpdaterRegistry.RegisterUpdater(loadNameUpdater,true);
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
                    Element.GetChangeTypeParameter(new ElementId(25928440)));
                //Параметр Контрольные цепи
                UpdaterRegistry.AddTrigger(loadNameUpdater.GetUpdaterId(),
                    loadNameUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(new ElementId(25928439)));
                #endregion

                ////Регистрация изменений для новых цепей. Установка значения bool-параметров в false
                var booleanParameterUpdaterForAddedElements = new Updaters.UpdateAddedElectricalSystems(uicApp.ActiveAddInId);
                UpdaterRegistry.RegisterUpdater(booleanParameterUpdaterForAddedElements, true);
                UpdaterRegistry.AddTrigger(booleanParameterUpdaterForAddedElements.GetUpdaterId(),
                    booleanParameterUpdaterForAddedElements.ElementFilter,
                    Element.GetChangeTypeElementAddition());

                #region Обновление длины цепи

                

                //Регистрация изменений Обновление длин кабелей
                var electricalSystemLengthUpdater = new Updaters.LengthOfElectricalSystem(uicApp.ActiveAddInId);
                UpdaterRegistry.RegisterUpdater(electricalSystemLengthUpdater, true);
                //Триггер на изменение параметра Длина для электрической цепи
                UpdaterRegistry.AddTrigger(electricalSystemLengthUpdater.GetUpdaterId(),
                    electricalSystemLengthUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.RBS_ELEC_CIRCUIT_LENGTH_PARAM)));
                //Триггер на изменение параметра "Способ расчета длины электрической цепи"
                UpdaterRegistry.AddTrigger(electricalSystemLengthUpdater.GetUpdaterId(),
                    electricalSystemLengthUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(new ElementId(25296192)));
                //Обновить длину при изменение параметра Запретить изменение
                UpdaterRegistry.AddTrigger(electricalSystemLengthUpdater.GetUpdaterId(),
                    electricalSystemLengthUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(new ElementId(24969484)));
                //Параметр Смещение электрической цепи
                UpdaterRegistry.AddTrigger(electricalSystemLengthUpdater.GetUpdaterId(),
                    electricalSystemLengthUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(new ElementId(25296204)));
                //Параметр Резервная группа
                UpdaterRegistry.AddTrigger(electricalSystemLengthUpdater.GetUpdaterId(),
                    electricalSystemLengthUpdater.ElementFilter,
                    Element.GetChangeTypeParameter(new ElementId(25928440))
                    );
                #endregion

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
            var electricalSystemLengthUpdater = new UpdateLengthOfElectricalSystemsDynamicModelUpdater(application.ActiveAddInId);
            UpdaterRegistry.UnregisterUpdater(electricalSystemLengthUpdater.GetUpdaterId());
            return Result.Succeeded;
        }
    }
}
