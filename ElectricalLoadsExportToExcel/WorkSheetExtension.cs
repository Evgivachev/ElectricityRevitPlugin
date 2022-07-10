// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* WorkSheetExtension.cs
 * https://revit-addins.blogspot.ru
 * © EvgIv, 2018
 *
 * This updater is used to create an updater capable of reacting
 * to changes in the Revit model.
 */
#region Namespaces
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Resources;
using System.Reflection;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using WPF = System.Windows;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Excel = Microsoft.Office.Interop.Excel;

#endregion

namespace ElectricalLoadsImportToExcel
{

    public static class WorkSheetExtension
    {
        public static bool Decor = true; 
        private static readonly string[] HeadStringArray = new[]
        {
            "Pу, кВт",
            "Kc",
            "cos \u03C6",
            "tg \u03C6",
            "Pр, кВт",
            "Qр, кВАр",
            "S, кВА",
            "I, А",
            "Кол-во"
        };
        public static void PutShieldName(this Excel.Worksheet worksheet, Cell startCell, string name)
        {
            worksheet.Range[startCell.ToString()].Value2 = name;
            var endCell = new Cell(startCell.Row, startCell.Column + 10);
            var excelCells = worksheet.Range[startCell.ToString(), endCell.ToString()];
            if (Decor)
            {

                //Текст по центру
                excelCells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                //Размер шрифта
                excelCells.Font.Size = 14;
                //Высота строки
                excelCells.RowHeight = 25;
                //Обводка
                excelCells.Borders.Weight = Excel.XlBorderWeight.xlMedium;
                //excelCells.Name = name;
                // Производим объединение
                excelCells.Merge(Type.Missing);
                //worksheet.Cells[1, 8] = "Общие";
            }

            startCell.Row++;

        }
        public static void PutHead(this Excel.Worksheet worksheet, Cell startCell)
        {
            var endCell = new Cell(startCell.Row, startCell.Column + 10);
            var excelCells = worksheet.Range[startCell.ToString(), endCell.ToString()];
            if (Decor)
            {
                //Текст по центру
                excelCells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                //Размер шрифта
                excelCells.Font.Size = 12;
                //Высота строки
                excelCells.RowHeight = 20;
                //Обводка
                excelCells.Borders.Weight = Excel.XlBorderWeight.xlMedium;
            }

            startCell.Column++;
            startCell.Column++;
            foreach (var s in HeadStringArray)
            {
                worksheet.Range[startCell.ToString()].Value2 = s;
                startCell.Column++;
            }

            startCell.Column = 1;
            startCell.Row++;

        }

        private delegate void Format();

        public static void PutLoads(this Excel.Worksheet worksheet, Cell startCell, IEnumerable<Load> loads, int phaseCount=1)
        {
           
            void SetFormat(Cell currentCell, object value, int columnWidth = 10)
            {
                var currentExcelCell = worksheet.Range[currentCell.ToString()];
                if (!string.IsNullOrEmpty(value?.ToString()))
                {
                    if (value.ToString()[0] == '=')
                        currentExcelCell.Formula = value;
                    else
                        currentExcelCell.Value2 = value;
                }

                if (Decor)
                {

                    //Текст по центру
                    currentExcelCell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    //Ширина столбца
                    currentExcelCell.ColumnWidth = columnWidth;
                    //Границы
                    currentExcelCell.Borders.Weight = Excel.XlBorderWeight.xlThin;
                }

            }
            var startRow = startCell.Row;
            //Cell numberCell, nameCell, ksCell, pyCell, cosPhiCell, tanPhiCell, ppCell, qpCell, sCell, iCell, countCell;
            var i = 1;
            foreach (var load in loads)
            {
                var values = new Object[]
                {
                    i.ToString(),
                    load.Classification,
                    load.P,
                    load.Ks,
                    load.CosPhi,
                    $"=TAN(ACOS({new Cell(startCell.Row, 5)}))",
                    $"={new Cell(startCell.Row, 3)}*{new Cell(startCell.Row, 4)}",
                    $"={new Cell(startCell.Row, 7)}*{new Cell(startCell.Row, 6)}",
                    $"={new Cell(startCell.Row, 7)}/{new Cell(startCell.Row, 5)}",
                    null,
                    load.Count
                };
                for (var j = 0; j < 11; j++)
                {
                    var cell = new Cell(startCell.Row, j+1);
                    if(j==1)
                        SetFormat(cell, values[j],35);
                    else
                    SetFormat(cell,values[j]);
                }

                #region MyRegion
                //numberCell = new Cell(startCell.Row, 1);
                //nameCell = new Cell(startCell.Row, 2);
                //pyCell = new Cell(startCell.Row, 3);
                //ksCell = new Cell(startCell.Row, 4);
                //cosPhiCell = new Cell(startCell.Row, 5);
                //tanPhiCell = new Cell(startCell.Row, 6);
                //ppCell = new Cell(startCell.Row, 7);
                //qpCell = new Cell(startCell.Row, 8);
                //sCell = new Cell(startCell.Row, 9);
                //iCell = new Cell(startCell.Row, 10);
                //countCell = new Cell(startCell.Row, 11);


                ////1
                //var currentExcelCell = worksheet.Range[numberCell.ToString()];
                //currentExcelCell.Value2 = i.ToString();

                ////Классификая нагрузки
                //currentExcelCell = worksheet.Range[nameCell.ToString()];
                //currentExcelCell.Value2 = load.Classification;
                ////Текст по центру
                //currentExcelCell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                ////Ширина столбца
                //currentExcelCell.ColumnWidth = 35;
                ////Границы
                //currentExcelCell.Borders.Weight = Excel.XlBorderWeight.xlThin;
                ////P
                //currentExcelCell = worksheet.Range[pyCell.ToString()];
                //currentExcelCell.Value2 = load.P / 1000;
                ////Kc
                //worksheet.Range[ksCell.ToString()].Value2 = load.Ks;
                ////cos phi
                //worksheet.Range[cosPhiCell.ToString()].Value2 = load.CosPhi;
                ////tan phi
                //worksheet.Range[tanPhiCell.ToString()].Formula = $"=TAN(ACOS({cosPhiCell}))";
                ////Pp
                //worksheet.Range[ppCell.ToString()].Formula = $"={ksCell}*{pyCell}";
                ////Q
                //worksheet.Range[qpCell.ToString()].Formula = $"={ppCell}*{tanPhiCell}";
                ////S
                //worksheet.Range[sCell.ToString()].Formula = $"={ppCell}/{cosPhiCell}";
                ////I
                //// worksheet.Range[startCell.ToString()].Formula = $"={ppCell}/{cosPhiCell}";
                ////N
                //worksheet.Range[countCell.ToString()].Value2 = load.Count;


                #endregion

                i++;
                startCell.Row++;
            }

            var currentI = phaseCount == 1 ? "4.55" : "1.52";
            //Итого по щиту
            var  valuesResult = new Object[]
            {
               null,
                "Итого по щиту",
                $"=SUM({new Cell(startRow, 3)}:{new Cell(startCell.Row - 1, 3)})",
                $"={new Cell(startCell.Row, 7)}/{new Cell(startCell.Row, 3)}",
                $"=COS(ATAN({new Cell(startCell.Row, 6)}))",
                $"={new Cell(startCell.Row, 8)}/{new Cell(startCell.Row, 7)}",
                $"=SUM({new Cell(startRow, 7)}:{new Cell(startCell.Row - 1, 7)})",
                $"=SUM({new Cell(startRow, 8)}:{new Cell(startCell.Row - 1, 8)})",
                $"=SQRT(({new Cell(startCell.Row, 7)}*{new Cell(startCell.Row, 7)})+({new Cell(startCell.Row, 8)}*{new Cell(startCell.Row, 8)}))",
                $"={new Cell(startCell.Row, 9)}*{currentI}",
                null
            };
            for (var j = 0; j < 11; j++)
            {
                var cell = new Cell(startCell.Row, j + 1);
                if (j == 1)
                    SetFormat(cell, valuesResult[j], 35);
                else
                    SetFormat(cell, valuesResult[j]);
            }

            //
            {
                /*
                 {
                numberCell = new Cell(startCell.Row, 1);
                  nameCell = new Cell(startCell.Row, 2);
                  pyCell = new Cell(startCell.Row, 3);
                  ksCell = new Cell(startCell.Row, 4);
                  cosPhiCell = new Cell(startCell.Row, 5);
                  tanPhiCell = new Cell(startCell.Row, 6);
                  ppCell = new Cell(startCell.Row, 7);
                  qpCell = new Cell(startCell.Row, 8);
                  sCell = new Cell(startCell.Row, 9);
                  iCell = new Cell(startCell.Row, 10);
                  //Классификая нагрузки
                  worksheet.Range[nameCell.ToString()].Value2 = "Итого по щиту";
                  //P
                  worksheet.Range[pyCell.ToString()].Formula = $"=SUM({new Cell(startRow, pyCell.Colomn)}:{new Cell(pyCell.Row - 1, pyCell.Colomn)})";
                  //Kc
                  worksheet.Range[ksCell.ToString()].Formula = $"={ppCell}/{pyCell}";
                  //cos phi
                  worksheet.Range[cosPhiCell.ToString()].Formula = $"=COS(ATAN({tanPhiCell}))";
                  //tan phi
                  worksheet.Range[tanPhiCell.ToString()].Formula = $"={qpCell}/{ppCell}";
                  //Pp
                  worksheet.Range[ppCell.ToString()].Formula = $"=SUM({new Cell(startRow, ppCell.Colomn)}:{new Cell(ppCell.Row - 1, ppCell.Colomn)})";
                  //Q
                  worksheet.Range[qpCell.ToString()].Formula = $"=SUM({new Cell(startRow, qpCell.Colomn)}:{new Cell(qpCell.Row - 1, qpCell.Colomn)})";
                  //S
                  worksheet.Range[sCell.ToString()].Formula = $"=SQRT(({ppCell}*{ppCell})+({qpCell}*{qpCell}))";
                  //I
                  worksheet.Range[iCell.ToString()].Formula = $"={sCell}*1.52";
                }
                */
            }


            startCell.Row++;



        }
    }
}
