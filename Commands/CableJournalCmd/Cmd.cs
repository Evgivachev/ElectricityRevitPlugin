namespace CableJournalCmd;

using Application;
using Autodesk.Revit.Attributes;
using CommonUtils;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class Cmd : CmdBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ICmdUseCase, UseCase>();
        services.AddSingleton<ICableJournalService, CableJournalService>()
            .AddTransient<ICableRepository, CableRepository>();
    }
}
