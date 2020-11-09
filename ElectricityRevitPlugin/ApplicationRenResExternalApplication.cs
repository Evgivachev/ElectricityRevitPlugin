using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ElectricityRevitPlugin.GroupByGost;
using ElectricityRevitPlugin.UpdateParametersInCircuits;
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
                            .SetLongDescription<MyButton>($"Обновление параметров \"Максимальный ток ОУ на группах в щитах\" и \"Количество модулей в щитах\"")
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
        private void RegisterUpdaters(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs e)
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
                UpdaterRegistry.RegisterUpdater(loadNameUpdater, true);
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
                #endregion
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\n" + exception.StackTrace);
                var uicApp = sender as UIControlledApplication;
                uicApp.ControlledApplication.DocumentOpened -= RegisterUpdaters;
            }
           
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            var electricalSystemLengthUpdater = new UpdateLengthOfElectricalSystemsDynamicModelUpdater(application.ActiveAddInId);
            UpdaterRegistry.UnregisterUpdater(electricalSystemLengthUpdater.GetUpdaterId());
            return Result.Succeeded;
        }

        public class ProjectParameterNameComparer : EqualityComparer<ParameterElement>
        {
            public override bool Equals(ParameterElement x, ParameterElement y)
            {
                if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
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
