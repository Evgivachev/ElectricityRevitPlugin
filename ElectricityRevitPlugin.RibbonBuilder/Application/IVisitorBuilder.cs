namespace ElectricityRevitPlugin.RibbonBuilder.Application;

using Domain;

public interface IVisitorBuilder
{
    void CreateTab(string tab);
    void AddRibbon(Tab tab, Ribbon ribbon);
    void CreateRibbon(Ribbon ribbon);
    void AddRibbonItem(Ribbon ribbon, IRibbonItem ribbonItem);
    void Create(Button button);
    void CreateSeparator(Separator separator);
    void Add(StackedItems stack, IRibbonItem ribbonItem);
    void Create(StackedItems stack);
}
