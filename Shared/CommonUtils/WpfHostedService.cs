namespace CommonUtils;

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class WpfHostedService<TWindow> : IHostedService where TWindow : Window
{
    private readonly IServiceProvider _serviceProvider;
    public WpfHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var window = _serviceProvider.GetRequiredService<TWindow>();
        window.Show();
        return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
