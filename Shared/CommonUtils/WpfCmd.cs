namespace CommonUtils;

using System;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/// <inheritdoc cref="Autodesk.Revit.UI.IExternalCommand" />
public abstract class WpfCmd<TWindow> : CmdBase
    where TWindow : Window
{
    private static IHost? _host;

    /// <inheritdoc />
    public override Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        try
        {
            if (_host is not null)
            {
                _host.Services.GetService<TWindow>().Show();
                return Result.Succeeded;
            }

            var hostBuilder = new HostBuilder();
            hostBuilder.ConfigureServices(sc =>
            {
                ConfigureBaseDependencies(sc, commandData);
                ConfigureServices(sc);
                sc.AddHostedService<WpfHostedService<TWindow>>();
                sc.AddSingleton<IHostLifetime, WpfLifeTime<TWindow>>();
            });
            _host = hostBuilder.Build();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Run(_host);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            return Result.Succeeded;
        }
        catch (Exception e)
        {
            _host?.Dispose();
            message = e.ToString();
            return Result.Failed;
        }
    }

    /// <inheritdoc />
    protected abstract override void ConfigureServices(IServiceCollection services);

    private async Task Run(IHost host)
    {
        try
        {
            await host.StartAsync();
            await host.WaitForShutdownAsync();
        }
        finally
        {
            _host = null;
            var revitTask = host.Services.GetRequiredService<RevitTask>();
            await revitTask.Run(_ => host.Dispose());
        }
    }
}
