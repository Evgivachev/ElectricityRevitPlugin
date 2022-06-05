namespace ElectricityRevitPlugin.GeneralSubject
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;

    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    class ShowGeneralSubjectWindowExternalCommand : DefaultExternalCommand
    {
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            var viewModel = GeneralSubjectViewModel.GetGeneralSubjectViewModel(UiDoc);
            viewModel.Run();
            return Result.Succeeded;
        }
    }
}
