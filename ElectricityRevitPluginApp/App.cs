namespace ElectricityRevitPluginApp
{
    using RxBim.Application.Revit;
    using RxBim.Shared;

    /// <summary>
    /// app
    /// </summary>
    public class App : RxBimApplication
    {
        /// <summary>
        /// start
        /// </summary>
        public PluginResult Start()
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
}
