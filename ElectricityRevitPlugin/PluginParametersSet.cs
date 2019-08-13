using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityRevitPlugin
{
    abstract class PluginParametersSet
    {
        protected abstract Dictionary<string, ExternalDefinition> Parameters { get; }
          
    }
}
