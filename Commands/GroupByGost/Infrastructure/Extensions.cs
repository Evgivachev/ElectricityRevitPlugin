namespace GroupByGost.Infrastructure;

using Bll;
using CommonUtils;
using Microsoft.Extensions.DependencyInjection;

public static class Extensions
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IDbRepository, DbRepository>();
        services.AddTransient<ITransactionsService, TransactionsService>();
    }
}
