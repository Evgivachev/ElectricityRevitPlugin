namespace ElectricityRevitPlugin.RibbonBuilder.Domain;

public class StackedItems : IRibbonItem
{
    private List<IRibbonItem> _items = [];
    
    public void Add(IRibbonItem item)
    {
        _items.Add(item);
    }
    public int Count => _items.Count;
    
    public IRibbonItem this[int index] => _items[index];
}
