namespace ElectricityRevitPlugin.RibbonBuilder.Application;

public interface IMenuBuilder
{
    IMenuBuilder Tab(string name, Action<TabBuilder> tabBuilder);
    void Build(IVisitorBuilder visitorBuilder);
}