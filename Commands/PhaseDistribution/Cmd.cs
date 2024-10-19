using Autodesk.Revit.Attributes;
using CommonUtils;
using Microsoft.Extensions.DependencyInjection;
using PhaseDistribution.Services;

namespace PhaseDistribution;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class Cmd : CmdBase
{
    /// <inheritdoc />
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ICmdUseCase, PhaseDistributionManager>();
    }
}
