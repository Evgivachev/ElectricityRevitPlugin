namespace CommonUtils;

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Hosting;

public class WpfLifeTime<TWindow> : IHostLifetime, IDisposable
    where TWindow : Window
{
    private readonly IApplicationLifetime _applicationLifetime;
    private readonly TWindow _window;

    public WpfLifeTime(IApplicationLifetime applicationLifetime, TWindow window)
    {
        _applicationLifetime = applicationLifetime;
        _window = window;
    }

    public Task WaitForStartAsync(CancellationToken cancellationToken)
    {
        _window.Closed += WindowOnClosed;
        return Task.CompletedTask;
    }

    // Сделано по аналогии с ConsoleLifetime.
    private void WindowOnClosed(object sender, EventArgs e)
    {
        _applicationLifetime.StopApplication();
    }
    
    // Вызывается автоматически при остановке приложения
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    // Вызывается автоматически
    public void Dispose()
    {

    }
}
