namespace ElectricityRevitPluginApp;

using System;
using System.IO;
using System.Reflection;
using AddedElectricalSystemsUpdater;
using CountFixturesInSpaceCmd;
using Diagrams.ExternalCommands.OneLineDiagram;
using JetBrains.Annotations;
using MarkingElectricalSystems;
using Microsoft.Extensions.Configuration;
using RxBim.Application.Ribbon;
using RxBim.Di;
using RxBim.Di.Extensions;
using ShieldPanel.SelectModelOfShield;
using Triggers;

/// <inheritdoc />
[UsedImplicitly]
public class Config : IApplicationConfiguration
{
    /// <inheritdoc />
    public void Configure(IContainer container)
    {
        // var buider = new ConfigurationBuilder();
        // buider.AddJsonFile(GetJsonPath("appsettings.json"), optional: false, reloadOnChange: true);
        // buider.AddJsonFile(GetJsonPath("appsettings.AddedElectricalSystemsUpdater.json"), optional: false, reloadOnChange: true);
        // var jsonConfiguration = buider.Build();
        
        ConfigurePanel(container);
        container.Services.AddElectricalSystemsUpdater();
        container.AddScoped<IUpdaterTrigger, OnCreatedElectricalSystemTrigger>();
        container.AddSingleton<Action<IContainer, IConfigurationBuilder>>((c , b) => b.AddJsonFile("appsettings.AddedElectricalSystemsUpdater.json", optional: false, reloadOnChange: true));
    }
    
    private IConfigurationBuilder GetBaseConfigurationBuilder(Assembly assembly)
    {
        var basePath = Path.GetDirectoryName(assembly.Location) 
                       ?? throw new InvalidOperationException("Can't find directory for assembly '" + assembly.FullName + "'!");
        var str = "appsettings." + assembly.GetName().Name + ".json";
        return new ConfigurationBuilder()
            .SetBasePath(basePath)
            .SetFileLoadExceptionHandler((Action<FileLoadExceptionContext>) (ctx => ctx.Ignore = true))
            .AddJsonFile(str, true)
            .AddEnvironmentJsonFile(basePath, str);
    }

    private string GetJsonPath(string json)
    {
        return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, json);
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
                            nameof(GroupByGost),
                            typeof(GroupByGost.Cmd),
                            button => button
                                .Text("Группы по ГОСТ")
                                .LargeImage(@"img\icons8.png")
                                .Description(
                                    "Группы по ГОСТ")
                                .ToolTip("Группы по ГОСТ"))
                        .StackedItems(builder => builder
                            .CommandButton(
                                nameof(MarkingElectricalSystems),
                                typeof(MarkingElectricalSystems.Cmd),
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
                                    .ToolTip("Плагин для для обновления марок электрических цепей"))
                            .CommandButton(
                                nameof(CableJournalCmd.Cmd),
                                typeof(CableJournalCmd.Cmd),
                                button => button
                                    .Text("Кабельный журнал")
                                    .Description(
                                        "Кабельный журнал")
                                    .ToolTip("Кабельный журнал"))
                        )
                        .Separator()
                        .StackedItems(builder => builder.CommandButton(
                                nameof(CountFixturesInSpaceCmd),
                                typeof(CountFixturesInSpaceCmd.Cmd),
                                button => button
                                    .Text("Подсчет светильников в помещениях"))
                            .CommandButton(
                                nameof(CountFixturesInSpaceCmd) + "2",
                                typeof(Cmd2),
                                button => button
                                    .Text("Подсчет светильников в помещениях 2"))
                        )
                        .Separator()
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
                    .Panel("Схемы", builder => builder
                        .StackedItems(sb => sb
                            .CommandButton<OneLineDiagramBuiltDiagram>(nameof(OneLineDiagramBuiltDiagram),
                                bt => bt
                                    .Text("Создать схему")
                                    .Description("Создание однолинейных схем щитов"))
                            .CommandButton<OneLineDiagramUpdateDiagram>(nameof(OneLineDiagramUpdateDiagram),
                                bt => bt.Text("Обновить схему")
                                    .Description("Обновить однолинейные схемы щитов"))
                        )
                        .CommandButton(
                            nameof(GeneralSubjectDiagram),
                            typeof(GeneralSubjectDiagram.Cmd),
                            button => button
                                .Text("Схема ВРУ")
                                .LargeImage(@"img\icons8-паутина-32.png")
                                .Description(
                                    "Плагин создания схемы ВРУ")
                                .ToolTip("Плагин создания схемы ВРУ"))
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
