using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.Hosting;

namespace CommonUtils;

public class WpfUseCase : ICmdUseCase
{
    private readonly Window _window;

    public WpfUseCase(Window window, IApplicationLifetime applicationLifetime)
    {
        _window = window;
        _window.Closed += (sender, args) => applicationLifetime.StopApplication();
    }

    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        _window.Show();
        return Result.Succeeded;
    }

    public void ShutDown()
    {
    }
}
