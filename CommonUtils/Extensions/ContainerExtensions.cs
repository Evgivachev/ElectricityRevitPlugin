namespace CommonUtils.Extensions;

using Autodesk.Revit.UI;
using RxBim.Di;

/// <summary>
/// Методы расширения
/// </summary>
public static class ContainerExtensions
{
    /// <summary>
    /// Добавляет базовые зависимости от ревит в контейнер.
    /// </summary>
    /// <param name="container"><see cref="IContainer"/></param>
    /// <param name="commandData"><see cref="ExternalCommandData"/></param>
    public static IContainer AddBaseRevitDependencies(this IContainer container, ExternalCommandData commandData)
    {
        container.AddInstance(commandData)
            .AddInstance(commandData.Application)
            .AddInstance(commandData.Application.Application)
            .AddTransient(() => commandData.Application.ActiveUIDocument)
            .AddTransient(() => commandData.Application.ActiveUIDocument?.Document!)
            .AddTransient(() => commandData.Application.ActiveUIDocument.Selection);
        return container;
    }
}
