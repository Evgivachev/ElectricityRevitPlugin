using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.DB;
using Application = Autodesk.Revit.ApplicationServices.Application;

namespace ElectricityRevitPlugin.UpdateModels
{
    public class ModelUpdater
    {
        public ModelUpdater()
        {
            _saveAsOption.SetWorksharingOptions(_worksharingSaveAsOptions);
            _openOptions.SetOpenWorksetsConfiguration(_worksetConfiguration);
        }
        private OpenOptions _openOptions = new OpenOptions()
        {
            AllowOpeningLocalByWrongUser = false,
            Audit = true,
            DetachFromCentralOption = DetachFromCentralOption.DetachAndPreserveWorksets
            
        };

        private WorksetConfiguration _worksetConfiguration = new WorksetConfiguration(WorksetConfigurationOption.CloseAllWorksets);

        private SaveAsOptions _saveAsOption = new SaveAsOptions()
        {
            Compact = false,
            MaximumBackups = 1,
            OverwriteExistingFile = true,
        };

        private WorksharingSaveAsOptions _worksharingSaveAsOptions = new WorksharingSaveAsOptions()
        {
            ClearTransmitted = false,
            OpenWorksetsDefault = SimpleWorksetConfiguration.AskUserToSpecify,
            SaveAsCentral = true
        };
        public string TargetDirectory { get; set; }
        public Func<string, string> TransformName = s => s; 
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
