namespace ElectricityRevitPlugin.CopyElementsInSameViewSchedule;

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Extensions;

[Regeneration(RegenerationOption.Manual)]
[Transaction(TransactionMode.Manual)]
public class ExternalCommand : DefaultExternalCommand

{
    protected override Result DoWork(ref string message, ElementSet elements)
    {
        var model = new CopyElementsInSameScheduleViewModel(Doc);
        var view = new CopyElementsInSameScheduleView(model);
        if (view.ShowDialog() == true)
            using (var tr = new Transaction(Doc))
            {
                tr.Start("Копирование элементов ключевой спецификации");
                foreach (var element in model.CheckedElements)
                {
                    var activeView = Doc.ActiveView as ViewSchedule;
                    activeView.AddElement(element, false);
                }

                tr.Commit();
            }

        return Result.Succeeded;
    }
}
