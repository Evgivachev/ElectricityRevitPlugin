namespace CommonUtils.Abstractions;

using System;
using System.Threading.Tasks;

public interface IRevitDispatcher
{
    Task RunAsync(Action action);
    Task<TResult> RunAsync<TResult>(Func<TResult> func);
}