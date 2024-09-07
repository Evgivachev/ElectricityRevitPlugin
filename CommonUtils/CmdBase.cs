namespace CommonUtils;

using System;
using System.Threading;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/// <summary>
/// Базовый класс команды
/// </summary>
public abstract class CmdBase : IExternalCommand, IExternalCommandAvailability
{
    private readonly ManualResetEvent _shutdownBlock = new(false);

    /// <inheritdoc cref="IExternalCommand.Execute" />
    public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
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
            using var host = hostBuilder.Build();
            host.Start();
            var usecase = host.Services.GetService<ICmdUseCase>();
            var result = usecase.Execute(commandData, ref message, elements);
            host.Services.GetService<IApplicationLifetime>().ApplicationStopped
                .Register(() => { _shutdownBlock.Set(); });

            host.StopAsync();
            _shutdownBlock.WaitOne();
            return result;
        }
        catch (Exception e)
        {
            message = e.ToString();
            return Result.Failed;
        }
    }

    /// <summary>
    /// Конфигурирует базовые зависимости.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="commandData"></param>
    protected virtual void ConfigureBaseDependencies(IServiceCollection serviceCollection,
        ExternalCommandData commandData)
    {
        serviceCollection.AddSingleton(commandData);
        serviceCollection.AddSingleton(commandData.Application);
        serviceCollection.AddSingleton(commandData.Application.Application);
        serviceCollection.AddTransient(_ => commandData.Application.ActiveUIDocument);
        serviceCollection.AddTransient(_ => commandData.Application.ActiveUIDocument?.Document);
        var task = new RevitTask();
        serviceCollection.AddTransient(typeof(RevitTask), _ => task);
    }

    /// <summary>
    /// Конфигурирует сервисы.
    /// </summary>
    /// <param name="serviceCollection"><see cref="IServiceCollection"/></param>
    protected abstract void ConfigureServices(IServiceCollection serviceCollection);

    /// <inheritdoc />
    public virtual bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
    {
        return applicationData.ActiveUIDocument?.Document is not null;
    }
}
