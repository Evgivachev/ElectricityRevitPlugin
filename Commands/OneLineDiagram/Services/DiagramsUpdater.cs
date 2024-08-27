namespace Diagrams.Services
{
    using System;
    using System.Linq;
    using Abstractions;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Models;

    public class DiagramsUpdater : IDiagramsUpdater
    {
        private readonly IDiagramsDrawer _diagramsDrawer;
        private readonly UIApplication _uiApplication;

        public DiagramsUpdater(UIApplication uiApplication, IDiagramsDrawer diagramsDrawer)
        {
            _uiApplication = uiApplication;
            _diagramsDrawer = diagramsDrawer;
        }

        public void UpdateDiagram(View view)
        {
            var doc = _uiApplication.ActiveUIDocument.Document;
            using var trGr = new TransactionGroup(doc);
            if (TransactionStatus.Started != trGr.Start("Группа транзакций обновление однолинейных схем"))
                return;
            var nameOfFamilyOfHead = "ЭОМ-Схемы однолинейные-Шапка (ГОСТ 2.708-81)";
            var familyHead = new FilteredElementCollector(doc)
                    .OfClass(typeof(Family))
                    .FirstOrDefault(x => x.Name == nameOfFamilyOfHead)
                as Family;
            var familySymbolHead = doc?.GetElement(familyHead?.GetFamilySymbolIds().First()) as FamilySymbol;
            if (familySymbolHead == null)
                throw new NullReferenceException($"Не удалось найти семейство \"{nameOfFamilyOfHead}\"");
            var filter = new FamilyInstanceFilter(doc, familySymbolHead.Id);
            //todo
            var head = new FilteredElementCollector(doc, view.Id)
                // .OfClass(typeof(FamilyInstance))
                .WherePasses(filter)
                .FirstOrDefault() as FamilyInstance;
            var shieldId = head?.LookupParameter("ID электрического щита")?.AsString();
            Shield shield = null;
            if (shieldId != null)
            {
                var shieldFi = doc.GetElement(shieldId) as FamilyInstance;
                shield = new Shield(shieldFi);
            }

            //Если не удалось найти щит по UnID
            if (shield is null)
            {
                var task = new TaskDialog("Info");
                task.MainContent = $"Не удалось найти щит по UniqueID.";
                task.Show();
                return;
            }

            using (var tr = new Transaction(doc))
            {
                tr.Start("Обновление параметров в шапке");
                //OneLineDiagramBuiltDiagram.CalculateCurrentImbalance(shield);
                _diagramsDrawer.SetParametersToHead(head, shield);
                tr.Commit();
            }

            //Удалить все отходящие линии
            DeleteOldLines(view);
            //Нарисовать новые линии
            _diagramsDrawer.DrawLines(shield, view);
            trGr.Assimilate();
        }

        private static void DeleteOldLines(View view)
        {
            var doc = view.Document;
            var nameOfFamilyLine = "ЭОМ-Схемы однолинейные-Отходящая линия (ГОСТ 2.708-81)";
            var familyLine = new FilteredElementCollector(doc)
                    .OfClass(typeof(Family))
                    .FirstOrDefault(x => x.Name == nameOfFamilyLine)
                as Family;
            if (familyLine == null)
                throw new NullReferenceException($"Не удалось найти семейство \"{nameOfFamilyLine}\"");
            var familySymbolLine = (FamilySymbol)doc?.GetElement(familyLine?.GetFamilySymbolIds().First());
            if (familySymbolLine == null)
                throw new NullReferenceException($"Не удалось найти семейство \"{nameOfFamilyLine}\"");
            var filter = new FamilyInstanceFilter(doc, familySymbolLine.Id);
            //todo
            var lines = new FilteredElementCollector(doc, view.Id)
                .OfClass(typeof(FamilyInstance))
                .WherePasses(filter).OfType<FamilyInstance>();
            using var tr = new Transaction(doc);
            tr.Start("Удалить отходящие линии");
            doc.Delete(lines.Select(x => x.Id).ToArray());
            tr.Commit();
        }
    }
}
