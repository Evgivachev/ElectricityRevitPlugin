namespace Diagrams.ModelUpdate
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;

    internal class ModelUpdater
    {
        private readonly Document _doc;

        private FamilyInstance _head;
        private View _workView;

        private Element[] Cables;
        private Element[] Device1;
        private Element[] Device2;
        private Element[] InputDevice;
        private FamilyInstance[] Lines;

        internal ModelUpdater(Document document)
        {
            _doc = document;
        }

        private View WorkView
        {
            get => _workView ?? _doc.ActiveView;
            set => _workView = value;
        }

        private FamilyInstance Head
        {
            get
            {
                if (_head is null)
                {
                    var head = new FilteredElementCollector(_doc, WorkView.Id)
                        .OfClass(typeof(FamilyInstance))
                        .FirstOrDefault(x => x.Name == $"ЭОМ-Схемы однолинейные-Шапка (ГОСТ 2.708-81)") as FamilyInstance;
                    _head = head;
                    return _head;
                }

                return _head;
            }
        }

        internal void SetDevice1FromView(ViewSchedule view)
        {
            var elements = new FilteredElementCollector(_doc, view.Id)
                .OfType<Element>();
            Device1 = elements.ToArray();
        }

        internal void SetDevice2FromView(ViewSchedule view)
        {
            var elements = new FilteredElementCollector(_doc, view.Id)
                .OfType<Element>();
            Device2 = elements.ToArray();
        }

        internal void SetImputDeviceFromView(ViewSchedule view)
        {
            var elements = new FilteredElementCollector(_doc, view.Id)
                .OfType<Element>();
            InputDevice = elements.ToArray();
        }

        internal void SetCablesFromView(ViewSchedule view)
        {
            var elements = new FilteredElementCollector(_doc, view.Id)
                .OfType<Element>();
            Cables = elements.ToArray();
        }

        internal void UpdateModelByHead()
        {
            var shieldId = Head?.LookupParameter("ID электрического щита")?.AsString();
            if (string.IsNullOrEmpty(shieldId))
            {
                Console.WriteLine($"shieldID is null");
                return;
            }

            var shield = _doc.GetElement(shieldId) as FamilyInstance;
            if (shield is null)
            {
                Console.WriteLine($"shield is null");
                return;
            }

            var deviceFromDiagramString = Head.LookupParameter($"Тип вводного автомата").AsString();
            var device = FindElement(InputDevice, deviceFromDiagramString);
            if (device is null)
            {
                Console.WriteLine($"Не удалось найти аппарта из спецификации");
                goto UpdateCable;
            }

            using (var tr = new Transaction(_doc))
            {
                tr.Start("Смена ОУ1");
                var flag = shield.LookupParameter($"Вводное отключающее устройство")?.Set(device.Id);
                Console.WriteLine($"{flag}");
                _doc.Regenerate();
                tr.Commit();
            }

            UpdateCable:
            var cableString = Head?.LookupParameter($"Марка кабеля в щитах")?.AsString();
            if (string.IsNullOrEmpty(cableString))
            {
                return;
            }

            var prefix = shield?.LookupParameter($"Префикс цепи").AsString();
            var powerCable = shield
                .MEPModel
                .GetElectricalSystems()
                .FirstOrDefault(x => !x.Name.StartsWith(prefix));
            if (powerCable is null)
            {
                Console.WriteLine($"Не удалось найти питающий кабель щита {shield.Name}");
                return;
            }

            var powerCableFromDiagram = Head.LookupParameter("Марка кабеля в щитах")?.AsString();
            var cable = FindElement(Cables, powerCableFromDiagram);
            using (var tr = new Transaction(_doc))
            {
                tr.Start("Смена смена питающего кабеля");
                var flag = powerCable.LookupParameter($"Марка кабеля")?.Set(cable.Id);
                Console.WriteLine($"{flag}");
                _doc.Regenerate();
                tr.Commit();
            }
        }

        internal void UpdateModelByLines()
        {
            var shieldId = Head?.LookupParameter("ID электрического щита")?.AsString();
            if (string.IsNullOrEmpty(shieldId))
            {
                Console.WriteLine($"shieldID is null");
                return;
            }

            var shield = _doc.GetElement(shieldId) as FamilyInstance;
            if (shield is null)
            {
                Console.WriteLine($"shield is null");
                return;
            }

            var lines = new FilteredElementCollector(_doc, WorkView.Id)
                .OfClass(typeof(FamilyInstance))
                .Where(x => x.Name == "ЭОМ-Схемы однолинейные-Отходящая линия (ГОСТ 2.708-81)");
            //todo сделать группу транз
            foreach (var line in lines)
            {
                var electricalSystems = shield.MEPModel?
                    .GetAssignedElectricalSystems()?
                    .OfType<ElectricalSystem>();
                if (electricalSystems is null)
                {
                    Console.WriteLine($"Нет подключенных цепей к щиту {shield.Name}");
                    return;
                }

                var prefix = line.LookupParameter("Номер цепи").AsString();
                var electricalS = electricalSystems.FirstOrDefault(x => x.Name == prefix);
                if (electricalS is null)
                {
                    Console.WriteLine($"Не удалось найти цепь {prefix} в щите {shield.Name}");
                    continue;
                }

                var device1FromDiagramString = line.LookupParameter($"Отключающее устройство").AsString();
                var device1 = FindElement(Device1, device1FromDiagramString);
                if (device1 is null)
                {
                    Console.WriteLine($"Не удалось найти аппарат 1 из спецификации");
                    goto Device2;
                }

                using (var tr = new Transaction(_doc))
                {
                    tr.Start("Смена ОУ1");
                    var flag = electricalS.LookupParameter($"Отключающее устройство 1")?.Set(device1.Id);
                    Console.WriteLine($"{flag}");
                    _doc.Regenerate();
                    tr.Commit();
                }

                Device2:
                var device2FromDiagramString = line.LookupParameter($"УЗО и др. аппарат").AsString();
                var device2 = FindElement(Device2, device2FromDiagramString);
                if (device2 is null)
                {
                    Console.WriteLine($"Не удалось найти аппарат 2 из спецификации");
                    goto CableLine;
                }

                using (var tr = new Transaction(_doc))
                {
                    tr.Start("Смена ОУ2");
                    var flag = electricalS.LookupParameter($"Отключающее устройство 2")?.Set(device2.Id);
                    Console.WriteLine($"{flag}");
                    _doc.Regenerate();
                    tr.Commit();
                }

                CableLine:
                var typeOfCable = line?.LookupParameter("Марка кабеля").AsString();
                var countOfLines = line.LookupParameter("Кол-во жил").AsDouble();
                countOfLines = Math.Round(countOfLines);
                var crossSection = line.LookupParameter("Сечение кабеля").AsDouble();
                crossSection = Math.Round(crossSection, 1);
                var cableString =
                    $"{typeOfCable} {countOfLines.ToString(CultureInfo.InvariantCulture)}х{crossSection.ToString(CultureInfo.InvariantCulture)}";
                if (string.IsNullOrEmpty(cableString))
                {
                    continue;
                }

                var cable = FindElement(Cables, cableString);
                using (var tr = new Transaction(_doc))
                {
                    tr.Start("Смена смена питающего кабеля");
                    var flag = electricalS.LookupParameter($"Марка кабеля")?.Set(cable.Id);
                    Console.WriteLine($"{flag}");
                    _doc.Regenerate();
                    tr.Commit();
                }
            }
        }


        private static int LevenshteinDistance(string first, string second)
        {
            var opt = new int[first.Length + 1, second.Length + 1];
            for (var i = 0; i <= first.Length; ++i)
                opt[i, 0] = i;
            for (var i = 0; i <= second.Length; ++i)
                opt[0, i] = i;
            for (var i = 1; i <= first.Length; ++i)
            for (var j = 1; j <= second.Length; ++j)
            {
                if (first[i - 1] == second[j - 1])
                    opt[i, j] = opt[i - 1, j - 1];
                else
                    opt[i, j] = Math.Min(opt[i - 1, j], Math.Min(opt[i, j - 1], opt[i - 1, j - 1])) + 1;
            }

            return opt[first.Length, second.Length];
        }

        private static int DamerauLevenshteinDistance(string string1, string string2)
        {
            return GetDistance(string1, string2, true);
        }

        private static int GetDistance(string left, string right, bool isDamerauDistanceApplied)
        {
            if (left.Length == 0) return right.Length;
            if (right.Length == 0) return left.Length;
            var lenLeft = left.Length;
            var lenRight = right.Length;
            var matrix = new int[lenLeft + 1, lenRight + 1];
            for (var i = 0; i <= lenLeft; i++)
                matrix[i, 0] = i;
            for (var i = 0; i <= lenRight; i++)
                matrix[0, i] = i;
            for (var i = 1; i <= lenLeft; i++)
            {
                for (var j = 1; j <= lenRight; j++)
                {
                    var cost = (left[i - 1] == right[j - 1]) ? 0 : 1;
                    matrix[i, j] = Math.Min(Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1), matrix[i - 1, j - 1] + cost);
                    if (isDamerauDistanceApplied)
                    {
                        // Fixed for string base 0 index.
                        if (i > 1 && j > 1 && left[i - 1] == right[j - 2] && left[i - 2] == right[j - 1])
                        {
                            matrix[i, j] = Math.Min(matrix[i, j], matrix[i - 2, j - 2] + cost);
                        }
                    }
                }
            }

            return matrix[lenLeft, lenRight];
        }

        private Element FindElement(IEnumerable<Element> elements, string nameOfElement)
        {
            var minLev = int.MaxValue;
            Element el = null;
            foreach (var e in elements)
            {
                if (e.Name == nameOfElement)
                    return e;
                var l = DamerauLevenshteinDistance(e.Name, nameOfElement);
                if (l <= minLev)
                {
                    minLev = l;
                    el = e;
                }
            }

            //var element = elements.MinBy(x => LevenshteinDistance(x.Name, nameOfElement));
            //return element;
            return el;
        }
    }
}
