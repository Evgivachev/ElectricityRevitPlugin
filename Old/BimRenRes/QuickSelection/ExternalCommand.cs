using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BimRenRes.QuickSelection;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
internal class ExternalCommand : DefaultExternalCommand
{
    protected override Result DoWork(ref string message, ElementSet elements)
    {
            var result = Result.Succeeded;
            using (var viewModel = new QuickSelectionViewModel(UiApp))
            {
                viewModel.DoWork();
            }
            return result;
        }
}