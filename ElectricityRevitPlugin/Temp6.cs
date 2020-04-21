using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Point = Autodesk.Revit.DB.Point;
using System.Windows.Forms;
using Button = System.Windows.Forms.Button;
using Form = System.Windows.Forms.Form;
using Label = System.Windows.Forms.Label;
using Size = System.Drawing.Size;

namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Temp6 : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var doc = uiDoc.Document;
            var app = uiApp.Application;
            var result = Result.Succeeded;

            var myForm = new MyForm();
            
            myForm.document = doc;
            myForm.Show();

            return result;
        }
    }

    public class MyForm : Form
    {
        public Document document;
        private readonly Label label;
        private readonly Button button;
        public MyForm()
        {
            label = new Label { Size = new Size(ClientSize.Width, 30) };
            button = new Button
            {
                Location = new System.Drawing.Point(0, label.Bottom),
                Text = "Start",
                Size = label.Size
            };
            button.Click += MakeWork;
            Controls.Add(label);
            Controls.Add(button);
        }

        Task<string> MakeWorkInThread()
        {
            var task = new Task<string>(
                () =>
                {
                    using (var tr = new Transaction(document))
                    {
                        tr.Start("test");
                        var fi = document.GetElement(new ElementId(857197));
                        fi.Name = "999999";
                        return tr.Commit().ToString();
                    }
                }
            );
            task.Start();
            return task;
            // Вместо создания задачи, тут мог быть вызов какой-либо полезной асинхронной операции. 
            // Имена методов асинхронных операций обычно заканчиваются словом Async, 
            // например, метод ReadLineAsync у класса StreamReader.
        }
        async void MakeWork(object sender, EventArgs e)
        {
            var labelText = await MakeWorkInThread();
            label.Text = labelText;
        }
    }
}
