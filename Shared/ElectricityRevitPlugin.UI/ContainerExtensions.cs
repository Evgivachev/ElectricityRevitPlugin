namespace ElectricityRevitPlugin.UI;

using RxBim.Di;
using Services;

public static class ContainerExtensions
{
    public static IContainer AddUI(this IContainer container)
    {
        container.AddSingleton<IUIDispatcher, UiDispatcher>();
        
        return container;
    }
    
}
