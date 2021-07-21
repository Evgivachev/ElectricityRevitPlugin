using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityRevitPlugin.UpdateModels
{
    class CreatorRevitLocalCopy
    {
        /// <summary>
        /// Создание локальных копий с revit server
        /// </summary>
        /// <param name="version">2019 or 2021</param>
        public CreatorRevitLocalCopy(int version)
        {
            if (version == 2019)
                _revitServerToolCommand = @"C:\Program Files\Autodesk\Revit 2019\RevitServerToolCommand";

            else if (version == 2021)
            {
                _revitServerToolCommand = @"C:\Program Files\Autodesk\Revit 2021\RevitServerToolCommand";
            }

        }

        private string _revitServerToolCommand;
        /// <summary>
        /// Sample "192.168.10.205"
        /// </summary>
        public string ServerPath { get; set; }
        /// <summary>
        /// A path that specifies the location and file name of the new model. Absolute, relative, and UNC paths are supported. By default, the new model is created in the subfolder "RevitServerTool" under the user's Documents folder. For example, C:\\Users|username\Document\RevitServerTool\modelname.rvt
        /// Note: If the new model name is not specified in the destination path, the name of the server-based central model is used by default.
        /// </summary>
        public Func<string,string> Destination { get; set; }

        public bool Overwrite { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelsToUpdate"> A model path relative to the Revit server project directory (for example: "Hospital \North Wing.rvt"). This is a required argument.</param>
        /// <returns></returns>
        private string CreateBatFile(IEnumerable<string> modelsToUpdate)
        {
            var sb = new StringBuilder();
            sb.Append($"cd {_revitServerToolCommand}");
            sb.AppendLine("chcp 65001");
            var format = $@"RevitServerTool createLocalRvt ""{{0}}"" -s {ServerPath} {{1}} {(Overwrite?"-o":"")}";
            foreach (var modelPath in modelsToUpdate)
            {
                
                sb.AppendFormat(format, modelPath, (Destination is null ? "" : "-d "+"\""+Destination(modelPath)+"\""));
                sb.AppendLine();
            }
            sb.AppendLine("pause");
            var tempBatFile = Path.GetTempFileName()+".bat";
            File.WriteAllText(tempBatFile, sb.ToString(), Encoding.UTF8);
            return tempBatFile;
        }

        private void RunBatFile(string path)
        {
            var p = Process.Start(path);
        }

        public bool CreateLocalCopy(IEnumerable<string> modelsToUpdate)
        {
            var result = true;
            try
            {
                var batFile = CreateBatFile(modelsToUpdate);
                RunBatFile(batFile);
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
    }
}
