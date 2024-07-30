namespace GeneralSubjectDiagram;

using System.Collections.Generic;
using System.Reflection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommonUtils;
using CommonUtils.Extensions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using MoreLinq;
using RxBim.Di;
using Services.ParametersUpdaters;
using ViewModels;
using Views;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
[UsedImplicitly]
public class Cmd : WpfCmd<GeneralSubjectView>
{
    private static GeneralSubjectView? _view;

    /*/// <inheritdoc />
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var container = CreateContainer(commandData);
        if (_view is not null)
        {
            _view.Activate();
            return Result.Succeeded;
        }

        _view = container.GetService<GeneralSubjectView>();
        _view.Closed += (_, _) => _view = null;
        _view.Show();


        return Result.Succeeded;
    }*/

    /// <inheritdoc />
    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        Assembly.Load(typeof(MoreEnumerable).Assembly.Location);
        serviceCollection.AddSingleton<GeneralSubjectView>()
            .AddSingleton<GeneralSubjectViewModel>();
        serviceCollection.AddSingleton(() => (IEnumerable<ParameterUpdater>) new[]
        {
            (ParameterUpdater) new CableParameterUpdater(),
            new DisconnectingDeviceParameterUpdater(),
            new ShieldParameterUpdater()
        });
    }

    private IContainer CreateContainer(ExternalCommandData commandData)
    {
        IContainer container = new SimpleInjectorContainer();
        container.AddBaseRevitDependencies(commandData);
        var config = new Config();
        config.Configure(container);
        return container;
    }

    /// <inheritdoc />
    public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
    {
        return true;
    }
}