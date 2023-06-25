namespace GeneralSubjectDiagram;

using System.Reflection;
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
        Assembly.Load(typeof(MoreLinq.MoreEnumerable).Assembly.Location);
        container.AddUi();
        container.AddSingleton<GeneralSubjectView>()
            .AddSingleton<GeneralSubjectViewModel>();
    }
}
