namespace ElectricityRevitPlugin.RibbonBuilder.Application;

public class MenuBuilder : IMenuBuilder
{
    private readonly Dictionary<string, Action<TabBuilder>> _tabBuilders = new();
    public IMenuBuilder Tab(string name, Action<TabBuilder> tabBuilder)
    {
        _tabBuilders[name] = tabBuilder;
        return this;
    }

    public void Build(IVisitorBuilder visitorBuilder)
    {
        foreach (var pair in _tabBuilders)
        {
            var tabBuilder = new TabBuilder();
            pair.Value(tabBuilder);
            visitorBuilder.CreateTab(pair.Key);
            var tab = tabBuilder.Build(pair.Key, visitorBuilder);
            
        }
    }
}
