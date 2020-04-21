using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace ElectricityRevitPlugin
{
    class RevitNameComparer :IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return NamingUtils.CompareNames(x, y);
        }
    }
}
