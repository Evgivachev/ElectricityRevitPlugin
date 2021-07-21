using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ElectricityRevitPlugin.UpdateModels
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
     class SaveLocalCopyExternalCommand : DefaultExternalCommand
    {
        private OpenOptions _openOptions = new OpenOptions()
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


                var saveAsOption = new SaveAsOptions()
                {
                    Compact = false,
                    MaximumBackups = 1,
                    OverwriteExistingFile = true,
                };
                var savePath = fileInfo.Directory.FullName + "\\" + fileInfo.Name + "2021";
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
}
