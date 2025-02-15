using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BimRenRes;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
class SelectSuperComponentsExternalCommand : DefaultExternalCommand
{
    protected override Result DoWork(ref string message, ElementSet elements)
    {
        var selection = UiDoc.Selection;
        var superComponents = new HashSet<ElementId>();
        foreach (var elementId in selection.GetElementIds())
        {
            var fi = Doc.GetElement(elementId) as FamilyInstance;
            var sc = fi?.SuperComponent;
            if(sc is null)
                continue;
            superComponents.Add(sc.Id);
        }
        selection.SetElementIds(superComponents);
        return Result.Succeeded;
    }
}