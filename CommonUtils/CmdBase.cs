#pragma warning disable CS4014
namespace CommonUtils;

using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/// <summary>
/// Базовый класс команды
/// </summary>
public abstract class CmdBase : IExternalCommand, IExternalCommandAvailability
{
    /// <inheritdoc />
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        try
        {
            var hostBuilder = new HostBuilder();
            hostBuilder.UseConsoleLifetime();
            hostBuilder.ConfigureServices(sc =>
            {
                ConfigureBaseDependencies(sc, commandData);
                ConfigureServices(sc);
            });
            var host = hostBuilder.Build();
            host.RunAsync(host.Services.GetService<RevitTask>()!);
            var result = Execute(host.Services);
            return result;
        }
        catch (Exception e)
        {
            message = e.ToString();
            return Result.Failed;
        }
    }

    private void ConfigureBaseDependencies(IServiceCollection serviceCollection, ExternalCommandData commandData)
    {
        serviceCollection.AddSingleton(commandData.Application);
        serviceCollection.AddSingleton(commandData.Application.Application);
        serviceCollection.AddTransient(_ => commandData.Application.ActiveUIDocument);
        serviceCollection.AddTransient(_ => commandData.Application.ActiveUIDocument.Document);
        var task = new RevitTask();
        serviceCollection.AddTransient(typeof(RevitTask), _ => task);
    }

    /// <summary>
    /// Конфигурирует сервисы.
    /// </summary>
    /// <param name="serviceCollection"><see cref="IServiceCollection"/></param>
    protected abstract void ConfigureServices(IServiceCollection serviceCollection);

    /// <summary>
    /// Запускает выполнение плагина.
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
    protected abstract Result Execute(IServiceProvider serviceProvider);

    /// <inheritdoc />
    public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
    {
        return applicationData.ActiveUIDocument.Document is not null;
    }
}