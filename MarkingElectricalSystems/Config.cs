namespace MarkingElectricalSystems;

using PikTools.LogWindow;
using RxBim.Di;

/// <inheritdoc />
public class Config : ICommandConfiguration
{
    /// <inheritdoc />
    public void Configure(IContainer container)
    {
        container.AddWindowLogger();
    }
}
