namespace GeneralSubjectDiagram;

using CommonUtils.Extensions;
using RxBim.Di;
using ViewModels;
using Views;

/// <inheritdoc />
public class Config : ICommandConfiguration
{
    /// <inheritdoc />
    public void Configure(IContainer container)
    {
        container.AddUi();
        container.AddSingleton<GeneralSubjectView>()
            .AddSingleton<GeneralSubjectViewModel>();
    }
}
