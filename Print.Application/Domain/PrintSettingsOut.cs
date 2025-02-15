namespace Print.Application.Domain;

public class PrintSettingsOut
{
    public IReadOnlyCollection<int> ViewSheetIds { get; init; }

    public string PrinterName { get; init; }
    
    public bool ReplaceHalftoneWithThinLines { get; init; }
    public bool HideCropBoundaries { get; set; }
    public bool HideScopeBoxes { get; set; }
    public bool MaskCoincidentLines { get; set; }
    public bool HideUnreferencedViewTags { get; set; }
    public bool HideReforWorkPlanes { get; set; }
    public bool ViewLinksinBlue { get; set; }
    public RasterQualityType RasterQuality { get; set; }
    public string Path { get; set; }
    public ColorDepthType ColorDepthType { get; set; }
    public HiddenLineViewsType HiddenLineViewsType { get; set; }
}
