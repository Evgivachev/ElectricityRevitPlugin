using Autodesk.Revit.Attributes;
using CommonUtils;
using Microsoft.Extensions.DependencyInjection;
using ShortCircuits.Services;

namespace ShortCircuits;

/// <inheritdoc />
[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class Cmd : CmdBase
{
    /// <inheritdoc />
    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ShortCircuitsService>();
        serviceCollection.AddSingleton<OneShortCircuitsService>();
        serviceCollection.AddSingleton<ICmdUseCase, CommonShortCircuitsCmdUseCase>();
    }
}
