namespace AddedElectricalSystemsUpdater;

using CommonUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Settings;

public static class ServiceCollectionExtensions
{
    public static void AddElectricalSystemsUpdater(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CircuitInitialValues>(configuration.Bind);
        services.AddScoped<UpdaterBase, AddedElectricalSystemsUpdater>();
    }
}
