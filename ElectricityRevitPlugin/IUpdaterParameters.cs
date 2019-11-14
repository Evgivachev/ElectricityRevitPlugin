using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityRevitPlugin
{
    public interface IUpdaterParameters<T> where T:Element
    {
        string UpdateParameters(T el);
    }
}
