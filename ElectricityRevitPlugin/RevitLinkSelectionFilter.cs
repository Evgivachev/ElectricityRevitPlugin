using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace ElectricityRevitPlugin
{
    class RevitLinkSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            var type = elem.GetType().Name;
            return elem is RevitLinkInstance;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            throw new NotImplementedException();
        }
    }
}
