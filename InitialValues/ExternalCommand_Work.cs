#region Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows.Documents;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using Form = Autodesk.Revit.DB.Form;

#endregion


namespace InitialValues
{
    public sealed partial class ExternalCommand
    {
        private readonly Dictionary<string, Action<Parameter>> _changeParametersDict = new Dictionary<string, Action<Parameter>>()
        {
            {
                "Раздел проектирования", x =>
                {
                    if (CheckParameter(x))
                        x.Set("ЭОМ@");
                }
            },
            {
                "Кол-во кабелей (провод) в одной группе", x =>
                {
                    if (CheckParameter(x))
                        x.Set(1.0);
                }
            },
            {
                "Количество НП", x =>
                {
                    if (CheckParameter(x))
                        x.Set(1.0);
                }
            },
            {
                "Третья гармоника", x =>
                {
                    if (CheckParameter(x))
                        x.Set(0.0);
                }
            },
            {
                "Содержание третьей гармоники, %", x =>
                {
                    if (CheckParameter(x))
                        x.Set(0.0);
                }
            },
            {
                "Поправочной коэф. для групп кабелей", x =>
                {
                    if (CheckParameter(x))
                        x.Set(1.0);
                }
            },
        };

        InitialValues formInitialValues = new InitialValues();

        private bool DoWork(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            #region MyRegion

            if (null == commandData) throw new ArgumentNullException(nameof(commandData));
            if (null == message) throw new ArgumentNullException(nameof(message));
            if (null == elements) throw new ArgumentNullException(nameof(elements));
            var uiApp = commandData.Application;
            var uiDoc = uiApp?.ActiveUIDocument;
            var app = uiApp?.Application;
            var doc = uiDoc?.Document;

            #endregion

            try
            {
                using (var tr = new Transaction(doc, "trName"))
                {
                    if (TransactionStatus.Started == tr.Start())
                    {
                        //Все цепи
                        var arrayOfCircuits = new FilteredElementCollector(doc)
                            .OfCategory(BuiltInCategory.OST_ElectricalCircuit)
                            .OfClass(typeof(ElectricalSystem))
                            .ToElements()
                            .ToArray();
                        var shields = new FilteredElementCollector(doc)
                            .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                            .OfClass(typeof(FamilyInstance))
                            .Cast<FamilyInstance>()
                            //.Where(x=>x.LookupParameter("Напряжение в щите")?.AsDouble()>0)
                            .ToArray();
                        var namesOfshields = shields.Select(x => x.Name).ToArray();
                        var circuitsFromShields = shields.SelectMany(
                            x =>
                            {
                                var q = x?
                                    .MEPModel?
                                    .GetAssignedElectricalSystems()?
                                    .Cast<ElectricalSystem>();
                                if (q is null) return new List<ElectricalSystem>();
                                return q;
                            }
                        ).ToArray();

                        #region lists

                        var device1Scheldule = new FilteredElementCollector(doc)
                            .OfClass(typeof(ViewSchedule))
                            .First(x => x.Name.Equals("* Отключающее устройство 1")) as ViewSchedule;
                        if (device1Scheldule == null) throw new Exception("Не удалось найти вид ОУ1");
                        var device1Array = new FilteredElementCollector(doc, device1Scheldule.Id).OrderBy(x => x.Name).ToArray();
                        formInitialValues.Device1comboBox.DataSource = device1Array;
                        formInitialValues.Device1comboBox.DisplayMember = "Name";
                        var device2Schedule = new FilteredElementCollector(doc)
                            .OfClass(typeof(ViewSchedule))
                            .First(x => x.Name.Equals("* Отключающее устройство 2")) as ViewSchedule;
                        if (device2Schedule == null) throw new Exception("Не удалось найти вид ОУ2");
                        var device2Array = new FilteredElementCollector(doc, device2Schedule.Id).OrderBy(x => x.Name).ToArray();
                        formInitialValues.Device2comboBox.DataSource = device2Array;
                        formInitialValues.Device2comboBox.DisplayMember = "Name";
                        var сablesSchedule = new FilteredElementCollector(doc)
                            .OfClass(typeof(ViewSchedule))
                            .First(x => x.Name.Equals("* Кабели")) as ViewSchedule;
                        if (сablesSchedule == null) throw new Exception("Не удалось найти вид Кабели");
                        var cablesArray = new FilteredElementCollector(doc, сablesSchedule.Id).OrderBy(x => x.Name).ToArray();
                        formInitialValues.CablesComboBox.DataSource = cablesArray;
                        formInitialValues.CablesComboBox.DisplayMember = "Name";
                        var tubeSchedule = new FilteredElementCollector(doc)
                            .OfClass(typeof(ViewSchedule))
                            .First(x => x.Name.Equals("* Трубы")) as ViewSchedule;
                        if (tubeSchedule == null) throw new Exception("Не удалось найти вид Трубы");
                        var tubeArray = new FilteredElementCollector(doc, tubeSchedule.Id).OrderBy(x => x.Name).ToArray();
                        formInitialValues.TubeComboBox.DataSource = tubeArray;
                        formInitialValues.TubeComboBox.DisplayMember = "Name";
                        var installSchedule = new FilteredElementCollector(doc)
                            .OfClass(typeof(ViewSchedule))
                            .First(x => x.Name.Equals("* Монтаж")) as ViewSchedule;
                        if (installSchedule == null) throw new Exception("Не удалось найти вид Монтаж");
                        var installArray = new FilteredElementCollector(doc, installSchedule.Id).OrderBy(x => x.Name).ToArray();
                        formInitialValues.InstallationСomboBox.DataSource = installArray;
                        formInitialValues.InstallationСomboBox.DisplayMember = "Name";
                        var temperatureSchedule = new FilteredElementCollector(doc)
                                .OfClass(typeof(ViewSchedule))
                                .First(x => x.Name.Equals(
                                    "* Поправочные коэффициенты для определения допустимых токовых нагрузок кабелей, проложенных в воздухе при температуре окружающей среды, отличной от 30 °С"))
                            as ViewSchedule;
                        if (temperatureSchedule == null) throw new Exception("Не удалось найти вид Попр.коэф.");
                        var temperatureArray = new FilteredElementCollector(doc, temperatureSchedule.Id).OrderBy(x => x.Name).ToArray();
                        formInitialValues.TemperatureComboBox.DataSource = temperatureArray;
                        formInitialValues.TemperatureComboBox.DisplayMember = "Name";

                        #endregion

                        formInitialValues.OkButton.Click += (sender, args) =>
                        {
                            SetValues(shields);
                            formInitialValues.Close();
                        };
                        formInitialValues.UpdateButton.Click += (sender, args) =>
                        {
                            var updateForm = new UpdateForm();

                            //updateForm.ShieldsCheckedListBox.DataSource = shields;
                            //updateForm.ShieldsCheckedListBox.DisplayMember = "Name";
                            var shieldsDictionary = shields.ToDictionary(x => x.UniqueId);
                            var q = shields.GroupBy(x =>
                            {
                                var index = x.Name.IndexOfAny(new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
                                var subName = x.Name.Substring(0, index > 0 ? index : x.Name.Length - 1);
                                return subName;
                            });
                            foreach (var pair in q)
                            {
                                var newNode = updateForm.shieldsTreeView.Nodes.Add(pair.Key);
                                foreach (var instance in pair)
                                {
                                    newNode.Nodes.Add(instance.UniqueId, instance.Name);
                                }
                            }

                            updateForm.comboBox1.DataSource = installArray;
                            updateForm.comboBox1.DisplayMember = "Name";
                            updateForm.RefreshButton.Click += (o, eventArgs) =>
                            {
                                var install = updateForm.comboBox1.SelectedItem as Element;
                                var shieldsList = new List<FamilyInstance>();
                                foreach (TreeNode node in updateForm.shieldsTreeView.Nodes)
                                {
                                    foreach (TreeNode sheetNode in node.Nodes)
                                    {
                                        var name = sheetNode.Name;
                                        if (sheetNode.Checked) shieldsList.Add(shieldsDictionary[sheetNode.Name]);
                                    }
                                }

                                UpdateShields(shieldsList, updateForm.Flags, install);
                                updateForm.Close();
                            };
                            updateForm.ShowDialog();
                            formInitialValues.Close();
                        };
                        Application.Run(formInitialValues);
                        return TransactionStatus.Committed == tr.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                /* TODO: Handle the exception here if you need 
                 * or throw the exception again if you need. */
                throw ex;
            }
            finally
            {
            }

            return false;
        }


        private void SetValues(IEnumerable<FamilyInstance> shields)
        {
            var shieldPhase = new PhaseNumber();
            foreach (var shield in shields)
            {
                shieldPhase.Flag = false;
                //Напряжение в щите
                //if 
                var u = 380;
                int.TryParse(shield.LookupParameter("Напряжение в щите")?.AsValueString().Split(' ')[0], out u);
                //||
                //u == 0);
                //continue;
                var circuits = shield
                    .MEPModel?
                    .GetAssignedElectricalSystems()?
                    .Cast<ElectricalSystem>()
                    .OrderBy(x => x.Name.Split(',')[0].Length)
                    .ThenBy(x => x.Name).ToArray();
                if (circuits is null) continue;
                var prefix = shield.LookupParameter("Префикс цепи").AsString();
                var number = 1;
                var circuitPhase = new PhaseNumber();
                foreach (var circuit in circuits)
                {
                    foreach (var pair in _changeParametersDict)
                    {
                        if (circuit.LookupParameter(pair.Key) is null) continue;
                        pair.Value(circuit.LookupParameter(pair.Key));
                    }

                    var param = circuit.LookupParameter("Номер группы по ГОСТ");
                    if (CheckParameter(param)) param.Set($"{prefix}.{number}");
                    var numberQFParam = circuit.LookupParameter("Номер QF");
                    if (CheckParameter(numberQFParam)) numberQFParam.Set($"QF{number}");
                    number++;
                    var phaseParameter = circuit.LookupParameter("Фаза");
                    if (CheckParameter(phaseParameter))
                    {
                        if (u == 380)
                        {
                            if (circuit.LookupParameter("Номер цепи").AsString().Split(',').Length > 1)
                            {
                                phaseParameter.Set("L1,L2,L3");
                            }
                            else
                            {
                                phaseParameter.Set($"L{circuitPhase.Get()}");
                                circuitPhase.Up();
                            }
                        }
                        else
                        {
                            phaseParameter.Set($"L{shieldPhase.Get()}");
                            shieldPhase.Flag = true;
                        }
                    }

                    SetValuesDevice(circuit, formInitialValues.checkBox1.Checked);
                }

                if (shieldPhase.Flag) shieldPhase.Up();
            }
        }

        private void SetValuesDevice(ElectricalSystem circuit, bool flag)
        {
            var parameters = new[]
            {
                circuit.LookupParameter("Отключающее устройство 1"),
                circuit.LookupParameter("Отключающее устройство 2"),
                circuit.LookupParameter("Марка кабеля"),
                circuit.LookupParameter("Обозначение трубы"),
                circuit.LookupParameter("Способ монтажа по ГОСТ"),
                circuit.LookupParameter("Поправочные коэфф для категорий кроме D"),
            };
            var initialValues = new[]
            {
                (formInitialValues.Device1comboBox.SelectedItem as Element)?.Id,
                (formInitialValues.Device2comboBox.SelectedItem as Element)?.Id,
                (formInitialValues.CablesComboBox.SelectedItem as Element)?.Id,
                (formInitialValues.TubeComboBox.SelectedItem as Element)?.Id,
                (formInitialValues.InstallationСomboBox.SelectedItem as Element)?.Id,
                (formInitialValues.TemperatureComboBox.SelectedItem as Element)?.Id
            };
            var shieldName = circuit.LookupParameter("Панель").AsString();
            ElementId?[] values1;
            if (shieldName.Contains("ЩО-"))
            {
                values1 = new ElementId?[]
                {
                    (formInitialValues.Device1comboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("S201 C10")))?.Id,
                    (formInitialValues.Device2comboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("-"))?.Id),
                    (formInitialValues.CablesComboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("ВВГнг(А)-LSLTx 3x1.5"))?.Id),
                    (formInitialValues.TubeComboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("ПВХ 20 скрыто"))?.Id),
                    (formInitialValues.InstallationСomboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("A2"))?.Id),
                    (formInitialValues.TemperatureComboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("30"))?.Id),
                };
            }
            else if (shieldName.Contains("ЩОA-") || shieldName.Contains("ЩАО-"))
            {
                values1 = new ElementId?[]
                {
                    (formInitialValues.Device1comboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("S201 C10")))?.Id,
                    (formInitialValues.Device2comboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("-"))?.Id),
                    (formInitialValues.CablesComboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("ВВГнг(А)-FRLSLTx 3x1.5"))
                        ?.Id),
                    (formInitialValues.TubeComboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("ОКЛ FRLine 25 скрыто"))?.Id),
                    (formInitialValues.InstallationСomboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("A2"))?.Id),
                    (formInitialValues.TemperatureComboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("30"))?.Id),
                };
            }
            else if (shieldName.Contains("РТ-"))
            {
                values1 = new ElementId?[]
                {
                    (formInitialValues.Device1comboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("S202 C10")))?.Id,
                    (formInitialValues.Device2comboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("-"))?.Id),
                    (formInitialValues.CablesComboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("ВВГнг(А)-FRLSLTx 2x1.5"))
                        ?.Id),
                    (formInitialValues.TubeComboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("ОКЛ FRLine 25 скрыто"))?.Id),
                    (formInitialValues.InstallationСomboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("A2"))?.Id),
                    (formInitialValues.TemperatureComboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("30"))?.Id),
                };
            }
            else if (shieldName.Contains("ЩC-") || shieldName.Contains("ЩС-"))
            {
                values1 = new ElementId?[]
                {
                    (formInitialValues.Device1comboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("DS201 C16 A30, 30 мА")))
                    ?.Id,
                    (formInitialValues.Device2comboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("-"))?.Id),
                    (formInitialValues.CablesComboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("ВВГнг(А)-LSLTx 3x2.5"))?.Id),
                    (formInitialValues.TubeComboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("ПВХ 20 скрыто"))?.Id),
                    (formInitialValues.InstallationСomboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("A2"))?.Id),
                    (formInitialValues.TemperatureComboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("30"))?.Id),
                };
            }
            else if (shieldName.Contains("ЩС-В"))
            {
                values1 = new ElementId?[]
                {
                    (formInitialValues.Device1comboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("S203 C16")))?.Id,
                    (formInitialValues.Device2comboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("-"))?.Id),
                    (formInitialValues.CablesComboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("ВВГнг(А)-LSLTx 5x2.5"))?.Id),
                    (formInitialValues.TubeComboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("ПВХ 20 скрыто"))?.Id),
                    (formInitialValues.InstallationСomboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("E"))?.Id),
                    (formInitialValues.TemperatureComboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("30"))?.Id),
                };
            }
            else
            {
                values1 = new ElementId?[]
                {
                    (formInitialValues.Device1comboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("DS201 C16 A30, 30 мА")))
                    ?.Id,
                    (formInitialValues.Device2comboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("-"))?.Id),
                    formInitialValues.CablesComboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("ВВГнг(А)-LSLTx 3x2.5"))?.Id,
                    (formInitialValues.TubeComboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("ПВХ 20 скрыто"))?.Id),
                    (formInitialValues.InstallationСomboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("A2"))?.Id),
                    (formInitialValues.TemperatureComboBox.Items.Cast<Element>().FirstOrDefault(x => x.Name.Equals("30"))?.Id),
                };
            }

            for (var i = 0; i < parameters.Length; i++)
            {
                if (CheckParameter(parameters[i]))
                {
                    try
                    {
                        parameters[i].Set(!formInitialValues.flags[i] ? initialValues[i] : values1[i]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        private static bool CheckParameter(Parameter parameter)
        {
            if (parameter.StorageType is StorageType.String)
            {
                return string.IsNullOrEmpty(parameter.AsString());
            }
            else if (parameter.StorageType is StorageType.ElementId)
            {
                var flag1 = parameter.AsElementId();
                var flag2 = parameter.AsElementId().ToString();
                var flag = parameter.AsElementId() is null;
                return (parameter.AsElementId() is null || parameter.AsElementId().ToString() == "-1");
            }

            return (string.IsNullOrEmpty(parameter.AsValueString()) || parameter.AsValueString() == "0");
        }

        private void UpdateShields(IEnumerable<FamilyInstance> shields, bool[] flags, Element element)
        {
            var shieldPhase = new PhaseNumber();
            foreach (var shield in shields)
            {
                shieldPhase.Flag = false;
                //Напряжение в щите
                if (!int.TryParse(shield.LookupParameter("Напряжение в щите").AsValueString().Split(' ')[0], out var u) || u == 0) continue;
                var circuits = shield
                    .MEPModel?
                    .GetAssignedElectricalSystems()?
                    .Cast<ElectricalSystem>()
                    .OrderBy(x => x.Name.Split(',')[0].Length)
                    .ThenBy(x => x.Name).ToArray();
                if (circuits is null) continue;
                var prefix = shield.LookupParameter("Префикс цепи").AsString();
                var number = 1;
                var circuitPhase = new PhaseNumber();
                foreach (var circuit in circuits)
                {
                    if (flags[0])
                    {
                        var numberQFParam = circuit.LookupParameter("Номер QF");
                        numberQFParam.Set($"QF{number}");
                    }

                    if (flags[1])
                    {
                        var param = circuit.LookupParameter("Номер группы по ГОСТ");
                        //if (CheckParameter(param))
                        param.Set($"{prefix}.{number}");
                    }

                    number++;
                    if (flags[2])
                    {
                        var phaseParameter = circuit.LookupParameter("Фаза");
                        if (u == 380)
                        {
                            if (circuit.LookupParameter("Номер цепи").AsString().Split(',').Length > 1)
                            {
                                phaseParameter.Set("L1,L2,L3");
                            }
                            else
                            {
                                phaseParameter.Set($"L{circuitPhase.Get()}");
                                circuitPhase.Up();
                            }
                        }
                        else
                        {
                            phaseParameter.Set($"L{shieldPhase.Get()}");
                            shieldPhase.Flag = true;
                        }
                    }

                    if (flags[3])
                        circuit.LookupParameter("Способ монтажа по ГОСТ").Set(element.Id);
                }

                if (shieldPhase.Flag) shieldPhase.Up();
            }
        }
    }

    internal class PhaseNumber
    {
        private int _n;

        internal void Up()
        {
            _n++;
        }

        internal int Get()
        {
            return _n % 3 + 1;
        }

        internal bool Flag;
    }
}
