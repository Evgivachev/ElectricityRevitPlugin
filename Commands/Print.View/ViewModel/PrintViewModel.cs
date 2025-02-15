namespace Print.View.ViewModel;

using System.Drawing.Printing;
using Application.Application;
using Application.Domain;
using GalaSoft.MvvmLight;

public class PrintViewModel(IPrintService printService) : ObservableObject
{
    private bool _isPaperPlacementTypeCenter = true;
    private bool _isPaperPlacementTypeMargins;
    private bool _isHiddenLineViewsTypeVectorProcessing = true;
    private bool _isHiddenLineViewsTypeRasterProcessing;
    private bool _isZoomTypeFitToPage = true;
    private bool _isZoomTypeZoom;
    private string? _printerName;
    private RasterQualityType _rasterQuality = RasterQualityType.High;
    private ColorDepthType _colorDepthType = ColorDepthType.Color;
    private IReadOnlyCollection<string> _availablePrinters = [];

    public string? PrinterName
    {
        get => _printerName;
        set => Set(ref _printerName, value);
    }

    public bool ViewLinksinBlue { get; set; }
    public bool HideReforWorkPlanes { get; set; } = true;
    public bool HideUnreferencedViewTags { get; set; }
    public bool MaskCoincidentLines { get; set; }
    public bool HideScopeBoxes { get; set; } = true;
    public bool HideCropBoundaries { get; set; } = true;
    public bool ReplaceHalftoneWithThinLines { get; set; }

    public bool IsPaperPlacementTypeCenter
    {
        get => _isPaperPlacementTypeCenter;
        set => Set(ref _isPaperPlacementTypeCenter, value);
    }

    public bool IsPaperPlacementTypeMargins
    {
        get => _isPaperPlacementTypeMargins;
        set => Set(ref _isPaperPlacementTypeMargins, value);
    }

    public bool IsHiddenLineViewsTypeVectorProcessing
    {
        get => _isHiddenLineViewsTypeVectorProcessing;
        set => Set(ref _isHiddenLineViewsTypeVectorProcessing, value);
    }

    public bool IsHiddenLineViewsTypeRasterProcessing
    {
        get => _isHiddenLineViewsTypeRasterProcessing;
        set => Set(ref _isHiddenLineViewsTypeRasterProcessing, value);
    }

    public bool IsZoomTypeFitToPage
    {
        get => _isZoomTypeFitToPage;
        set => Set(ref _isZoomTypeFitToPage, value);
    }

    public bool IsZoomTypeZoom
    {
        get => _isZoomTypeZoom;
        set => Set(ref _isZoomTypeZoom, value);
    }


    public RasterQualityType RasterQuality
    {
        get => _rasterQuality;
        set => Set(ref _rasterQuality, value);
    }

    public ColorDepthType ColorDepthType
    {
        get => _colorDepthType;
        set => Set(ref _colorDepthType, value);
    }

    public IReadOnlyCollection<string> AvailablePrinters
    {
        get => _availablePrinters;
        set => Set(ref _availablePrinters, value);
    }

    public Task Initialize()
    {
        AvailablePrinters = PrinterSettings.InstalledPrinters.Cast<string>().ToList();
        PrinterName = AvailablePrinters.FirstOrDefault();
        return Task.CompletedTask;
    }

    public async Task Print(IReadOnlyCollection<int> sheets, string folder)
    {
        var settings = new PrintSettingsOut
        {
            ViewSheetIds = sheets,
            PrinterName = PrinterName ?? string.Empty,
            ReplaceHalftoneWithThinLines = ReplaceHalftoneWithThinLines,
            HideCropBoundaries = HideCropBoundaries,
            HideScopeBoxes = HideScopeBoxes,
            MaskCoincidentLines = MaskCoincidentLines,
            HideUnreferencedViewTags = HideUnreferencedViewTags,
            HideReforWorkPlanes = HideReforWorkPlanes,
            ViewLinksinBlue = ViewLinksinBlue,
            RasterQuality = RasterQuality,
            Path = folder,
            ColorDepthType = ColorDepthType,
            HiddenLineViewsType = HiddenLineViewsType.RasterProcessing,
        };
        await printService.Print(settings);
    }
}
