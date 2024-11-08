namespace ElectricityRevitPlugin.RibbonBuilder.Application;

using Domain;

public class TabBuilder
{

    private readonly List<Action<RibbonBuilder>> ribbonBuilders = [];
    public TabBuilder Panel(string name, Action<RibbonBuilder> ribbonBuilder)
    {
        ribbonBuilders.Add(ribbon =>
        {
            ribbon.SetName(name);
            ribbonBuilder(ribbon);
        });
        return this;
    }

    public Tab Build(string name, IVisitorBuilder visitorBuilder)
    {
        var tab = new Tab(name);
        
        foreach (var ribbonBuilderAct in ribbonBuilders)
        {
            var ribbonBuilder = new RibbonBuilder();
            ribbonBuilderAct(ribbonBuilder);
            var ribbon = ribbonBuilder.Build(visitorBuilder);
            tab.AddRibbon(ribbon);
            visitorBuilder.AddRibbon(tab, ribbon);
        }
        
        return tab;
    }
}