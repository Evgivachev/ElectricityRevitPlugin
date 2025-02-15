namespace Print.View.Test;

using Application.Application;
using ElectricityRevitPlugin.UI;
using ElectricityRevitPlugin.UI.Services;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using View;
using ViewModel;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();
        Configure(services);
        var provider = services.BuildServiceProvider();
        var view = provider.GetService<PrintAndExportView>();
        view.ShowDialog();
    }

    private static void Configure(IServiceCollection services)
    {
        services.AddSingleton<IUIDispatcher, UiDispatcher>();
        services.AddSingleton<PrintViewModel>();
        services.AddSingleton<ExportViewModel>();
        services.AddSingleton<BaseViewModel>();
        services.AddSingleton<PrintAndExportView>();
        services.AddSingleton<IPrintService, PrintService>();
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<ISheetsRepository, SheetsRepository>();
        services.AddSingleton<BaseViewModel>();
    }
}
