namespace ElectricityRevitPlugin.Extensions
{
    using System;
    using System.Linq;
    using Autodesk.Revit.DB;

    public static class KeyScheduleExtension
    {
        public static void AddElement(this ViewSchedule schedule, Element el, bool openTransaction)
        {
            var fromElementName = el.Name;
            var doc = schedule.Document;
            var td = schedule.GetTableData();
            var body = td.GetSectionData(SectionType.Body);
            var flag = body.CanInsertRow(body.FirstRowNumber);
            if (!flag)
                throw new Exception("Невозможно вставить строку в данную спецификацию");
            body.InsertRow(body.FirstRowNumber);
            var addedElement = new FilteredElementCollector(doc, schedule.Id)
                .FirstOrDefault(x => int.TryParse(x.Name, out _));
            addedElement?.CopyParameters(el, openTransaction);
        }
    }
}
