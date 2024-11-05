namespace ElectricityRevitPlugin.RibbonBuilder;

using Application;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddMenuBuilder<T>(this IServiceCollection services) where T : class, IVisitorBuilder
    {
        services.AddSingleton<IMenuBuilder, MenuBuilder>();
        services.AddSingleton<IVisitorBuilder, T>();
    }
}
