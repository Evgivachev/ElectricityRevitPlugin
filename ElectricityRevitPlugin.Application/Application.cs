namespace ElectricityRevitPlugin.Application;

using System.Reflection;
using AddedElectricalSystemsUpdater;
using Autodesk.Revit.UI;
using RibbonBuilder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Triggers;
using Result = Autodesk.Revit.UI.Result;

public class Application : RxBimApplication
{
    public override Result OnStartup(UIControlledApplication application)
    {
        try
        {
            var hostBuilder = new HostBuilder();
            hostBuilder.ConfigureHostConfiguration(_ => { });

            hostBuilder.ConfigureAppConfiguration((_, builder) =>
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
        catch (Exception)
        {
            return Result.Failed;
        }
    }
    public override Result OnShutdown(UIControlledApplication application)
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
        services.AddSingleton<ISyncBackGroundService, RegisterAssemblyResolver>();
        services.AddElectricalSystemsUpdater(configuration);
        services.AddScoped<IUpdaterTrigger, OnCreatedElectricalSystemTrigger>();
        
        services.AddMenuBuilder<RevitVisitorBuilder>();
    }
}
