namespace ElectricityRevitPlugin.Application;

using Autodesk.Revit.UI;

public  abstract class RxBimApplication : IExternalApplication
{
    public abstract Result OnStartup(UIControlledApplication application);
    public abstract Result OnShutdown(UIControlledApplication application);
}
