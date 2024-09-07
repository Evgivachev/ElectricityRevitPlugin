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
    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        Assembly.Load(typeof(MoreEnumerable).Assembly.Location);
        serviceCollection.AddSingleton<IUIDispatcher, UiDispatcher>();
        serviceCollection.AddSingleton<GeneralSubjectView>()
            .AddSingleton<GeneralSubjectViewModel>();
        serviceCollection.AddSingleton(() => (IEnumerable<ParameterUpdater>)new[]
        {
            (ParameterUpdater)new CableParameterUpdater(),
            new DisconnectingDeviceParameterUpdater(),
            new ShieldParameterUpdater()
        });
    }
}
