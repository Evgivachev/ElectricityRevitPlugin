namespace ElectricityRevitPlugin.CopyElementsInSameViewSchedule;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Autodesk.Revit.DB;

public class CopyElementsInSameScheduleViewModel
{
    private readonly Document _doc;

    public CopyElementsInSameScheduleViewModel(Document doc)
    {
        _doc = doc;
        var activeView = _doc.ActiveView as ViewSchedule;
        if (activeView is null)
            throw new InvalidOperationException("Вам следует перейти на вид ключевой спецификации");
        Elements = new ObservableCollection<CheckableItem>(new FilteredElementCollector(_doc, activeView.Id)
            .Select(el => new CheckableItem(el))
            .OrderBy(x => x.Name)
        );
    }

    public ObservableCollection<CheckableItem> Elements { get; set; }

    public IEnumerable<Element> CheckedElements =>
        Elements
            .Where(x => x.IsChecked)
            .Select(x => x.Element);
}
