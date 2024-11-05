namespace ElectricityRevitPlugin.RibbonBuilder.Domain;

public class Ribbon
{
    private readonly List<RibbonItem> _items = new();
    
    public IReadOnlyCollection<RibbonItem> Items => _items;
    public string Name { get; set; }
}
