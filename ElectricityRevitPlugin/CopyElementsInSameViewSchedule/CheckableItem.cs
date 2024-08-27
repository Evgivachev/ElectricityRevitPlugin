namespace ElectricityRevitPlugin.CopyElementsInSameViewSchedule;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;

public class CheckableItem : INotifyPropertyChanged

{
    public CheckableItem(Element element)
    {
        Name = element.Name;
        Element = element;
    }

    public string Name { get; }

    public bool IsChecked { get; set; } = false;

    public Element Element { get; private set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
