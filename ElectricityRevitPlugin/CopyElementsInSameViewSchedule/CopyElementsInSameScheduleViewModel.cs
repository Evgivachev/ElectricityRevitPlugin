using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ElectricityRevitPlugin.CopyElementsInSameViewSchedule
{
    public class CopyElementsInSameScheduleViewModel
    {
        private readonly Document _doc;
        public CopyElementsInSameScheduleView(Document doc)
        {
            _doc = doc;
            var activeView = _doc.ActiveView as ViewSchedule;
            if (activeView is null)
                throw new InvalidOperationException("Вам следует перейти на вид ключевой спецификации");
            Elements = new ObservableCollection<Element>(new FilteredElementCollector(_doc, activeView.Id)
               );
        }
        public ObservableCollection<Element> Elements { get; set; }
    }
}
