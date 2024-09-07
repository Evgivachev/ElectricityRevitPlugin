namespace ElectricityRevitPlugin.UI;

public interface IUIDispatcher
{

    Task InvokeAsync(Action action);
}
