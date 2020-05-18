using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RevitParametersCodeGenerater
{
    public  class Program
    {
        public static void Main()
        {
            var file = OpenFile();
            if (!File.Exists(file))
            {
                MessageBox.Show("File is not exist");
                return;
            }
            using (var stream = new StreamReader(file,System.Text.Encoding.Default))
            {
                var line = stream.ReadLine();


            }

            

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
    }
}
