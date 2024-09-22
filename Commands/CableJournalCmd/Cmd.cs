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
    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ICmdUseCase, UseCase>();
        serviceCollection.AddSingleton<ICableJournalService, CableJournalService>()
            .AddTransient<ICableRepository, CableRepository>();
    }
}
