namespace AddedElectricalSystemsUpdater;

using CommonUtils;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddElectricalSystemsUpdater(this IServiceCollection services)
    {
        services.AddScoped<UpdaterBase, AddedElectricalSystemsUpdater>();
    }
}
