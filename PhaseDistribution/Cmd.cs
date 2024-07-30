using Autodesk.Revit.Attributes;
using CommonUtils;
using CommonUtils.Services;
using Microsoft.Extensions.DependencyInjection;
using PhaseDistribution.Services;

namespace PhaseDistribution;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class Cmd : CmdBase
{
    /// <inheritdoc />
    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ICmdUseCase, PhaseDistributionManager>();
    }
}
