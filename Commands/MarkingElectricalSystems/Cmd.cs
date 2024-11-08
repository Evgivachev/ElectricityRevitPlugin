namespace MarkingElectricalSystems;

using Application;
using Autodesk.Revit.Attributes;
using CommonUtils;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;

/// <inheritdoc />
[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class Cmd : CmdBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ICmdUseCase, CmdUseCase>();
        services.AddTransient<IElementsRepository, ElementsRepository>();
        services.AddTransient<IParameterSettingService, ParameterSettingService>();
    }
}
