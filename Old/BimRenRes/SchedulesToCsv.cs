using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BimRenRes;

[Regeneration(RegenerationOption.Manual)]
[Transaction(TransactionMode.Manual)]
public class SchedulesToCsv :DefaultExternalCommand
{
    protected override Result DoWork(ref string message, ElementSet elements)
    {
            var result = Result.Failed;
            try
            {
                var selectedIds = UiDoc.Selection.GetElementIds();
                if (selectedIds is null || selectedIds.Count == 0)
                    return result;
                var views = selectedIds
                    .Select(x => Doc.GetElement(x))
                    .OfType<ViewSchedule>()
                    .OrderBy(x => x.Name);

                var path = Path.GetTempPath();

                var viewOptions = new ViewScheduleExportOptions
                {
                    ColumnHeaders = ExportColumnHeaders.None,
                    FieldDelimiter = CultureInfo.CurrentUICulture.TextInfo.ListSeparator,
                    //группировка по группам и заголовок
                    HeadersFootersBlanks = true,
                    TextQualifier = ExportTextQualifier.None,
                    Title = false
                };

                var generalSpec = new List<Tuple<string, string>>();

                foreach (var schedule in views)
                {
                    if (string.IsNullOrEmpty(schedule.Name))
                    {
                        throw new NullReferenceException("name is null");
                    }
                    schedule.Export(path, schedule.Name, viewOptions);
                    var name = path + schedule.Name;
                    if (!File.Exists(name))
                        continue;
                    var text = File.ReadAllText(name);
                    generalSpec.Add(Tuple.Create(schedule.Name, text));
                }


                if (generalSpec.Count == 0)
                    return result;

                var saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    //if ((myStream = saveFileDialog1.OpenFile()) != null)
                    //{
                    //    // Code to write the stream goes here.
                    //    myStream.Close();
                    //}
                    using (var sw = new StreamWriter(saveFileDialog1.FileName))
                    {
                        foreach (var spec in generalSpec)
                        {
                            sw.WriteLine(spec.Item1);
                            sw.WriteLine(spec.Item2);
                        }
                    }
                }
                //var array = views.ToArray();
                if (File.Exists(saveFileDialog1.FileName))
                    TaskDialog.Show("UnloadSpec", $"Файл сохранён {saveFileDialog1.FileName}");
                result = Result.Succeeded;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + '\n' + ex.StackTrace, "Error");
                result = Result.Failed;
            }
            finally
            {
                //resMng.ReleaseAllResources();
                //defResMng.ReleaseAllResources();
            }

            return result;



        }
}