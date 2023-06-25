namespace GeneralSubjectDiagram;

using CommonUtils.Extensions;
using RxBim.Di;

/// <inheritdoc />
public class Config : ICommandConfiguration
{
    /// <inheritdoc />
    public void Configure(IContainer container)
    {
        container.AddUi();
        container.AddSingleton<Views.GeneralSubjectView>()
            .AddSingleton<ViewModels.GeneralSubjectViewModel>();
    }
}
