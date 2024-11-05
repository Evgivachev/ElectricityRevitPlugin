namespace ElectricityRevitPlugin.RibbonBuilder.Application;

using ElectricityRevitPlugin.RibbonBuilder.Domain;

public class StackedItemsBuilder
{
    private readonly List<Func<IVisitorBuilder, IRibbonItem>> _actions = [];
    public StackedItemsBuilder CommandButton(string name, Type cmd, Action<ButtonBuilder> action)
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

    public StackedItemsBuilder CommandButton<T>(string name, Action<ButtonBuilder> action)
    {
        _actions.Add(visitor =>
        {
            var builder = new ButtonBuilder();
            builder.SetName(name);
            builder.SetCmd(typeof(T));
            action(builder);
            var button = builder.Build(visitor);
            return button;
        });
        return this;
    }

    public StackedItems Build(IVisitorBuilder visitor)
    {
        var stack = new StackedItems();
        visitor.Create(stack);
        foreach (var action in _actions)
        {
            var ribbonItem = action(visitor);
            stack.Add(ribbonItem);
            visitor.Add(stack, ribbonItem);
        }
        return stack;
    }
}
