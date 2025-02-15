namespace PrintCmd.Infrastructure;

using System.Linq;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Print.Application.Application;
using Print.Application.Domain;
using ColorDepthType = Autodesk.Revit.DB.ColorDepthType;
using HiddenLineViewsType = Autodesk.Revit.DB.HiddenLineViewsType;
using RasterQualityType = Autodesk.Revit.DB.RasterQualityType;

public class PrintService(UIApplication uiApplication, RevitTask revitTask) : IPrintService
{
    private Document Document => uiApplication.ActiveUIDocument.Document;

    public async Task Print(PrintSettingsOut exportSettingsOut)
    {
        await revitTask.Run(uiApp =>
        {
            var views = exportSettingsOut.ViewSheetIds
                .Select(x => Document.GetElement(new ElementId(x)))
                .OfType<ViewSheet>()
                .ToList();

            RevitPrinter.RunPrinting(Document,
                views,
                exportSettingsOut.PrinterName,
                exportSettingsOut.Path,
                exportSettingsOut.ReplaceHalftoneWithThinLines,
                exportSettingsOut.HideCropBoundaries,
                exportSettingsOut.HideScopeBoxes,
                exportSettingsOut.MaskCoincidentLines,
                exportSettingsOut.HideUnreferencedViewTags,
                exportSettingsOut.HideReforWorkPlanes,
                exportSettingsOut.ViewLinksinBlue,
                (RasterQualityType)exportSettingsOut.RasterQuality,
                (ColorDepthType)exportSettingsOut.ColorDepthType,
                (HiddenLineViewsType)exportSettingsOut.HiddenLineViewsType);
        });
    }

    public Task Export(ExportSettingsOut exportSettingsOut)
    {
        var dwgOptions = DWGExportOptions.GetPredefinedOptions(Document, exportSettingsOut.DwgExportOption);
        // Export the active view
        var views = exportSettingsOut.ViewSheetIds
            .Select(id => new ElementId(id))
            .ToArray();
        // The document has to be saved already, therefore it has a valid PathName.
        var exported = Document.Export(exportSettingsOut.Folder, exportSettingsOut.FilePrefix, views, dwgOptions);
        return Task.CompletedTask;
    }
}
