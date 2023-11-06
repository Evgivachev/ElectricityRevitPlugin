namespace PhaseDistribution;

using Abstractions;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using CommonUtils;
using CommonUtils.Services;
using Microsoft.Extensions.DependencyInjection;
using Services;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class Cmd : CmdBase
{
    /// <inheritdoc />
    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IPhaseDistributionManager, PhaseDistributionManager>();
    }

    /// <inheritdoc />
    protected override Result Execute(IServiceProvider serviceProvider)
    {
        serviceProvider.GetService<IPhaseDistributionManager>()!.Execute();
        return Result.Succeeded;
    }
}
