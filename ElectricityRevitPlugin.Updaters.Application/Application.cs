namespace ElectricityRevitPlugin.Updaters.Application;

using System.Diagnostics;
using System.Reflection;
using AddedElectricalSystemsUpdater;
using Autodesk.Revit.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RibbonBuilder;
using Triggers;

public class Application : IExternalApplication
{
    public Result OnStartup(UIControlledApplication application)
    {
        try
        {
            Debugger.Launch();
            var hostBuilder = new HostBuilder();
            hostBuilder.ConfigureHostConfiguration(c => { });

            hostBuilder.ConfigureAppConfiguration((context, builder) =>
            {
                builder.SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                builder.AddJsonFile("appsettings.AddedElectricalSystemsUpdater.json", false);
            });
            hostBuilder.ConfigureServices((context, collection) =>
            {
                ConfigureBaseDependencies(collection, application);
                ConfigureServices(collection, context.Configuration);
            });
            using var host = hostBuilder.Build();

            var sbs = host.Services.GetRequiredService<IEnumerable<ISyncBackGroundService>>();
            foreach (var syncBackGroundService in sbs)
            {
                syncBackGroundService.Execute();
            }

            return Result.Succeeded;
        }
        catch (Exception e)
        {
            return Result.Failed;
        }
    }
    public Result OnShutdown(UIControlledApplication application)
    {
        return Result.Succeeded;
    }

    protected virtual void ConfigureBaseDependencies(IServiceCollection serviceCollection,
        UIControlledApplication application)
    {
        serviceCollection.AddSingleton(application);
        serviceCollection.AddSingleton(application.ActiveAddInId);
        serviceCollection.AddSingleton(application.ControlledApplication);
    }

    protected void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ISyncBackGroundService, RegisterUpdatersService>();
        services.AddSingleton<ISyncBackGroundService, ConfigurePanelService>();
        services.AddElectricalSystemsUpdater(configuration);
        services.AddScoped<IUpdaterTrigger, OnCreatedElectricalSystemTrigger>();
        
        services.AddMenuBuilder<RevitVisitorBuilder>();
    }
}
