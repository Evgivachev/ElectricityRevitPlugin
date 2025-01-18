namespace CommonUpdateCmd;

using Application;
using Autodesk.Revit.Attributes;
using CommonUtils;
using GroupByGost.Application;
using GroupByGost.Infrastructure;
using Infrastructure;
using Infrastructure.UpdateElectricalSystem;
using Microsoft.Extensions.DependencyInjection;

/// <inheritdoc />
[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class Cmd : CmdBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ICmdUseCase, UseCase>()
            .AddScoped<IExternalCommand, GroupByGostExternalCommandAdapter>()
            .AddScoped<IGroupByGostService, GroupByGostService>()
            .AddInfrastructure();
        services.AddScoped<IExternalCommand, UpdaterParametersOfShields>()
            .AddScoped<IUpdateElSystemsService, UpdateElSystemsService>();
    }
}
