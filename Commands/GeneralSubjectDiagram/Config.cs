namespace GeneralSubjectDiagram;

using System.Collections.Generic;
using System.Reflection;
using CommonUtils.Extensions;
using RxBim.Di;
using Services.ParametersUpdaters;
using ViewModels;
using Views;

/// <inheritdoc />
public class Config : ICommandConfiguration
{
    /// <inheritdoc />
    public void Configure(IContainer container)
    {
        Assembly.Load(typeof(MoreLinq.MoreEnumerable).Assembly.Location);
        container.AddUi();
        container.AddSingleton<GeneralSubjectView>()
            .AddSingleton<GeneralSubjectViewModel>();
        container.AddInstance(new RevitTask());
        container.AddSingleton(() => (IEnumerable<ParameterUpdater>)new[]
        {
            (ParameterUpdater)new CableParameterUpdater(),
            new DisconnectingDeviceParameterUpdater(),
            new ShieldParameterUpdater()
        });
    }
}
