namespace ElectricityRevitPlugin.UpdateModels
{
    using System;
    using System.IO;
    using System.Windows;
    using Autodesk.Revit.DB;
    using Application = Autodesk.Revit.ApplicationServices.Application;

    public class ModelUpdater
    {
        private OpenOptions _openOptions = new OpenOptions()
        {
            AllowOpeningLocalByWrongUser = false,
            Audit = true,
            DetachFromCentralOption = DetachFromCentralOption.DetachAndPreserveWorksets
        };

        private SaveAsOptions _saveAsOption = new SaveAsOptions()
        {
            Compact = false,
            MaximumBackups = 1,
            OverwriteExistingFile = true,
        };

        private WorksetConfiguration _worksetConfiguration = new WorksetConfiguration(WorksetConfigurationOption.CloseAllWorksets);

        private WorksharingSaveAsOptions _worksharingSaveAsOptions = new WorksharingSaveAsOptions()
        {
            ClearTransmitted = false,
            OpenWorksetsDefault = SimpleWorksetConfiguration.AskUserToSpecify,
            SaveAsCentral = true
        };

        public Func<string, string> TransformName = s => s;

        public ModelUpdater()
        {
            _saveAsOption.SetWorksharingOptions(_worksharingSaveAsOptions);
            _openOptions.SetOpenWorksetsConfiguration(_worksetConfiguration);
        }

        public string TargetDirectory { get; set; }

        public bool UpdateModel(Application app, string path)
        {
            var name2019 = new FileInfo(path);
            return TryUpdateModel(app, name2019);
        }

        public bool TryUpdateModel(Application app, FileInfo path)
        {
            try
            {
                var name2019 = path.Name;
                var modelPath = new FilePath(path.FullName);
                var doc = app.OpenDocumentFile(modelPath, _openOptions);
                var savePath = TargetDirectory + "\\" + TransformName(name2019);
                var saveModelPath = new FilePath(savePath);
                doc.SaveAs(saveModelPath, _saveAsOption);
                return doc.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}\n{e.StackTrace}");
                return false;
            }
        }
    }
}
