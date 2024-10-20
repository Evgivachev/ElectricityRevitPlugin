namespace ElectricityRevitPlugin.UI.Services;

using System.Windows.Threading;

public class UiDispatcher : IUIDispatcher
{
    private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;
    public async Task InvokeAsync(Action action)
    {
        await _dispatcher.InvokeAsync(action);
    }
}
