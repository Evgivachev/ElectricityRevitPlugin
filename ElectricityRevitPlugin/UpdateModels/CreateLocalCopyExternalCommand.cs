namespace ElectricityRevitPlugin.UpdateModels
{
    using System.IO;
    using System.Windows;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class CreateLocalCopyExternalCommand : DefaultExternalCommand
    {
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            var creator = new CreatorRevitLocalCopy(2019);
            var targetDirectory = @"\\drive\RENESSANS\TEMP\temp__models";
            creator.Destination = s =>
            {
                var fi = new FileInfo(s);
                var result = targetDirectory + "\\" + fi.Name;
                return result;
            };
            creator.Overwrite = true;
            creator.ServerPath = "192.168.10.205";
            var models = File.ReadLines(@"C:\Users\iev\Documents\Список моделей для теста.txt");
            var flag = creator.CreateLocalCopy(models);
            MessageBox.Show(flag.ToString());
            return Result.Succeeded;
        }
    }
}
