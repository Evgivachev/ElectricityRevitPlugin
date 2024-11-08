namespace ElectricityRevitPlugin.UpdateModels;

using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
class SaveLocalCopyExternalCommand : DefaultExternalCommand
{
    private OpenOptions _openOptions = new()
    {
        AllowOpeningLocalByWrongUser = false,
        Audit = true,
        DetachFromCentralOption = DetachFromCentralOption.DetachAndPreserveWorksets
    };

    protected override Result DoWork(ref string message, ElementSet elements)
    {
        var modelGetter = new ModelFromServerListGetter();
        var textPath = @"C:\Users\iev\Documents\Список моделей для теста.txt";
        var modelsPath = modelGetter.GetModels(textPath);
        foreach (var revitModelPath in modelsPath)
        {
            var modelPath = new FilePath(revitModelPath);
            Doc = App.OpenDocumentFile(modelPath, _openOptions);
            throw new NotImplementedException();
        }
        
        return Result.Succeeded;
    }
}
