namespace ElectricityRevitPluginApp;

using Autodesk.Revit.UI;
using JetBrains.Annotations;
using RxBim.Application.Revit;
using RxBim.Shared;

/// <summary>
/// app
/// </summary>
[PublicAPI]
public class App : RxBimApplication
{
    /// <summary>
    /// start
    /// </summary>
    public PluginResult Start(
        UIApplication uiApplication)
    {
        return PluginResult.Succeeded;
    }

    /// <summary>
    /// shutdown
    /// </summary>
    public PluginResult Shutdown()
    {
        return PluginResult.Succeeded;
    }
}
