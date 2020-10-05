using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace RevitParametersCodeGenerator
{
    
    public  class Program
    {
        [STAThread]
        public static void Main()
        {
            var file = OpenFile();
            if (!File.Exists(file))
            {
                MessageBox.Show("File is not exist");
                return;
            }
            var fileGenerator = new SharedParametersFileGenerator();
            
            var sharedParametersFile = fileGenerator.GetClassFile(file);
            SaveFile(sharedParametersFile);

        }

        private static string OpenFile()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt";
            string filePath = null;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            { 
                filePath = openFileDialog.FileName;
            }
            return filePath;
        }
        private static void SaveFile(string text)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "SharedParameterFile"; // Default file name
            dlg.DefaultExt = ".cs"; // Default file extension
            dlg.Filter = "ClassFile (.cs)|*.cs"; // Filter files by extension

            // Show save file dialog box
            var result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                var filename = dlg.FileName;
                File.WriteAllText(filename,text );
            }
            
        }
    }
}
