namespace MarkingElectricalSystems.Infrastructure;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application;
using Autodesk.Revit.DB;
using Domain;

public class ParameterSettingService(Document document) : IParameterSettingService
{
    public void SetParameters(IReadOnlyCollection<int> elements, IReadOnlyCollection<ElectricalSystem> systems)
    {
       var lines = new SortedList<string, Line>();
        var numberOfSymbols1 = 0;
        var numberOfSymbols2 = 0;
        var numberOfSymbols3 = 0;
        foreach (var system in systems)
        {
            var revitElectricalSystem = (Autodesk.Revit.DB.Electrical.ElectricalSystem)document.GetElement(new ElementId((int)system.Id));
            var id = revitElectricalSystem.UniqueId;
            var number = revitElectricalSystem.CircuitNumber;
            var numberGost = revitElectricalSystem.LookupParameter("Номер группы по ГОСТ")?.AsString()!;
            if (string.IsNullOrEmpty(numberGost))
                throw new Exception($"Пустой номер группы по ГОСТ у цепи {number}");

            var cable = revitElectricalSystem.LookupParameter("Марка кабеля").AsValueString();
            var panelName = revitElectricalSystem.BaseEquipment?.Name!;
            if (string.IsNullOrEmpty(panelName))
                panelName = "Не подключено";
            numberOfSymbols1 = Math.Max(numberOfSymbols1, numberGost.Length);
            numberOfSymbols2 = Math.Max(numberOfSymbols2, panelName.Length);
            numberOfSymbols3 = Math.Max(numberOfSymbols3, cable.Length);
            lines[numberGost] = new Line(id, number, numberGost, cable, panelName);
        }

        var k = 7 / 1000.0;
        var numberOfSymbols = new[]
        {
            numberOfSymbols1,
            numberOfSymbols2,
            numberOfSymbols3
        };
        foreach (var id in elements)
        {
            if (document.GetElement(new ElementId(id)) is not AnnotationSymbol element)
                continue;
            var circuitIdParameter = element.LookupParameter("ID цепей");
            var numbersParameter = element.LookupParameter("Номера цепей");
            var numberGostParameter = element.LookupParameter("Номера цепей по ГОСТ");
            var cableParameter = element.LookupParameter("Кабели");
            var panelParameter = element.LookupParameter("Панель");
            var width = new[]
            {
                element.LookupParameter("Ширина столбца 1"),
                element.LookupParameter("Ширина столбца 2"),
                element.LookupParameter("Ширина столбца 3")
            };
            var countOfLineParameter = element.LookupParameter("Количество строк");
            var headParameter = element.LookupParameter("Заголовок");
            var sbs = new StringBuilder[5];
            for (var i = 0; i < sbs.Length; i++)
                sbs[i] = new StringBuilder();

            foreach (var line in lines)
            {
                sbs[0].AppendLine(line.Value.Id);
                sbs[1].AppendLine(line.Value.Number);
                sbs[2].AppendLine(line.Value.NumberGost);
                sbs[3].AppendLine(line.Value.Cable);
                sbs[4].AppendLine(line.Value.Panel);
            }

            circuitIdParameter.Set(sbs[0].ToString());
            numbersParameter.Set(sbs[1].ToString());
            numberGostParameter.Set(sbs[2].ToString());
            cableParameter.Set(sbs[3].ToString());
            panelParameter.Set(sbs[4].ToString());
            countOfLineParameter.Set(lines.Count);
            if (headParameter.AsString() == "Заголовок")
                headParameter.Set("");
            for (var i = 0; i < width.Length && i < numberOfSymbols.Length; i++)
            {
                var value = numberOfSymbols[i] * k;

                //Увеличить ширину второго столбца
                if (i == 1)
                    value += 2.0 / 1000 * numberOfSymbols[i];
                width[i].Set(value);
            }
        }
    }
    
    public void SetParameters(Document doc, IEnumerable<AnnotationSymbol> annotations)
    {
        var deletedElements = new List<ElementId>();
        using var tr = new Transaction(doc);
        tr.Start("Установка параметров");
        foreach (var an in annotations)
        {
            var systems = an.LookupParameter("ID цепей")
                    .AsString()
                    .Split("\n\r".ToCharArray())
                    .Select(id => doc.GetElement(id) as Autodesk.Revit.DB.Electrical.ElectricalSystem)
                    .Where(es => es != null)
                    .Select(s => new ElectricalSystem()
                    {
                        Id = s!.Id.IntegerValue,
                        CategoryId = s.Category.Id.IntegerValue,
                    })
                    .ToArray()
                ;
            if (systems.Length == 0)
            {
                deletedElements.Add(an.Id);
                continue;
            }

            SetParameters(new[] { an.Id.IntegerValue }, systems);
        }

        doc.Delete(deletedElements);
        tr.Commit();
    }

    private record Line(string Id, string Number, string NumberGost, string Cable, string Panel);
}
