namespace ElectricityRevitPlugin.RibbonBuilder.Domain;

public class Button : IRibbonItem
{
    public string LargeImage { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ToolTip { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public Type CommandType { get; set; } = null!;
}
