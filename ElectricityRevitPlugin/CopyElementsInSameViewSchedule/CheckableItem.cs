namespace ElectricityRevitPlugin.CopyElementsInSameViewSchedule;

using Autodesk.Revit.DB;

public class CheckableItem

{
    public CheckableItem(Element element)
    {
        Name = element.Name;
        Element = element;
    }

    public string Name { get; }

    public bool IsChecked { get; set; } = false;

    public Element Element { get; private set; }
}
