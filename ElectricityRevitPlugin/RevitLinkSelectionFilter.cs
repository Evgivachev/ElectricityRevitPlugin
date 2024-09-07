namespace ElectricityRevitPlugin;

using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

class RevitLinkSelectionFilter : ISelectionFilter
{
    public bool AllowElement(Element elem)
    {
        return elem is RevitLinkInstance;
    }

    public bool AllowReference(Reference reference, XYZ position)
    {
        throw new NotImplementedException();
    }
}
