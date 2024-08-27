using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Hosting;

namespace CommonUtils.Services;

/// <inheritdoc />
public class WpfPlugin : IHostedService
{
    private readonly Window _window;
    private readonly IApplicationLifetime _applicationLifetime;

    /// <inheritdoc />
    public WpfPlugin(Window window, IApplicationLifetime applicationLifetime)
    {
        _window = window;
        _applicationLifetime = applicationLifetime;
        _window.Closed += (_, _) => StopApplication();
    }

    private void StopApplication()
    {
        _applicationLifetime.StopApplication();
    }

    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _window.Show();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
