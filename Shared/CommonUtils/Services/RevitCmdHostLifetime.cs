namespace CommonUtils.Services;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

public class RevitCmdHostLifetime : IHostLifetime
{
    private readonly IApplicationLifetime _applicationLifetime;
    private readonly ManualResetEvent _shutdownBlock = new(false);

    public RevitCmdHostLifetime(IApplicationLifetime applicationLifetime)
    {
        _applicationLifetime = applicationLifetime;
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
