namespace ElectricityRevitPlugin.UI.Services;

using System.Windows.Threading;

public class UiDispatcher : IUIDispatcher
{
    public async Task InvokeAsync(Action action)
    {
        await Dispatcher.CurrentDispatcher.InvokeAsync(action);
    }
}
