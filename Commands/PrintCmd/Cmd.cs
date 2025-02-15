namespace PrintCmd;

using System.Reflection;
using Autodesk.Revit.Attributes;
using CommonUtils;
using ElectricityRevitPlugin.UI;
using ElectricityRevitPlugin.UI.Services;
using Infrastructure;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using MoreLinq;
using Print.Application.Application;
using Print.View.View;
using Print.View.ViewModel;

[Transaction(TransactionMode.Manual)]
[UsedImplicitly]
public class Cmd : WpfCmd<PrintAndExportView>
{
    /// <inheritdoc />
    protected override void ConfigureServices(IServiceCollection services)
    {
        Assembly.Load(typeof(MoreEnumerable).Assembly.Location);
        services.AddSingleton<IUIDispatcher, UiDispatcher>();

        services.AddSingleton<ITransactionsService, TransactionsService>();

        services.AddSingleton<PrintAndExportView>()
            .AddSingleton<BaseViewModel>()
            .AddSingleton<ExportViewModel>()
            .AddSingleton<PrintViewModel>();

        services.AddSingleton<IPrintService, PrintService>();
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<ISheetsRepository, SheetRepository>();
    }
}
