namespace CommonUtils.Services;

using System;
using System.Threading.Tasks;
using Abstractions;

/// <inheritdoc />
public class RevitDispatcher : IRevitDispatcher
{
    private readonly RevitTask _revitTask;

    public RevitDispatcher(RevitTask revitTask)
    {
        _revitTask = revitTask;
    }

    public async Task RunAsync(Action action)
    {
        await _revitTask.Run(_ => action());
    }

    public async Task<TResult> RunAsync<TResult>(Func<TResult> func)
    {
        return await _revitTask.Run(_ => func());
    }
}
