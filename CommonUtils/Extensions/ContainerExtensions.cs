namespace CommonUtils.Extensions;

using Autodesk.Revit.UI;
using RxBim.Di;

public static class ContainerExtensions
{
    public static IContainer AddBaseRevitDependences(this IContainer container, ExternalCommandData commandData)
    {
        container.AddInstance(commandData)
            .AddInstance(commandData.Application)
            .AddInstance(commandData.Application.Application)
            .AddTransient(() => commandData.Application.ActiveUIDocument)
            .AddTransient(() => commandData.Application.ActiveUIDocument?.Document)
            .AddTransient(() => commandData.Application.ActiveUIDocument.Selection);
        return container;
    }
}
