using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BimRenRes.PrintAndExport;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
class ExternalCommand :DefaultExternalCommand
{
    protected override Result DoWork(ref string message, ElementSet elements)
    {
        var view = new PrintAndExportView();
        var exportViewModel = new PrintAndExportCommonViewModel(Doc);
        exportViewModel.View = view;
        view.DataContext = exportViewModel;
        view.ShowDialog();
        return Result.Succeeded;

    }
}