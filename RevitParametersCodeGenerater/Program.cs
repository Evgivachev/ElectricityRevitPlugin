namespace RevitParametersCodeGenerator;

using System;
using System.IO;
using System.Windows.Forms;

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
        var dlg = new SaveFileDialog();
        dlg.FileName = "SharedParameterFile"; // Default file name
        dlg.DefaultExt = ".cs"; // Default file extension
        dlg.Filter = "ClassFile (.cs)|*.cs"; // Filter files by extension

        // Show save file dialog box
        var result = dlg.ShowDialog();

        // Process save file dialog box results
        if (result == DialogResult.OK)
        {
            // Save document
            var filename = dlg.FileName;
            File.WriteAllText(filename,text );
        }
            
    }
}