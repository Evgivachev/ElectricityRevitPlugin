namespace ElectricityRevitPlugin.RibbonBuilder.Application;

using Domain;

public class ButtonBuilder
{
    private readonly List<Action<Button>> _actions = [];
    public ButtonBuilder Text(string text)
    {
        _actions.Add(button => button.Text = text);
        return this;
    }

    public ButtonBuilder LargeImage(string path)
    {
        _actions.Add(button => button.LargeImage = path);
        return this;
    }

    public ButtonBuilder Description(string text)
    {
        _actions.Add(button => button.Description = text);
        return this;
    }

    public ButtonBuilder ToolTip(string text)
    {
        _actions.Add(button => button.ToolTip = text);
        return this;
    }

    public Button Build(IVisitorBuilder visitor)
    {
        var button = new Button();
        _actions.ForEach(action => action(button));
        visitor.Create(button);
        return button;
    }
    public void SetName(string name)
    {
        _actions.Add(button => button.Name = name);
    }
    public void SetCmd(Type cmd)
    {
        _actions.Add(button => button.CommandType = cmd);
    }
}
