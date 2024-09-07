namespace CommonUtils;

using System;
using System.Threading;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/// <inheritdoc cref="Autodesk.Revit.UI.IExternalCommand" />
public abstract class WpfCmd<TWindow> : CmdBase
    where TWindow : Window
{
    private readonly ManualResetEvent _shutdownBlock = new(false);
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
            });
            var host = hostBuilder.Build();
            _host = host;
            host.Start();
            var window = host.Services.GetService<TWindow>();
            window.Closed += (sender, args) =>
            {
                _host.Services.GetService<RevitTask>().Run(_ =>
                {
                    _host?.Services.GetService<IApplicationLifetime>().ApplicationStopped
                        .Register(() => { _shutdownBlock.Set(); });
                    _host?.StopAsync();
                    _shutdownBlock.WaitOne();
                    _host?.Dispose();
                    _host = null;
                });
            };

            window.Show();

            return Result.Succeeded;
        }
        catch (Exception e)
        {
            message = e.ToString();
            return Result.Failed;
        }
    }

    /// <inheritdoc />
    protected abstract override void ConfigureServices(IServiceCollection serviceCollection);
}
