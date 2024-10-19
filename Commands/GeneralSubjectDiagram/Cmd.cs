namespace GeneralSubjectDiagram;

using System.Collections.Generic;
using System.Reflection;
using Autodesk.Revit.Attributes;
using CommonUtils;
using ElectricityRevitPlugin.UI;
using ElectricityRevitPlugin.UI.Services;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using MoreLinq;
using Services.ParametersUpdaters;
using ViewModels;
using Views;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
[UsedImplicitly]
public class Cmd : WpfCmd<GeneralSubjectView>
{
    /// <inheritdoc />
    protected override void ConfigureServices(IServiceCollection services)
    {
        Assembly.Load(typeof(MoreEnumerable).Assembly.Location);
        services.AddSingleton<IUIDispatcher, UiDispatcher>();
        services.AddSingleton<GeneralSubjectView>()
            .AddSingleton<GeneralSubjectViewModel>();
        
        services.AddSingleton(_ => (IReadOnlyCollection<ParameterUpdater>)new[]
        {
            (ParameterUpdater)new CableParameterUpdater(),
            new DisconnectingDeviceParameterUpdater(),
            new ShieldParameterUpdater()
        });
    }
}
