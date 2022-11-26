namespace MarkingElectricalSystems;

using PikTools.Ui;
using RxBim.Di;

/// <inheritdoc />
public class Config : ICommandConfiguration
{
    /// <inheritdoc />
    public void Configure(IContainer container)
    {
        container.AddUi();
    }
}
