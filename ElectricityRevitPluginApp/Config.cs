namespace ElectricityRevitPluginApp;

using System.Reflection;
using MarkingElectricalSystems;
using RxBim.Application.Ribbon;
using RxBim.Di;

/// <inheritdoc />
public class Config : IApplicationConfiguration
{
    /// <inheritdoc />
    public void Configure(IContainer container)
    {
        var tabName = "ЭОМ";
        container.AddRevitMenu(
            ribbon => ribbon
                .EnableDisplayVersion()
                .Tab(tabName, tabBuilder => tabBuilder
                    .Panel("Цепи", panelBuilder => panelBuilder
                            .CommandButton(
                                nameof(MarkingElectricalSystems),
                                typeof(Cmd),
                                button => button
                                    .Text("Маркировка цепей")
                                    .LargeImage(@"img\icons8-нео-32.png")
                                    .Description(
                                        "Плагин для сгруппированного маркирования цепей по выделенным элементам")
                                    .ToolTip("Маркировка цепей по выделенным элементам"))
                        /*
                        .StackedItems(stackedBuilder => stackedBuilder
                              .CommandButton(
                                  nameof(WallDecoration),
                                  typeof(WallDecoration.Cmd),
                                  button => button
                                      .Text($"Оформление{Environment.NewLine}отделки стен")
                                      .SmallImage(@"img\WallDecoration16.png")
                                      .LargeImage(@"img\WallDecoration32.png")
                                      .ToolTip(
                                          "Позволяет оформить планы отделки стен и полов(плинтус) для раздела АИ")
                                      .Description(
                                          "Плагин анализирует модель, подбирает линии детализации для каждой отделке и формирует контуры.")
                                      .HelpUrl(
                                          "https://tools-help.pik.ru/users/%D0%B8%D0%BD%D1%81%D1%82%D1%80%D1%83%D0%BA%D1%86%D0%B8%D0%B8-%D0%BA-%D0%BF%D0%BB%D0%B0%D0%B3%D0%B8%D0%BD%D0%B0%D0%BC/%D0%B0%D1%80/%D0%BE%D1%84%D0%BE%D1%80%D0%BC%D0%BB%D0%B5%D0%BD%D0%B8%D0%B5-%D0%BE%D1%82%D0%B4%D0%B5%D0%BB%D0%BA%D0%B8-%D1%81%D1%82%D0%B5%D0%BD"))
                              .CommandButton(
                                  nameof(FloorSchedule),
                                  typeof(FloorSchedule.Cmd),
                                  button => button
                                      .Text($"Экспликация{Environment.NewLine}полов")
                                      .SmallImage(@"img\FloorSchedule16.png")
                                      .LargeImage(@"img\FloorSchedule32.png")
                                      .Description(
                                          "Плагин автоматизирует заполнение параметра “Комментарии к типоразмеру” для дальнейшего вывода данных в спецификации")
                                      .ToolTip("Позволяет заполнить номера помещений для каждого типа пола")
                                      .HelpUrl(
                                          "https://tools-help.pik.ru/users/%D0%B8%D0%BD%D1%81%D1%82%D1%80%D1%83%D0%BA%D1%86%D0%B8%D0%B8-%D0%BA-%D0%BF%D0%BB%D0%B0%D0%B3%D0%B8%D0%BD%D0%B0%D0%BC/%D0%B0%D1%80/%D1%8D%D0%BA%D1%81%D0%BF%D0%BB%D0%B8%D0%BA%D0%B0%D1%86%D0%B8%D1%8F-%D0%BF%D0%BE%D0%BB%D0%BE%D0%B2")))
                          */)),
            Assembly.GetExecutingAssembly());
    }
}
