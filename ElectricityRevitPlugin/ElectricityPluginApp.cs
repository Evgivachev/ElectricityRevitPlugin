using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityRevitPlugin
{
    public class ElectricityPluginApp : IExternalApplication
    {
        public readonly string NameApp = "ElAp";
        public AppSetting AppSetting = new AppSetting();

        public Result OnShutdown(UIControlledApplication application)
        {
            throw new NotImplementedException();
        }

        public Result OnStartup(UIControlledApplication application)
        {
            throw new NotImplementedException();
        }

        protected virtual Result BuiltPanel()
        {
            throw new NotImplementedException();
        }
    }
}
