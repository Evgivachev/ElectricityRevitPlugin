﻿namespace ElectricityRevitPlugin.UpdateModels;

using System;
using System.IO;
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
            var fileInfo = new FileInfo(revitModelPath);
            //var revitPath = Path.Combine(revitModel.FullName
            //    .Split(Path.DirectorySeparatorChar)
            //    .Skip(5)
            //    .ToArray())
            //    ;
            //revitPath = revitPath.Replace('\\', '/');
            ////revitPath = Path.Combine(centralServerLocation,revitPath);
            //var modelPath = new ServerPath(centralServerLocation, revitPath);

            ////var flag = ModelPathUtils.IsValidUserVisibleFullServerPath(revitPath);
            ////var q = ModelPathUtils.IsValidUserVisibleFullServerPath(modelPath.ToString());
            ////modelPath = ModelPathUtils.ConvertUserVisiblePathToModelPath(revitModel.FullName);
            var modelPath = new FilePath(revitModelPath);
            Doc = App.OpenDocumentFile(modelPath, _openOptions);
            throw new NotImplementedException();
            //Doc.SaveAs();
            //UiDoc = UiApp.OpenAndActivateDocument(q, openOption, true);
            //UiDoc = UiApp.OpenAndActivateDocument(modelPath, openOption, true);
            //break;
        }

        //var modelPath = new ServerPath()
        //var doc = App.OpenDocumentFile()
        //var files = File.ReadLines()
        //throw new NotImplementedException();
        return Result.Succeeded;
    }
}
