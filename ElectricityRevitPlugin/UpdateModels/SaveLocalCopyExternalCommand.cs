using System;
using System.Collections.Generic;
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
    public class SaveLocalCopyExternalCommand : DefaultExternalCommand
    {
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            var root = @"\\10.1.1.2\revit\PROJECTS\тест_19ПД77-МСИ";
            var centralServerLocation = @"RNS://192.168.10.205/";
            var rootDirectory = new DirectoryInfo(root);
            var revitModels = rootDirectory.EnumerateDirectories("*.rvt", SearchOption.AllDirectories);
            var qq = string.Join("\n", revitModels);
            File.WriteAllLines(@"C:\Users\iev\Documents\Список моделей для теста.txt", revitModels.Select(di => Path.Combine(di.FullName.Split(Path.DirectorySeparatorChar)
                .Skip(5).ToArray())));

            //MessageBox.Show(q);
            //var openOption = new OpenOptions()
            //{
            //    AllowOpeningLocalByWrongUser = false,
            //    Audit = true,
            //    DetachFromCentralOption = DetachFromCentralOption.DetachAndPreserveWorksets
            //};
            
            //foreach (var revitModel in revitModels)
            //{
                
            //    var revitPath = Path.Combine(revitModel.FullName
            //        .Split(Path.DirectorySeparatorChar)
            //        .Skip(5)
            //        .ToArray())
            //        ;
            //    revitPath = revitPath.Replace('\\', '/');
            //    //revitPath = Path.Combine(centralServerLocation,revitPath);
            //    var modelPath = new ServerPath(centralServerLocation, revitPath);
            //    //var flag = ModelPathUtils.IsValidUserVisibleFullServerPath(revitPath);
            //    //var q = ModelPathUtils.IsValidUserVisibleFullServerPath(modelPath.ToString());
            //    //modelPath = ModelPathUtils.ConvertUserVisiblePathToModelPath(revitModel.FullName);
            //    //Doc = App.OpenDocumentFile(modelPath, openOption);
            //    var q = ModelPathUtils.ConvertUserVisiblePathToModelPath(revitModel.FullName);
            //    UiDoc = UiApp.OpenAndActivateDocument(q, openOption, true);
            //    UiDoc =  UiApp.OpenAndActivateDocument(modelPath, openOption,true);
            //    break;
            //}


            //var modelPath = new ServerPath()
            //var doc = App.OpenDocumentFile()
            //var files = File.ReadLines()
            //throw new NotImplementedException();
            return Result.Succeeded;
        }
    }
}
