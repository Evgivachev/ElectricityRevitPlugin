namespace GroupByGost.Infrastructure;

using Application;
using CommonUtils;
using Microsoft.Extensions.DependencyInjection;

public static class Extensions
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IDbRepository, DbRepository>();
        services.AddSingleton<ITransactionsService, TransactionsService>();
    }
}
