namespace GroupByGost.Application;

using Microsoft.Extensions.DependencyInjection;

public static class Extensions
{
    public static void AddBll(this IServiceCollection services)
    {
        services.AddScoped<IGroupByGostService, GroupByGostService>();
    }
}
