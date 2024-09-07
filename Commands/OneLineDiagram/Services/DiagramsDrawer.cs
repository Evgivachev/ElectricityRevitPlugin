namespace Diagrams.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using Abstractions;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;
    using Autodesk.Revit.UI;
    using CommonUtils.Extensions;
    using ExternalCommands.OneLineDiagram;
    using Models;
    using View = Autodesk.Revit.DB.View;

    public class DiagramsDrawer : IDiagramsDrawer
    {
        private readonly UIApplication _uiApplication;

        public DiagramsDrawer(UIApplication uiApplication)
        {
            _uiApplication = uiApplication;
        }

        public void DrawDiagram(Shield shield)
        {
            try
            {
                var doc = _uiApplication.ActiveUIDocument.Document;
                using var tr = new TransactionGroup(doc);
                if (TransactionStatus.Started == tr.Start($"Схема {shield.Name}"))
                {
                    //Создание нового вида
                    var viewDrafting = CreateViewDrafting(shield.Name);
                    //рисование шапки
                    var headFamilyInstance = DrawHead(shield, viewDrafting);
                    //Рисование отходящих линий
                    DrawLines(shield, viewDrafting);
                    tr.Commit();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void SetParametersToHead(FamilyInstance familyInstanceHead, Shield shield)
        {
            var shieldFi = shield.Object;
            ElectricalSystem electricalSystem = null;
            var prefix = shieldFi.LookupParameter($"Префикс цепи").AsString();
            var electricalSystems = shieldFi.MEPModel?.GetElectricalSystems();
            if (electricalSystems != null)
                foreach (var system in electricalSystems)
                {
                    if (!system.Name.StartsWith(prefix))
                    {
                        electricalSystem = system;
                        break;
                    }
                }

            if (electricalSystem is null)
                Console.WriteLine($@"Не найден питающий кабель щита {shieldFi.Name}");

            //Установка параметров
            var parametersDict = new Dictionary<string, Func<ElectricalSystem, dynamic>>()
            {
                #region Текст

                ["ID связанного элемента"] = (line) => $@"{line?.UniqueId}
{shieldFi.UniqueId}",
                //"1QS для РТ" String
                //["1QS для РТ"]=
                ["ID электрического щита"] = powerCable => shieldFi.UniqueId,
                //"Кабель к медной шине" String
                //["Кабель к медной шине"]

                //"Классификация вводных устройств" Integer
                ["Классификация вводных устройств"] = powerCable =>
                {
                    //"Классификация вводных устройств" Integer
                    var value = shieldFi.LookupParameter("Классификация вводных устройств")?.AsInteger();
                    return value == 0 ? 3 : value;
                },
                //"Колво отключаемых полюсов" Double
                ["Колво отключаемых полюсов"] = powerCable => shieldFi.LookupParameter("Кол-во отключаемых полюсов")?.AsDouble(),
                //"Номер 0" String
                ["Номер 0"] = powerCable => { return "QF0"; },
                //"Номер группы в щитах Revit" String
                ["Номер группы в щитах Revit"] = powerCable => { return powerCable?.Name; },
                //"Номинальный ток вводного устройства" Double
                ["Номинальный ток вводного устройства"] =
                    powerCable => shieldFi.LookupParameter("Номинальный ток вводного устройства")?.AsDouble(),
                //"Панель" String
                ["Панель"] = powerCable => { return shieldFi.Name; },
                //"Тип вводного автомата" String
                ["Тип вводного автомата"] = powerCable => shieldFi.LookupParameter("Тип вводного автомата")?.AsString(),
                //"Уставка вводного устроуства" Double
                ["Уставка вводного устроуства"] = powerCable => shieldFi.LookupParameter("Уставка вводного устроуства")?.AsDouble(),

                #endregion

                #region Электросети нагрузки

                //"Коэффициент спроса в щитах" Double
                ["Коэффициент спроса в щитах"] = powerCable => shieldFi.LookupParameter("Коэффициент спроса в щитах")?.AsDouble(),
                //"Перекос фаз" Double
                ["Перекос фаз"] = powerCable => shieldFi.LookupParameter("Перекос фаз")?.AsDouble() * 100,
                //"Суммарный ток L1" Double
                ["Суммарный ток L1"] = powerCable => shieldFi.LookupParameter("Суммарный ток L1")?.AsDouble(),
                //"Суммарный ток L2" Double
                ["Суммарный ток L2"] = powerCable => shieldFi.LookupParameter("Суммарный ток L2")?.AsDouble(),
                //"Суммарный ток L3" Double
                ["Суммарный ток L3"] = powerCable => shieldFi.LookupParameter("Суммарный ток L3")?.AsDouble(),
                //"Установленная мощность в щитах" Double
                ["Установленная мощность в щитах"] = powerCable => shieldFi.LookupParameter("Установленная мощность в щитах")?.AsDouble(),
                //"Ток в щитах" Double
                ["Ток в щитах"] = powerCable => shieldFi.LookupParameter("Ток в щитах")?.AsDouble(),
                //"Полная мощность в щитах" Double
                ["Полная мощность в щитах"] = powerCable => shieldFi.LookupParameter("Полная мощность в щитах")?.AsDouble(),
                //"Косинус в щитах" Double
                ["Косинус в щитах"] = powerCable => shieldFi.LookupParameter("Косинус в щитах").AsDouble(),
                //"Активная мощность в щитах" Double
                ["Активная мощность в щитах"] = powerCable => shieldFi.LookupParameter("Активная мощность в щитах")?.AsDouble(),

                #endregion

                #region Размеры

                //"Магистраль" Double
                // ["Магистраль"] = "",

                #endregion

                #region Результаты анализа

                //"Длина линии в щитах" Double
                ["Длина линии в щитах"] = powerCable =>
                {
                    var value = powerCable?.get_Parameter(new Guid("387ba243-768e-45cf-9c22-ce1b5650fe3d"))?.AsDouble();
                    if (!value.HasValue)
                        return string.Empty;
                    value = UnitUtils.ConvertFromInternalUnits(value.Value, UnitTypeId.Meters);
                    return value.Value.ToString("F2");
                },
                //"Количество кабелей в щитах" String
                ["Количество кабелей в щитах"] =
                    powerCable => powerCable?.LookupParameter("Кол-во кабелей (провод) в одной группе")?.AsDouble(),
                //"Марка кабеля в щитах" String
                ["Марка кабеля в щитах"] = powerCable =>
                {
                    var value = powerCable?.LookupParameter("Марка кабеля").AsValueString() ?? string.Empty;
                    return value;
                },
                //"Номер группы в щитах" String
                ["Номер группы в щитах"] = powerCable => { return powerCable?.LookupParameter("Номер группы по ГОСТ")?.AsString(); },
                //"Питание от в щитах" String
                ["Питание от в щитах"] = powerCable => { return powerCable?.PanelName; },
                //"Потеря напряжения в щитах" String
                ["Потеря напряжения в щитах"] = powerCable =>
                    powerCable?.LookupParameter("Потеря напряжения для ОС")?.AsDouble().ToString("F2"),
                //"Способ прокладки в щитах" String
                ["Способ прокладки в щитах"] = powerCable => powerCable?.LookupParameter("Способ прокладки для схем")?.AsString(),

                #endregion

                #region Видимость

                //"PE" Integer
                ["PE"] = powerCable => { return 1; },
                //"Видимость на схеме перекоса фаз" Integer
                ["Видимость на схеме перекоса фаз"] = powerCable => { return 1; },
                //"Магистраль вид" Integer
                ["Магистраль вид"] = powerCable => { return 0; },
                //"Начало щита" Integer
                ["Начало щита"] = powerCable => { return 1; },
                //"Продолжение щита" Integer
                ["Продолжение щита"] = powerCable => { return 1; },
                //"Шапка видимость" Integer
                ["Шапка видимость"] = powerCable => { return 1; },

                #endregion

                #region Прочее

                //"1" Integer
                //["1"] = "",
                //"Количество групп" Integer
                ["Количество групп"] = powerCable =>
                {
                    return shieldFi
                        .MEPModel?
                        .GetAssignedElectricalSystems()?
                        .Count(x => x.PanelName == shieldFi.Name) + 1;
                },

                #endregion

                #region Другие параметры

                //"Категория" ElementId

                //"Длина линии" Double
                //"РТ прямое" Integer

                //"Питание от Revit" Integer
                //"Семейство и типоразмер" ElementId

                //"Семейство" ElementId

                //"УЗО-4П" Integer

                //"Код типа" ElementId

                //"Имя семейства" String
                //"АВ3П и контакт" Integer
                //"УЗО-2П" Integer
                //"Полная мощность в щитах" Double
                //"4" Double

                //"Длина вывода заземления" Double
                //"Категория" ElementId

                //"Видимость перекоса фаз" Integer
                //"Тип" ElementId

                //"АВ-1П" Integer

                //"Магистраль" Double
                //"ВН для РТ" Integer

                //"Вариант конструкции" ElementId
                //"АВ-3П" Integer
                //"Панель" String
                //"Однофазный щит" Integer
                //"N" Integer
                //"Обычный ввод" Integer
                //"L2" Integer
                //"Код основы" ElementId

                //"Номинальный ток вводного устройства" Double
                //"Объем" Double

                //"Имя типа" String
                //"ID электрического щита" String
                //"Длина для вывода заземления" Double

                //"ВН-1П" Integer

                //"PE вводная" Integer
                //"QF0" Integer

                //"3" Double
                //"УКВК" Integer

                //"Длина ввод от ГРЩ" Double
                //"Уровень" ElementId

                //"ВН-3П" Integer
                //"Площадь" Double

                #endregion
            };

            //Журнал
            //Установка параметров
            var nullParametr = new StringBuilder();
            foreach (var pair in parametersDict)
            {
                var parameter = familyInstanceHead.LookupParameter(pair.Key);
                if (parameter is null)
                {
                    nullParametr.AppendLine($"Параметр \"{pair.Key}\" равен null.");
                    continue;
                }

                var value = pair.Value.Invoke(electricalSystem);
                if (value is null)
                {
                    nullParametr.AppendLine($"Значение \"{pair.Key}\" равно null");
                    continue;
                }

                try
                {
                    var flag = parameter.Set(value);
                    if (!flag)
                        throw new Exception();
                }
                catch (Exception)
                {
                    nullParametr.AppendLine($"Ошибка! Не удалось установить значение \"{parameter.Definition.Name}\"");
                }
            }
        }

        public void DrawLines(Shield shield, View view)
        {
            var shieldFamilyInstance = shield.Object;
            var doc = shieldFamilyInstance.Document;
            var nameOfFamilyLine = "ЭОМ-Схемы однолинейные-Отходящая линия (ГОСТ 2.708-81)";
            var familyLine = new FilteredElementCollector(doc)
                    .OfClass(typeof(Family))
                    .FirstOrDefault(x => x.Name == nameOfFamilyLine)
                as Family;
            if (familyLine == null)
                throw new NullReferenceException($"Не удалось найти семейство \"{nameOfFamilyLine}\"");
            var familySymbolLine = (FamilySymbol)doc?.GetElement(familyLine?.GetFamilySymbolIds().First());

            //Установка параметров
            var numberOfLine = 1;
            var parametersOfLineDict = new Dictionary<string, Func<ElectricalSystem, dynamic>>()
            {
                #region Прочее

                ////1 Integer
                //["1"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////1НЗ Integer
                //["1НЗ"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////1НО Integer
                //["1НО"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////1НО_1НЗ Integer
                //["1НО_1НЗ"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////2 Фазы Integer
                //["2 Фазы"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////2НЗ Integer
                //["2НЗ"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////2НО Integer
                //["2НО"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////3 Фазы Integer
                //["3 Фазы"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////PE Integer
                //["PE"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Telemando Integer
                //["Telemando"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Видимость PE Integer
                //["Видимость PE"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Выбор текста Integer
                //["Выбор текста"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////дополнительная фаза для АВ-2П Integer
                //["дополнительная фаза для АВ-2П"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Имя семейства String
                //["Имя семейства"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Категория ElementId
                //["Категория"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Категория ElementId
                //["Категория"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Код основы ElementId
                //["Код основы"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Код типа ElementId
                //["Код типа"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Нормальные фазы Integer
                //["Нормальные фазы"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Отходящая лини группы Integer
                //["Отходящая лини группы"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Пусто в N Integer
                //["Пусто в N"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},

                //Резерв Integer
                ["Резерв"] = line => line.LookupParameter("Резервная группа")?.AsInteger() ?? 0,

                ////Семейство ElementId
                //["Семейство"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Семейство и типоразмер ElementId
                //["Семейство и типоразмер"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Силовой контактор Integer
                //["Силовой контактор"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////сумма Double
                //["сумма"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Текст в нижнем столбце для групп Integer
                //["Текст в нижнем столбце для групп"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Тип ElementId
                //["Тип"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////УР1 Integer
                //["УР1"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////ур1_АВ Integer
                //["ур1_АВ"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////ур1_АВ-2П Integer
                //["ур1_АВ-2П"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////ур1_ВН Integer
                //["ур1_ВН"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////ур1_ДВ Integer
                //["ур1_ДВ"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////ур1_ниже_пусто Integer
                //["ур1_ниже_пусто"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////ур1_пусто Integer
                //["ур1_пусто"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////УР2 Integer
                //["УР2"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////ур2_Контакт Integer
                //["ур2_Контакт"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////ур2_пусто Integer
                //["ур2_пусто"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////ур2_УЗО Integer
                //["ур2_УЗО"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////УР3 Integer
                //["УР3"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////ур3_Контакт Integer
                //["ур3_Контакт"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////ур3_пусто Integer
                //["ур3_пусто"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////ур3_УЗО Integer
                //["ур3_УЗО"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////УР4 Integer
                //["УР4"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Фазы РТ Integer
                //["Фазы РТ"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                //Цепи управления Integer
                ["Цепи управления"] = line => line.LookupParameter("Контрольные цепи")?.AsInteger() ?? 0,

                #endregion

                #region Текст

                //ID связанного элементаString
                ["ID связанного элемента"] = (line) => { return line.UniqueId; },
                //QF для узо String
                ["QF для узо"] = (line) =>
                {
                    var cl = line.LookupParameter("Классификатор ОУ2")?.AsValueString();
                    if (cl == "2")
                        return $"QSD{numberOfLine}";
                    return "";
                },
                //Группы по ГОСТ String
                ["Группы по ГОСТ"] = (line) => line.LookupParameter("Номер группы по ГОСТ")?.AsString(),
                //Длина Double
                ["Длина"] = line =>
                {
                    ////"Длина кабелей для ОС"	Double
                    var value = line.LookupParameter("Длина кабелей для ОС")?.AsDouble();
                    return value;
                },
                //К щиту ЩОА String
                ["К щиту ЩОА"] = line =>
                {
                    //
                    return null
                        ;
                },
                //кей Double
                ["кей"] = line => 0.0,
                //Кол-во жил Double
                ["Кол-во жил"] = line =>
                {
                    //todo много параметров
                    ////"Кол-во жил"	Double
                    //"Кол-во жил по факту"	String
                    return line.LookupParameter("Кол-во жил")?.AsDouble();
                },
                //Количество пар кабелей String
                ["Количество пар кабелей"] = line =>
                {
                    //"Кол-во кабелей (провод) в одной группе"	Double
                    var value = line.LookupParameter("Кол-во кабелей (провод) в одной группе")?.AsValueString();
                    if (value == "1")
                        return string.Empty;
                    return value;
                },
                //Коэффициент мощности Double
                ["Коэффициент мощности"] = line =>
                {
                    //todo непонятно
                    //"Косинус в щитах"	Double
                    //"Коэффициент мощности"	Double
                    //"cos F"	Double
                    return line.LookupParameter("Коэффициент мощности")?.AsDouble();
                },
                //Марка кабеля String
                ["Марка кабеля"] = line =>
                {
                    //"Тип изоляции"	String
                    return line.LookupParameter("Тип изоляции").AsString() ?? string.Empty;
                },
                //Марка трубы String
                ["Марка трубы"] = line =>
                {
                    //"Способ прокладки для схем"	String
                    return line.LookupParameter("Способ прокладки для схем").AsString() ?? string.Empty;
                },
                //Наименование пустых ячеек String
                //todo
                ["Наименование пустых ячеек"] = line => null,

                //Наименование электроприемника String
                ["Наименование электроприемника"] = line =>
                {
                    //todo есть такой параметр
                    //"Имя нагрузки"	String
                    var types = new HashSet<string>();
                    var spaces = new HashSet<string>();
                    var elements = line
                        .Elements
                        .Cast<FamilyInstance>()
                        .Where(x => x != null);
                    foreach (var familyInstance in elements)
                    {
                        var type = familyInstance
                            .LookupParameter("Классификация нагрузки")?
                            .AsValueString();
                        if (type is null)
                            type = familyInstance?
                                .Symbol?
                                .LookupParameter("Классификация нагрузки")?
                                .AsValueString();
                        if (string.IsNullOrEmpty(type))
                            type = familyInstance
                                .get_Parameter(BuiltInParameter
                                    .RBS_ELEC_PANEL_NAME)?
                                .AsString();

                        if (!string.IsNullOrEmpty(type) && type != "/" && type != "\\" && type != "Соединитель")
                            types.Add(type);

                        var space = familyInstance.Space?
                            .LookupParameter("Номер")?
                            .AsString();
                        if (!string.IsNullOrEmpty(space))
                            spaces.Add(space);
                    }

                    var result = new StringBuilder();
                    result.Append(string.Join(", ", types));
                    if (spaces.Count > 0)
                        result.Append($" пом. ");
                    result.Append(string.Join(", ", spaces.OrderBy(x => x)));
                    var resultStr = result.ToString();
                    return resultStr;
                },
                //Номер контакта КМ String
                ["Номер контакта КМ"] = (line) =>
                {
                    //string
                    var cl = line.LookupParameter("Классификатор ОУ2")?
                        .AsValueString();
                    if (cl == "1")
                        return $"KM{numberOfLine}";
                    return "";
                },
                //Номер контактора String
                ["Номер контактора"] = line => { return null; },
                //Номер контактора 2 String
                ["Номер контактора 2"] = line => null,
                //Номер отключающего устройства в схеме String
                ["Номер отключающего устройства в схеме"] = line => $"QF{numberOfLine}",
                //Номер цепи String
                ["Номер цепи"] = line => line.CircuitNumber,
                //Отключающее устройство String
                ["ОУ1"] = line =>
                {
                    //"ОУ1"	String
                    var result = line.LookupParameter("ОУ1")?.AsString();
                    return result;
                },
                //Отключающее устройство String
                ["Отключающее устройство"] = line =>
                {
                    //"ОУ1"	String
                    var result = line.LookupParameter("ОУ1")?.AsString();
                    return result;
                },
                //потери напряжения, % String
                ["потери напряжения, %"] = line => line.LookupParameter("Потеря напряжения для ОС")?.AsString(),
                //Расчетный ток Double
                ["Расчетный ток"] = line => line.LookupParameter("Ток от полной установленной мощности")?.AsDouble(),
                //Сечение кабеля Double
                ["Сечение кабеля"] = line => line.LookupParameter("Сечение кабеля")?.AsDouble(),
                //УЗО и др. аппарат String
                ["УЗО и др. аппарат"] = line => line.LookupParameter("ОУ2").AsString() ?? string.Empty,
                //Установленная мощность Double
                ["Установленная мощность"] = line => line.LookupParameter("Активная нагрузка")?.AsDouble(),

                #endregion

                //PG_GEOMETRY
                //Объем Double
                //["Объем"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Площадь Double
                //["Площадь"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},

                #region Общие

                //PG_GENERAL
                ["_Классификатор ОУ2"] = line =>
                {
                    //"Классификатор ОУ2"	Double
                    var param = line.LookupParameter("Классификатор ОУ2");
                    if (param is null)
                        throw new NullReferenceException("Классификатор ОУ2 is null");
                    return param.AsDouble();
                },
                //Классификатор ОУ1 Double
                ["Классификатор ОУ1"] = line =>
                {
                    //"Классификатор ОУ1"	Double
                    return line.LookupParameter("Классификатор ОУ1").AsDouble();
                },
                //Классификатор ОУ2 Double
                ["Классификатор ОУ2"] = line =>
                {
                    //"Классификатор ОУ2"	Double
                    var param = line.LookupParameter("Классификатор ОУ2");
                    if (param is null)
                        throw new NullReferenceException("Классификатор ОУ2 is null");
                    return param.AsDouble();
                },
                //Классификатор ОУ3 Double
                ["Классификатор ОУ3"] = line => line.LookupParameter("Классификатор ОУ3")?.AsDouble(),
                //Классификация изображения нагрузки для схем Double
                ["Классификация изображения нагрузки для схем"] =
                    line => line.get_Parameter(new Guid("ffef279b-4e26-4115-a0a0-8045f5824a5b"))?.AsDouble(),
                //Количество полюсов ОУ1 Double
                ["Количество полюсов ОУ1"] = line => line.LookupParameter("Количество полюсов ОУ1")?.AsDouble(),
                //Количество полюсов ОУ2 Double
                ["Количество полюсов ОУ2"] = line => line.LookupParameter("Количество полюсов ОУ2")?.AsDouble(),

                #endregion

                #region Видимость

                ////PG_VISIBILITY
                ////N Integer
                //["N"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////PE проводник Integer
                //["PE проводник"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////В щит ЩОА Integer
                //["В щит ЩОА"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////НР Integer
                //["НР"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Пустые ячейки справа Integer
                //["Пустые ячейки справа"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Свободный разрыв Integer
                //["Свободный разрыв"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},

                #endregion

                //PG_IDENTITY_DATA
                //Вариант конструкции ElementId
                //["Вариант конструкции"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ////Имя типа String
                //["Имя типа"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                //Фаза String
                ["Фаза"] = line => line.LookupParameter("Фаза")?.AsString(),
                //PG_CONSTRAINTS
                //Уровень ElementId
                //["Уровень"] = () =>
                //{
                //    //
                //    return line.LookupParameter("") ?
                //   ;
                //},
                ["Порядковый номер группы"] = line => { return (double)numberOfLine; }
            };
            var nullParameter = new StringBuilder();
            var connectedSystems = shieldFamilyInstance?
                .MEPModel?
                .GetAssignedElectricalSystems()?
                .OrderBy(x => GetGostNameOfCircuit(x).Length)
                .ThenBy(x => GetGostNameOfCircuit(x))
                .ToArray();
            if (connectedSystems == null || !connectedSystems.Any()) return;
            using var tr = new Transaction(doc);
            tr.Start("Рисование отходящих линий");
            foreach (var system in connectedSystems)
            {
                var point = new XYZ(0.283560642716327 + (numberOfLine - 1) * 0.0721784776902887, 0, 0);
                var familyInstanceLine = doc?.Create.NewFamilyInstance(point, familySymbolLine, view);
                if (familyInstanceLine == null)
                    throw new NullReferenceException($"Не удалось найти семейство \"{nameOfFamilyLine}\"");
                doc.Regenerate();
                var familyInstanceLineParameters = familyInstanceLine
                    .Parameters
                    .OfType<Parameter>()
                    .Where(x => x.IsShared && !x.IsReadOnly)
                    .ToDictionary(x => x.GUID);
                var elSystemParameters = system
                    .Parameters.OfType<Parameter>()
                    .Where(x => x.IsShared && !x.IsReadOnly);
                foreach (var elParameter in elSystemParameters)
                {
                    if (!familyInstanceLineParameters.ContainsKey(elParameter.GUID))
                        continue;
                    var lineP = familyInstanceLineParameters[elParameter.GUID];
                    //if (lineP.IsShared || lineP.IsReadOnly)
                    //    continue;
                    var value = elParameter.GetValueDynamic();
                    var isOk = ParameterExtension.SetDynamicValue(lineP, value);
                }

                foreach (var pair in parametersOfLineDict)
                {
                    var parameter = familyInstanceLine.LookupParameter(pair.Key);
                    if (parameter is null)
                    {
                        nullParameter.AppendLine($"{system.Name} \"{pair.Key}\" is null");
                        continue;
                    }

                    if (parameter.IsReadOnly)
                    {
                        nullParameter.AppendLine($"{system.Name} \"{pair.Key}\" is readonly");
                        continue;
                    }

                    var currentValue = parameter.AsValueString() ?? parameter.AsString();
                    var q = parameter.ResetValue();
                    doc.Regenerate();
                    var value = pair.Value.Invoke(system);
                    if (value is null)
                    {
                        nullParameter.AppendLine($"{system.Name} \"{pair.Key}\" value is null");
                        continue;
                    }

                    var valueType = value.GetType();
                    try
                    {
                        var flag = ParameterExtension.SetDynamicValue(parameter, value);
                        if (!flag)
                            nullParameter.AppendLine($"{system.Name} Не удалось установить значение \"{parameter.Definition.Name}\"");
                    }
                    catch (Exception)
                    {
                        nullParameter.AppendLine(
                            $"{system.Name} Ошибка! Не удалось установить значение \"{parameter.Definition.Name}\"");
                    }
                }

                numberOfLine++;
            }

            tr.Commit();
            var q1 = nullParameter.ToString();
            var tempFolder = Path.GetTempPath();
            var name = "RevitDiagram";
            File.WriteAllText(tempFolder + $"\\{name}.txt", q1);
        }


        private static ViewDrafting CreateViewDrafting(string name)
        {
            #region MyRegion

            if (OneLineDiagramBuiltDiagram.CommandData is null)
                throw new NullReferenceException();
            var uiApp = OneLineDiagramBuiltDiagram.CommandData.Application;
            var uiDoc = uiApp?.ActiveUIDocument;
            var app = uiApp?.Application;
            var doc = uiDoc?.Document;

            #endregion

            //все чертёжные виды
            var view = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Views)
                .OfClass(typeof(ViewDrafting))
                .Cast<ViewDrafting>().ToDictionary(x => x.Name);
            var i = 1;
            if (view.ContainsKey(name))
            {
                name += $"({i})";
                i++;
                while (view.ContainsKey(name))
                {
                    var number = i.ToString(CultureInfo.InvariantCulture);
                    var index = name.IndexOf('(');
                    name = name.Substring(0, index + 1) + number + ')';
                    i++;
                }
            }

            var viewFamilyType = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewFamilyType))
                .Cast<ViewFamilyType>().FirstOrDefault(vft => vft.ViewFamily == ViewFamily.Drafting);
            if (viewFamilyType is null)
                throw new NullReferenceException("Нет вида Drafting");

            ViewDrafting currentView = null;
            using (var tr = new Transaction(doc))
            {
                tr.Start($"Создание вида {name}");
                currentView = ViewDrafting.Create(doc, viewFamilyType.Id);
                currentView.LookupParameter("Назначение вида")?.Set("Однолинейные схемы");
                var index1 = name.IndexOfAny(new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
                var subName = name.Substring(0, index1 > 0 ? index1 : name.Length - 1);
                currentView.LookupParameter("Группа вида")?.Set(subName);
                currentView.Name = name;
                currentView.Scale = 1;
                tr.Commit();
            }

            return currentView;
        }

        private FamilyInstance DrawHead(Shield shield, View view)
        {
            #region MyRegion

            if (OneLineDiagramBuiltDiagram.CommandData is null)
                throw new NullReferenceException();
            var uiApp = OneLineDiagramBuiltDiagram.CommandData.Application;
            var uiDoc = uiApp?.ActiveUIDocument;
            var app = uiApp?.Application;
            var doc = uiDoc?.Document;

            #endregion

            var nameOfFamilyOfHead = "ЭОМ-Схемы однолинейные-Шапка (ГОСТ 2.708-81)";
            var familyHead = new FilteredElementCollector(doc)
                    .OfClass(typeof(Family))
                    .FirstOrDefault(x => x.Name == nameOfFamilyOfHead)
                as Family;
            if (familyHead == null)
                throw new NullReferenceException($"Не удалось найти семейство \"{nameOfFamilyOfHead}\"");
            var familySymbolHead = (FamilySymbol)doc?.GetElement(familyHead?.GetFamilySymbolIds().First());
            using (var tr = new Transaction(doc))
            {
                tr.Start($"Вставка семейсва шапки {shield.Name}");
                var familyInstanceHead = doc?.Create.NewFamilyInstance(new XYZ(), familySymbolHead, view);
                if (familyInstanceHead == null)
                    throw new NullReferenceException($"Не удалось найти семейство \"{nameOfFamilyOfHead}\"");
                var sb = new StringBuilder();
                //Вычисление перекола фаз
                //CalculateCurrentImbalance(shield);
                SetParametersToHead(familyInstanceHead, shield);
                tr.Commit();
                return familyInstanceHead;
            }
        }

        private static string GetGostNameOfCircuit(ElectricalSystem es)
        {
            var param = es.LookupParameter("Номер группы по ГОСТ")?.AsString();
            if (string.IsNullOrEmpty(param))
            {
                MessageBox.Show($"Отсутствует номер группы по ГОСТ у цепи {es.CircuitNumber} {es.Id}");
                throw new ArgumentException($"Отсутствует номер группы по ГОСТ у цепи {es.CircuitNumber} {es.Id}");
            }

            return param;
        }
    }
}
