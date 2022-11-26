#region Namespaces

#endregion


namespace ElectricalLoadsExportToExcel

{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;

    public sealed partial class ExternalCommand
    {
        private void DoWork(ExternalCommandData commandData, ICollection<string> shieldList)
        {
            #region MyRegion

            if (null == commandData) throw new ArgumentNullException(nameof(commandData));
            var uiApp = commandData.Application;
            var uiDoc = uiApp?.ActiveUIDocument;
            var app = uiApp?.Application;
            var doc = uiDoc?.Document;

            #endregion

            using var tr = new Transaction(doc, "trName");
            if (TransactionStatus.Started != tr.Start())
                return;
            var allShieldsList = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .ToList();
            var names = allShieldsList.Select(x => x.Name).OrderBy(x => x).ToList();
            var allShields = new Dictionary<string, FamilyInstance>();
            var message = new HashSet<string>();
            foreach (var familyInstance in allShieldsList)
            {
                if (allShields.ContainsKey(familyInstance.Name))
                    message.Add($"Несколько щитов с именем \"{familyInstance.Name}\"");
                allShields[familyInstance.Name] = familyInstance;
            }

            var messageString = string.Join("\n", message);
            if (!string.IsNullOrEmpty(messageString))
                MessageBox.Show(messageString);
            var baseShields = allShieldsList
                .Where(x => shieldList.Contains(x.UniqueId))
                .Where(x =>
                {
                    var name = x.Name;
                    var uString = x.LookupParameter("Напряжение в щите").AsValueString().Split(' ')[0];
                    //if (double.TryParse(uString, out var u)&&u<100 || uString=="0") return false;
                    //if (double.TryParse(uString, out var u) && u < 200 ) return false;
                    var flag = x.MEPModel?
                        .GetElectricalSystems()?
                        .Any();
                    if (flag is null || !flag.Value) return false;
                    var parentShieldName = x.LookupParameter("Питание от")?.AsString();
                    var flag1 = string.IsNullOrEmpty(parentShieldName);
                    if (!flag1)
                    {
                        var parentShield = allShields[parentShieldName];
                        flag1 = !shieldList.Contains(parentShield.UniqueId);
                    }

                    return flag1;
                })
                .ToArray();
            if (baseShields.Length == 0)
            {
                TaskDialog.Show("Error", "Не найдено вводных щитов");
                return;
            }

            var mainDialog = new TaskDialog("Info");
            mainDialog.MainInstruction = "Оформлять листы в Excel?";
            mainDialog.MainContent =
                $"Оформление листов в Excel достаточно ресурсоёмкий процесс, для выполнения которого потребуется некоторое время";

            // Add commmandLink options to task dialog
            mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1,
                "Выполнить оформление листов в Excel");
            mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2,
                "Не оформлять листы в Excel");

            // Set common buttons and default button. If no CommonButton or CommandLink is added,
            // task dialog will show a Close button by default
            mainDialog.CommonButtons = TaskDialogCommonButtons.Close;
            mainDialog.DefaultButton = TaskDialogResult.Close;

            // Set footer text. Footer text is usually used to link to the help document.
            mainDialog.FooterText =
                "<a href=\"http://usa.autodesk.com/adsk/servlet/index?siteID=123112&id=2484975 \">"
                + "Click here for the Revit API Developer Center</a>";
            var tResult = mainDialog.Show();
            WorkSheetExtension.Decor = tResult switch
            {
                // If the user clicks the first command link, a simple Task Dialog 
                // with only a Close button shows information about the Revit installation. 
                // If the user clicks the second command link, a simple Task Dialog 
                // created by static method shows information about the active document
                TaskDialogResult.CommandLink1 => true,
                TaskDialogResult.CommandLink2 => false,
                _ => WorkSheetExtension.Decor
            };
            var graph = new Graph();
            graph.MakeGraph(baseShields);
            var q = graph.Print();
            graph.UpdateGraph();
            var q1 = graph.Print();
            //считать файл Excel
            var openFile = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Excel Files|*.xls;*.xlsx;*.xlsm"
            };
            openFile.ShowDialog();
            if (openFile.FileName is null or "")
                return;
            var excelFile = ExcelClass.ReadExcelFile(openFile.FileName);
            //обработать наш граф
            graph.UpdateGraphFromExcelFile(excelFile);
            var q3 = graph.Print();
            graph.ExportToExcel(openFile.FileName);
            TaskDialog.Show("Info", "Успешно!");
            tr.RollBack();
        }
    }
}
