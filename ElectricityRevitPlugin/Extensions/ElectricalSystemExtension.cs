using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Electrical;

namespace ElectricityRevitPlugin.Extensions
{
    public static class ElectricalSystemExtension
    {
        private static readonly Guid ByGost  = new Guid("8d1b8079-3007-4140-835c-73f0de4e81bd");

        public static string GetGroupByGost(this ElectricalSystem es)
        {
            return es.get_Parameter(ByGost).AsString();
        }
    }
}
