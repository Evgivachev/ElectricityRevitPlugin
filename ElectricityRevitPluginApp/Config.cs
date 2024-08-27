using JetBrains.Annotations;

namespace ElectricityRevitPluginApp;

using System.Reflection;
using RxBim.Application.Ribbon;
using RxBim.Di;
using ShieldPanel.SelectModelOfShield;

/// <inheritdoc />
[UsedImplicitly]
public class Config : IApplicationConfiguration
{
    /// <inheritdoc />
    public void Configure(IContainer container)
    {
        ConfigurePanel(container);
    }

    private void ConfigurePanel(IContainer container)
    {
        const string tabName = "ЭОМ new";
        container.AddRevitMenu(
            ribbon => ribbon
                .EnableDisplayVersion()
                .Tab(tabName, tabBuilder => tabBuilder
                    .Panel("Цепи", panelBuilder => panelBuilder
                        .CommandButton(
                            nameof(GroupByGost.Cmd),
                            typeof(GroupByGost.Cmd),
                            button => button
                                .Text("Группы по ГОСТ")
                                .LargeImage(@"img\icons8.png")
                                .Description(
                                    "Группы по ГОСТ")
                                .ToolTip("Группы по ГОСТ"))
                        .StackedItems(builder => builder.CommandButton(
                                nameof(MarkingElectricalSystems),
                                typeof(MarkingElectricalSystems.Cmd),
                                button => button
                                    .Text("Маркировка цепей")
                                    .LargeImage(@"img\icons8-нео-32.png")
                                    .Description(
                                        "Плагин для сгруппированного маркирования цепей по выделенным элементам")
                                    .ToolTip("Маркировка цепей по выделенным элементам"))
                            .CommandButton(
                                nameof(MarkingElectricalSystems.UpdatingMarkingOfCircuitsExternalCommand),
                                typeof(MarkingElectricalSystems.UpdatingMarkingOfCircuitsExternalCommand),
                                button => button
                                    .Text("Обновление маркировок цепей")
                                    .LargeImage(@"img\icons8-морфеус-32.png")
                                    .Description(
                                        "Плагин для для обновления марок электрических цепей")
                                    .ToolTip("Плагин для для обновления марок электрических цепей")))
                        .CommandButton(
                            nameof(InitialValues),
                            typeof(InitialValues.ExternalCommand),
                            button => button
                                .Text("Начальные значения")
                                .LargeImage(@"img\icons8.png")
                                .Description(
                                    "Плагин для задания начальных значений электрическим цепям")
                                .ToolTip("Плагин для задания начальных значений электрическим цепям"))
                        .CommandButton(
                            nameof(GeneralSubjectDiagram),
                            typeof(GeneralSubjectDiagram.Cmd),
                            button => button
                                .Text("Схема ВРУ")
                                .LargeImage(@"img\icons8-паутина-32.png")
                                .Description(
                                    "Плагин создания схемы ВРУ")
                                .ToolTip("Плагин создания схемы ВРУ"))
                        .CommandButton(
                            nameof(PhaseDistribution),
                            typeof(PhaseDistribution.Cmd),
                            button => button
                                .Text("Распределение по фазам")
                                .LargeImage(@"img\icons8-death-star-32.png")
                                .Description(
                                    "Распределение по фазам отходящих линий в щитах")
                                .ToolTip("Распределение по фазам отходящих линий в щитах"))
                        .CommandButton(
                            nameof(ShortCircuits),
                            typeof(ShortCircuits.Cmd),
                            button => button
                                .Text("Токи кз")
                                .LargeImage(@"img\shortCircuits.png")
                                .Description(
                                    "Плагин для расчета токов короткого замыкания")
                                .ToolTip("Плагин для расчета токов короткого замыкания"))
                    )
                    .Panel("Щиты", builder => builder
                        .CommandButton(
                            nameof(SelectModelOdShieldExternalCommand),
                            typeof(SelectModelOdShieldExternalCommand),
                            button => button
                                .Text("Подбор щитов")
                                .LargeImage(@"img\icons8-супермен-32.png")
                                .Description(
                                    "Плагин подбора корпусов щитового оборудования")
                                .ToolTip("Плагин подбора корпусов щитового оборудования"))
                    )
                    .Panel("Нагрузки", builder =>
                        builder.StackedItems(stack => stack
                            .CommandButton(
                                nameof(ElectricalLoadsExportToExcel),
                                typeof(ElectricalLoadsExportToExcel.ExternalCommand),
                                button => button
                                    .Text("Экспорт в Excel")
                                    .Description("Экспорт электрических нагрузок в таблицу Excel")
                                    .ToolTip("Экспорт электрических нагрузок в таблицу Excel")
                            )
                            .CommandButton(
                                nameof(ElectricalLoadsImportFromExcel),
                                typeof(ElectricalLoadsImportFromExcel.ExternalCommand),
                                button => button
                                    .Text("Импорт из Excel")
                                    .Description("Импорт электрических нагрузок из таблицы Excel")
                                    .ToolTip("Импорт электрических нагрузок из таблицы Excel")
                            ))
                    )
                ),
            Assembly.GetExecutingAssembly());
    }
}
