namespace CommonUtils.Services;

using System.Windows;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;

/// <inheritdoc />
public class WpfPlugin : ApplicationLifetime
{
    private readonly Window _window;

    /// <inheritdoc />
    public WpfPlugin(ILogger<WpfPlugin> logger, Window window)
        : base(logger)
    {
        _window = window;
        _window.Closed += (_, _) => StopApplication();
    }

    public void Run()
    {
        _window.Show();
    }
}
