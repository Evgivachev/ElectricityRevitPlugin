namespace CountFixturesInSpaceCmd;

using Autodesk.Revit.Attributes;
using CommonUtils;
using Microsoft.Extensions.DependencyInjection;

/// <inheritdoc />
[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class Cmd2 : CmdBase
{
    /// <inheritdoc />
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ICmdUseCase, CountFixturesInSpaceService2>();
    }
}
