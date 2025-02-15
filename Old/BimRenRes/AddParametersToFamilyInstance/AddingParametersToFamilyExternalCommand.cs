using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BimRenRes.AddParametersToFamilyInstance;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
class AddingParametersToFamilyExternalCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
            var uiApp = commandData?.Application;
            var uiDoc = uiApp?.ActiveUIDocument;
            var doc = uiDoc?.Document;
            var app = uiApp?.Application;
            var result = Result.Succeeded;
            try
            {
                using (var tr = new Transaction(doc))
                {
                    tr.Start("Добавление параметров");
                    if (doc != null && !doc.IsFamilyDocument)
                    {
                        message += "Необходимо открыть файл семейства Revit";
                        return Result.Failed;
                    }
                    var viewModel = new AddingParametersViewModel(doc);
                    var window = new AddingParametersView { DataContext = viewModel };
                    if (true != window.ShowDialog())
                        result = Result.Cancelled;
                    tr.Commit();
                }
                return result;
            }
            catch (Exception e)
            {
                message += e.Message + "\n" + e.StackTrace;
                return Result.Failed;
            }
        }
}