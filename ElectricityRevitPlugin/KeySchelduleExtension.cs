using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;

namespace ElectricityRevitPlugin
{
    public static class KeyScheduleExtension
    {
        public static void AddElement(this ViewSchedule schedule, Application app, Element el, bool openTransaction)
        {
            var fromElementName = el.Name;
            var doc = el.Document;
            var td = schedule.GetTableData();
            var body = td.GetSectionData(SectionType.Body);
            var flag = body.CanInsertRow(body.FirstRowNumber);
            if(!flag)
                throw new Exception("Невозможно вставить строку в данную спецификацию");
            body.InsertRow(body.FirstRowNumber);
            var addedElement = new FilteredElementCollector(doc, schedule.Id)
                .FirstOrDefault(x => int.TryParse(x.Name, out _));
            addedElement?.CopyParameters(el,openTransaction);
        }
    }
}
