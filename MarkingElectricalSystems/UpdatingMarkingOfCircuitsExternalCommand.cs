namespace MarkingElectricalSystems;

using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Services;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class UpdatingMarkingOfCircuitsExternalCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiApp = commandData.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var app = uiApp.Application;
        var doc = uiDoc.Document;
        var result = Result.Failed;
        using (var trGr = new TransactionGroup(doc))
        {
            trGr.Start("Обновление параметров");
            var annotations = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_GenericAnnotation)
                .WhereElementIsNotElementType()
                .Where(x => x.Name == "Марка групп цепей")
                .Cast<AnnotationSymbol>();
            var parameterSetter = new MarkParameterSetter();
            parameterSetter.SetParameters(doc, annotations);
            result = trGr.Assimilate() == TransactionStatus.Committed ? Result.Succeeded : Result.Failed;
        }

        return result;
    }
}
