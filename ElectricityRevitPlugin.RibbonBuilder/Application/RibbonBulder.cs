namespace ElectricityRevitPlugin.RibbonBuilder.Application;

using Domain;

public class RibbonBuilder
{
    private readonly List<IRibbonItem> _items = [];
    private readonly List<Func<IVisitorBuilder, IRibbonItem>> _actions = new();
    private string? _ribbonName;

    public RibbonBuilder StackedItems(Action<StackedItemsBuilder> action)
    {
        _actions.Add(visitor =>
        {
            var builder = new StackedItemsBuilder();
            action(builder);
            var stackedItems = builder.Build(visitor);
            return stackedItems;
        });
        return this;
    }

    public RibbonBuilder CommandButton(string name, Type cmd, Action<ButtonBuilder> action)
    {
        _actions.Add(visitor =>
        {
            var builder = new ButtonBuilder();
            builder.SetName(name);
            builder.SetCmd(cmd);
            action(builder);
            var button = builder.Build(visitor);
            return button;
        });
        return this;
    }

    public RibbonBuilder Separator()
    {
        _actions.Add(visitor =>
        {
            var separator = new Separator();
            visitor.CreateSeparator(separator);
            return separator;
        });
        return this;
    }

    public Ribbon Build(IVisitorBuilder visitorBuilder)
    {
        var ribbon = new Ribbon
        {
            Name = _ribbonName ?? Guid.NewGuid().ToString(),
        };
        visitorBuilder.CreateRibbon(ribbon);
        foreach (var ribbonItemBuilder in _actions)
        {
            var ribbonItem = ribbonItemBuilder(visitorBuilder);
            _items.Add(ribbonItem);
            visitorBuilder.AddRibbonItem(ribbon, ribbonItem);
        }
        return ribbon;
    }

    public RibbonBuilder SetName(string name)
    {
        _ribbonName = name;
        return this;
    }
}
