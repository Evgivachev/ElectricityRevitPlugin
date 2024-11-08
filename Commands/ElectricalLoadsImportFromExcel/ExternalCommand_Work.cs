#region Namespaces

#endregion


namespace ElectricalLoadsImportFromExcel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;
    using Autodesk.Revit.UI;
    using Microsoft.Office.Interop.Excel;
    using Microsoft.Win32;
    using Application = Microsoft.Office.Interop.Excel.Application;

    public sealed partial class ExternalCommand
    {
        private bool DoWork(ExternalCommandData commandData)
        {
            #region MyRegion

            if (null == commandData) throw new ArgumentNullException(nameof(commandData));
            var uiApp = commandData.Application;
            var uiDoc = uiApp?.ActiveUIDocument;
            var doc = uiDoc?.Document;

            #endregion

            using var tr = new Transaction(doc, "trName");
            if (TransactionStatus.Started == tr.Start())
            {
                //словарь питающих цепей
                var powerDictionary = new Dictionary<int, ElectricalSystem>();
                var shieldsParamDictionary = GetShields();
                if (shieldsParamDictionary.Count == 0) return TransactionStatus.Committed == tr.RollBack();
                var listOfShields = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                    .OfClass(typeof(FamilyInstance))
                    .Cast<FamilyInstance>()
                    .Where(x => x.MEPModel.GetElectricalSystems() is not null).ToList();
                var namesOfParameters = new[]
                {
                    "Установленная мощность в щитах",
                    "Коэффициент спроса в щитах",
                    "Косинус в щитах",
                    "Тангенс в щитах",
                    "Активная мощность в щитах",
                    "Реактивная мощность в щитах",
                    "Полная мощность в щитах",
                    "Ток в щитах"
                };
                var counter = 0;
                foreach (var shield in listOfShields)
                {
                    //Запись в параметры щитов
                    if (!shieldsParamDictionary.ContainsKey(shield.Name)) continue;
                    for (var i = 0; i < 8; i++)
                        if (!shield.LookupParameter(namesOfParameters[i])
                                .Set(shieldsParamDictionary[shield.Name][i]))
                            MessageBox.Show($"Не удалось установить параметр \"{namesOfParameters[i]}\" в щите {shield.Name} ",
                                "Error");

                    //Запись в параметры цепи
                    //Питающая сеть
                    var powerCirсuit = shield.MEPModel
                        .GetElectricalSystems()
                        .FirstOrDefault(x => x.PanelName != shield.Name);
                    if (powerCirсuit is null)
                    {
                        MessageBox.Show($"Не удалось найти питающую цепь в щите {shield.Name} ", "Error");
                        continue;
                    }

                    if (powerDictionary.ContainsKey(powerCirсuit.Id.IntegerValue))
                        for (var i = 0; i < 8; i++)
                        {
                            var currentValue = powerCirсuit.LookupParameter(namesOfParameters[i]).AsDouble();
                            if (!powerCirсuit.LookupParameter(namesOfParameters[i])
                                    .Set(shieldsParamDictionary[shield.Name][i] + currentValue))
                                MessageBox.Show(
                                    $"Не удалось установить параметр \"{namesOfParameters[i]}\" в питающей цепи щита {shield.Name} ",
                                    "Error");

                            var Py = powerCirсuit.LookupParameter(namesOfParameters[0]).AsDouble();
                            var Pr = powerCirсuit.LookupParameter(namesOfParameters[4]).AsDouble();
                            var Q = powerCirсuit.LookupParameter(namesOfParameters[5]).AsDouble();
                            var S = powerCirсuit.LookupParameter(namesOfParameters[6]).AsDouble();
                            powerCirсuit.LookupParameter(namesOfParameters[1]).Set(Pr / Py); //коэффициент спроса
                            powerCirсuit.LookupParameter(namesOfParameters[2]).Set(Pr / S); //cos phi
                            powerCirсuit.LookupParameter(namesOfParameters[3]).Set(Q / Pr); //tan phi
                        }
                    else
                    {
                        powerDictionary[powerCirсuit.Id.IntegerValue] = powerCirсuit;
                        for (var i = 0; i < 8; i++)
                            if (!powerCirсuit.LookupParameter(namesOfParameters[i])
                                    .Set(shieldsParamDictionary[shield.Name][i]))
                                MessageBox.Show(
                                    $"Не удалось установить параметр \"{namesOfParameters[i]}\" в питающей цепи щита {shield.Name} ",
                                    "Error");
                    }

                    counter++;
                }

                TaskDialog.Show("Message", $"Количество обработанных щитов: {counter} ");
                return TransactionStatus.Committed == tr.Commit();
            }

            return false;
        }

        private bool IsShield(string str)
        {
            var q3 = !int.TryParse(str, out _);
            return str != "" && str != " " && q3;
        }

        private bool IsResult(string str)
        {
            return str == "Итого по щиту";
        }

        private Dictionary<string, double[]> GetShields()
        {
            var openFile = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Excel Files|*.xls;*.xlsx;*.xlsm"
            };
            openFile.ShowDialog();
            if (openFile.FileName == "")
                return new Dictionary<string, double[]>();
            var result = new Dictionary<string, double[]>();
            var objExcel = new Application();
            try
            {
                var objWorkBook = objExcel.Workbooks.Open(
                    openFile.FileName,
                    0,
                    true,
                    5,
                    "",
                    "",
                    false,
                    XlPlatform.xlWindows,
                    "", true, false, 0, true, false, false);
                var objWorkSheet = (Worksheet)objWorkBook.Sheets[1];
                var currentCell = new Cell(1, 1);
                var flag = true;
                while (flag)
                {
                    string? nameOfShield = null;
                    var paramOfShield = new double[8];
                    //поиск ячейки щита
                    var emptyCells = 0;
                    while (flag)
                    {
                        var q2 = currentCell.ToString();
                        nameOfShield = objWorkSheet.Range[q2].Text.ToString();
                        if (IsShield(nameOfShield))
                            break;

                        if (emptyCells == 50)
                            flag = false;
                        emptyCells++;
                        currentCell.Row++;
                    }

                    if (!flag) break;
                    //поиск ячейки "итого по щиту"
                    var shieldResult = new Cell(currentCell);
                    shieldResult.Colomn++;
                    emptyCells = 0;
                    while (flag)
                    {
                        var q1 = objWorkSheet.Range[shieldResult.ToString()].Text.ToString();
                        if (IsResult(q1)) break;
                        if (emptyCells == 500)
                        {
                            TaskDialog.Show("Error", $"Не удалось найти строку расчётов в щите {nameOfShield}");
                            flag = false;
                        }

                        emptyCells++;
                        shieldResult.Row++;
                    }

                    if (!flag) break;
                    currentCell.Row = shieldResult.Row + 1;
                    var paramOfShieldCell = new Cell(shieldResult);
                    paramOfShieldCell.Colomn++;
                    for (var j = 0; j < 8; j++)
                    {
                        string param = objWorkSheet.Range[paramOfShieldCell.ToString()].Text.ToString();
                        if (!double.TryParse(param, out paramOfShield[j]))
                            MessageBox.Show($"Проверьте формат данных в щите {nameOfShield}", "Error");

                        paramOfShieldCell.Colomn++;
                    }

                    result[nameOfShield!] = paramOfShield;
                }

                objExcel.Quit();
                return result;
            }
            catch (Exception)
            {
                objExcel.Quit();
                throw;
            }
            finally
            {
                objExcel.Quit();
            }
        }
    }
}
