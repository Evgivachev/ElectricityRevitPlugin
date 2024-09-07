using System;
using System.Threading;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Microsoft.Extensions.Hosting;

namespace CommonUtils.Services;

public class RevitCmdHostLifetime : IHostLifetime
{
    private readonly IApplicationLifetime _applicationLifetime;
    private readonly UIApplication _application;
    private readonly ManualResetEvent _shutdownBlock = new(false);

    public RevitCmdHostLifetime(IApplicationLifetime applicationLifetime, UIApplication application)
    {
        _applicationLifetime = applicationLifetime;
        _application = application;
    }

    public Task WaitForStartAsync(CancellationToken cancellationToken)
    {
        AppDomain.CurrentDomain.ProcessExit += (_, _) =>
        {
            _applicationLifetime.StopApplication();
            _shutdownBlock.WaitOne();
        };

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // There's nothing to do here
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _shutdownBlock.Set();
    }
}
