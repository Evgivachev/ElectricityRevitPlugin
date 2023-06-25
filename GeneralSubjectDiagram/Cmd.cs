namespace GeneralSubjectDiagram;

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommonUtils.Extensions;
using JetBrains.Annotations;
using RxBim.Di;
using Views;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
[UsedImplicitly]
public class Cmd : IExternalCommand
{
    private static GeneralSubjectView? _view;

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
    }

    private IContainer CreateContainer(ExternalCommandData commandData)
    {
        IContainer container = new SimpleInjectorContainer();
        container.AddBaseRevitDependences(commandData);
        var config = new Config();
        config.Configure(container);
        return container;
    }
}
