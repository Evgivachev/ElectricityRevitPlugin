namespace ElectricityRevitPlugin.Application;

using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;
using ElectricityRevitPlugin.RibbonBuilder.Application;
using RibbonBuilder.Domain;
using Tab = RibbonBuilder.Domain.Tab;

public class RevitVisitorBuilder(UIControlledApplication application) : IVisitorBuilder
{
    private readonly Dictionary<StackedItems, List<IRibbonItem>> _stackedElements = [];
    private readonly Dictionary<object, RibbonItemData> _elements = new();
    private readonly Dictionary<Ribbon, List<Action<RibbonPanel>>> _panelAction = new();
    public void CreateTab(string tab)
    {
        application.CreateRibbonTab(tab);
    }
    public void AddRibbon(Tab tab, Ribbon ribbon)
    {
        var ribbonPanel = application.CreateRibbonPanel(tab.Name, ribbon.Name);
        foreach (var action in _panelAction[ribbon])
            action(ribbonPanel);
    }
    public void CreateRibbon(Ribbon ribbon)
    {
        _panelAction[ribbon] = [];
        // do nothing
    }
    public void AddRibbonItem(Ribbon ribbon, IRibbonItem ribbonItem)
    {
        if (_elements.TryGetValue(ribbonItem, out var itemData))
        {
            _panelAction[ribbon].Add(panel => { panel.AddItem(itemData); });
            return;
        }
        if (ribbonItem is StackedItems stackedItems)
        {
            if (stackedItems.Count == 2)
                _panelAction[ribbon].Add(panel => { panel.AddStackedItems(_elements[stackedItems[0]], _elements[stackedItems[1]]); });
            if (stackedItems.Count == 3)
                _panelAction[ribbon].Add(panel =>
                {
                    panel.AddStackedItems(_elements[stackedItems[0]], _elements[stackedItems[1]], _elements[stackedItems[2]]);
                });
        }

        if (ribbonItem is Separator separator)
        {
            _panelAction[ribbon].Add(panel => { panel.AddSeparator(); });
        }
    }

    public void Create(Button button)
    {
        var basePath = Path.GetDirectoryName(GetType().Assembly.Location)!;
        var data = new PushButtonData(
            Guid.NewGuid().ToString(),
            button.Text,
            button.CommandType.Assembly.Location,
            button.CommandType.FullName);
        var largeImagePath = Path.Combine(basePath, button.LargeImage);
        if (File.Exists(largeImagePath))
            data.LargeImage = new BitmapImage(new Uri(largeImagePath));
        _elements.Add(button, data);
    }
    public void CreateSeparator(Separator separator)
    {
    }

    public void Add(StackedItems stack, IRibbonItem ribbonItem)
    {
        _stackedElements[stack].Add(ribbonItem);
    }

    public void Create(StackedItems stack)
    {
        if (!_stackedElements.ContainsKey(stack))
            _stackedElements.Add(stack, []);

        if (_stackedElements[stack].Count == 2)
        {

        }

    }
}
