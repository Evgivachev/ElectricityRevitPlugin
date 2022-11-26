// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Excel.cs
 * https://revit-addins.blogspot.ru
 * © EvgIv, 2018
 *
 * This updater is used to create an updater capable of reacting
 * to changes in the Revit model.
 */

namespace ElectricalLoadsExportToExcel
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Autodesk.Revit.UI;
    using Microsoft.Office.Interop.Excel;
    using Application = Microsoft.Office.Interop.Excel.Application;

    public sealed partial class ExcelClass
    {
        public static Dictionary<string, List<Load>> ReadExcelFile(string file)
        {
            var result = new Dictionary<string, List<Load>>();
            var objExcel = new Application();
            try
            {
                var objWorkBook = objExcel.Workbooks.Open(
                    file,
                    0,
                    true,
                    5,
                    "",
                    "",
                    false,
                    XlPlatform.xlWindows,
                    "", true, false, 0, true, false, false);
                var objWorkSheet = (Worksheet)objWorkBook.Sheets[1];
                var sheetName = objWorkSheet.Name;
                var currentCell = new Cell(1, 1);
                var flag = true;
                while (flag)
                {
                    string nameOfShield = null;
                    //поиск ячейки щита
                    var emptyCells = 0;
                    while (flag)
                    {
                        var q2 = currentCell.ToString();
                        nameOfShield = objWorkSheet.Range[q2].Text.ToString();
                        if (IsShield(nameOfShield))
                        {
                            break;
                        }

                        if (emptyCells == 50)
                            flag = false;
                        emptyCells++;
                        currentCell.Row++;
                    }

                    if (!flag) break;
                    result[nameOfShield] = new List<Load>();

                    //поиск ячейки "итого по щиту"
                    var shieldResult = new Cell(currentCell);

                    //Поиск ячейки нагрузки
                    var loadCell = new Cell(currentCell);
                    loadCell.Column++;
                    loadCell.Row++;
                    loadCell.Row++;
                    emptyCells = 0;
                    while (!IsResult(objWorkSheet.Range[loadCell.ToString()].Text.ToString()))
                    {
                        if (emptyCells == 500)
                        {
                            TaskDialog.Show("Error", $"Много итераций {nameOfShield}");
                            break;
                        }

                        var loadName = objWorkSheet.Range[loadCell.ToString()].Text.ToString();
                        var ksCell = new Cell(loadCell.Row, loadCell.Column + 2);
                        var ksString = objWorkSheet.Range[ksCell.ToString()].Value2.ToString();
                        if (string.IsNullOrEmpty(loadName) || !double.TryParse(ksString, out var ks))
                        {
                            emptyCells++;
                            continue;
                        }

                        var load = new Load(loadName, ks);
                        result[nameOfShield].Add(load);
                        emptyCells = 0;
                        loadCell.Row++;
                    }

                    currentCell.Row = loadCell.Row + 1;
                }

                objExcel.DisplayAlerts = false;
                objExcel.Quit();
                return result;
            }
            catch (Exception e)
            {
                objExcel.Quit();
                MessageBox.Show($"{e.Message}\n{e.StackTrace}");
                throw;
            }
            finally
            {
                objExcel.Quit();
            }
        }


        private static bool IsShield(string str)
        {
            var q3 = !(int.TryParse(str, out _));
            return (str != "" && str != " " && q3);
        }

        private static bool IsResult(string str)
        {
            return (str == "Итого по щиту");
        }
    }
}
