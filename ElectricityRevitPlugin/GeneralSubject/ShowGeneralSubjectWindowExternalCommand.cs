using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ElectricityRevitPlugin.GeneralSubject
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    class ShowGeneralSubjectWindowExternalCommand : DefaultExternalCommand
    {
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            var viewModel = new GeneralSubjectViewModel(UiDoc);
            var window = new GeneralSubjectWpf(viewModel);
            window.ShowDialog();
            return Result.Succeeded;
        }
    }
}
