namespace CommonUtils.Extensions;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class HostExtensions
{
    /// <summary>
    /// Returns a Task that completes when shutdown is triggered via the given token.
    /// </summary>
    /// <param name="host">The running <see cref="IHost"/>.</param>
    /// <param name="revitTask"><see cref="RevitTask"/></param>
    /// <param name="token">The token to trigger shutdown.</param>
    public static async Task WaitForShutdownAsync(
        this IHost host,
        RevitTask revitTask,
        CancellationToken token = default)
    {
        var applicationLifetime = host.Services.GetService<IApplicationLifetime>();
        token.Register(state => { ((IApplicationLifetime) state).StopApplication(); },
            applicationLifetime);
        var waitForStop = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
        applicationLifetime.ApplicationStopping.Register(obj =>
        {
            var tcs = (TaskCompletionSource<object>) obj;
            tcs.TrySetResult(null);
        }, waitForStop);
        await waitForStop.Task;

        // Host will use its default ShutdownTimeout if none is specified.
        await revitTask.Run(async _ => { host.StopAsync(); });
    }

    public static async Task RunAsync(this IHost host, RevitTask revitTask, CancellationToken token = default)
    {
        try
        {
            await host.StartAsync(token);
            await host.WaitForShutdownAsync(token);
        }
        finally
        {
            await revitTask.Run(_ => host.Dispose());
        }
    }
}