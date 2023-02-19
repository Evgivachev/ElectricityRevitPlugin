namespace ElectricityRevitPluginApp;

using System.Reflection;
using MarkingElectricalSystems;
using RxBim.Application.Ribbon;
using RxBim.Di;
using ShieldPanel.SelectModelOfShield;

/// <inheritdoc />
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
                                typeof(Cmd),
                                button => button
                                    .Text("Маркировка цепей")
                                    .LargeImage(@"img\icons8-нео-32.png")
                                    .Description(
                                        "Плагин для сгруппированного маркирования цепей по выделенным элементам")
                                    .ToolTip("Маркировка цепей по выделенным элементам"))
                            .CommandButton(
                                nameof(UpdatingMarkingOfCircuitsExternalCommand),
                                typeof(UpdatingMarkingOfCircuitsExternalCommand),
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
                    )),
            Assembly.GetExecutingAssembly());
    }
}
