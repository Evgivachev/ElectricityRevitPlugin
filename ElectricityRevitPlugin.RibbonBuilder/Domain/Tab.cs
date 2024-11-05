namespace ElectricityRevitPlugin.RibbonBuilder.Domain;

public class Tab(string name)
{
    private List<Ribbon> ribbons = new List<Ribbon>();
    public string Name { get; init; } = name;
    public IReadOnlyList<Ribbon> Ribbons => ribbons;
    internal void AddRibbon(Ribbon ribbon) => ribbons.Add(ribbon);
}
