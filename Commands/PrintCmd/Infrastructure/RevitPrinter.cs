namespace PrintCmd.Infrastructure;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Threading;
using System.Windows;
using Autodesk.Revit.DB;
using MoreLinq.Extensions;

public class RevitPrinter
{
    public static void RunPrinting(Document doc,
        IEnumerable<ViewSheet> viewSet,
        string printName,
        string path,
        bool replaceHalftoneWithThinLines,
        bool hideCropBoundaries,
        bool hideScopeBoxes,
        bool maskCoincidentLines,
        bool hideUnreferencedViewTags,
        bool hideReforWorkPlanes,
        bool viewLinksinBlue,
        RasterQualityType rasterQualityType,
        ColorDepthType colorDepthType,
        HiddenLineViewsType hiddenLineViewsType
    )
    {
        var pManager = doc.PrintManager;
        //var pps = pManager.PrintSetup.CurrentPrintSetting.PrintParameters;

        //using (var tr = new Transaction(doc))
        //{
        //    tr.Start("Save");

        if (pManager.PrinterName != printName)
        {
            pManager.SelectNewPrintDriver(printName);
            if (pManager.IsVirtual == VirtualPrinterType.AdobePDF)
            {
                pManager.PrintToFile = true;
            }
            else
            {
                MessageBox.Show("Следует выбрать виртуальный PDF принтер!");
                return;
            }
        }
        pManager = doc.PrintManager;

        pManager.PrintToFile = true;
        pManager.PrintRange = PrintRange.Current;
        pManager.PrintSetup.InSession.PrintParameters.PaperPlacement = PaperPlacementType.Center;
        pManager.PrintSetup.InSession.PrintParameters.ZoomType = ZoomType.Zoom;
        pManager.PrintSetup.InSession.PrintParameters.Zoom = 100;
        pManager.PrintSetup.InSession.PrintParameters.ColorDepth = colorDepthType;
        pManager.PrintSetup.InSession.PrintParameters.RasterQuality = rasterQualityType;
        pManager.PrintSetup.InSession.PrintParameters.HiddenLineViews = hiddenLineViewsType;
        pManager.PrintSetup.InSession.PrintParameters.HideCropBoundaries = hideCropBoundaries;
        pManager.PrintSetup.InSession.PrintParameters.HideReforWorkPlanes = hideReforWorkPlanes;
        pManager.PrintSetup.InSession.PrintParameters.HideScopeBoxes = hideScopeBoxes;
        pManager.PrintSetup.InSession.PrintParameters.HideUnreferencedViewTags = hideUnreferencedViewTags;
        pManager.PrintSetup.InSession.PrintParameters.MaskCoincidentLines = maskCoincidentLines;
        pManager.PrintSetup.InSession.PrintParameters.ViewLinksinBlue = viewLinksinBlue;
        pManager.PrintSetup.InSession.PrintParameters.ReplaceHalftoneWithThinLines = replaceHalftoneWithThinLines;

        //pManager.PrintSetup.CurrentPrintSetting.PrintParameters.PaperPlacement = PaperPlacementType.Center;
        //pManager.PrintSetup.CurrentPrintSetting.PrintParameters.ZoomType = ZoomType.Zoom;
        //pManager.PrintSetup.CurrentPrintSetting.PrintParameters.Zoom = 100;
        //pManager.PrintSetup.CurrentPrintSetting.PrintParameters.ColorDepth = colorDepthType;
        //pManager.PrintSetup.CurrentPrintSetting.PrintParameters.RasterQuality = rasterQualityType;
        //pManager.PrintSetup.CurrentPrintSetting.PrintParameters.HiddenLineViews = hiddenLineViewsType;
        //pManager.PrintSetup.CurrentPrintSetting.PrintParameters.HideCropBoundaries = hideCropBoundaries;
        //pManager.PrintSetup.CurrentPrintSetting.PrintParameters.HideReforWorkPlanes = hideReforWorkPlanes;
        //pManager.PrintSetup.CurrentPrintSetting.PrintParameters.HideScopeBoxes = hideScopeBoxes;
        //pManager.PrintSetup.CurrentPrintSetting.PrintParameters.HideUnreferencedViewTags = hideUnreferencedViewTags;
        //pManager.PrintSetup.CurrentPrintSetting.PrintParameters.MaskCoincidentLines = maskCoincidentLines;
        //pManager.PrintSetup.CurrentPrintSetting.PrintParameters.ViewLinksinBlue = viewLinksinBlue;
        //pManager.PrintSetup.CurrentPrintSetting.PrintParameters.ReplaceHalftoneWithThinLines = replaceHalftoneWithThinLines;
        //    tr.Commit();
        //}
        var frames = new FilteredElementCollector(doc)
            .OfCategory(BuiltInCategory.OST_TitleBlocks)
            .WhereElementIsNotElementType()
            .OfType<FamilyInstance>()
            .DistinctBy(f => f.OwnerViewId.IntegerValue)
            .ToDictionary(f => f.OwnerViewId.IntegerValue);
        foreach (var view in viewSet)
        {
            using (var tr = new Transaction(doc))
            {
                tr.Start("Задание размеров лиcта");
                var name = view.Name;
                var invalidFileNameChars = Path.GetInvalidFileNameChars();
                foreach (var c in invalidFileNameChars)
                {
                    name = name.Replace(c, '_');
                }
                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                    pManager.PrintToFileName = Path.Combine(path, name + ".pdf");
                else
                {
                    pManager.PrintToFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), name + ".pdf");
                }
                
                if (!frames.TryGetValue(view.Id.IntegerValue, out var frameInstance))
                {
                    MessageBox.Show($"Лист \"{name}\" не содержит основной надписи.");
                    continue;
                }

                var frame = frameInstance.Symbol;

                double.TryParse(frame.LookupParameter("Ширина")?.AsValueString(), out var width);
                double.TryParse(frame.LookupParameter("Высота")?.AsValueString(), out var height);
                var parameters = GetListParams(width, height);

                pManager.PrintSetup.InSession.PrintParameters.PageOrientation = parameters.Item1;
                pManager.PrintSetup.CurrentPrintSetting.PrintParameters.PageOrientation = parameters.Item1;
                //pManager.PrintSetup.InSession.PrintParameters.PageOrientation = height > width ? PageOrientationType.Landscape : PageOrientationType.Portrait;
                pManager.Apply();
                var paperSizes = pManager.PaperSizes;

                var flag = false;
                foreach (PaperSize paper in paperSizes)
                {
                    if (paper.Name != parameters.Item2) continue;
                    flag = true;

                    pManager.PrintSetup.InSession.PrintParameters.PaperSize = paper;
                    var time = DateTime.Now.TimeOfDay.ToString().Replace(':', '-');
                    pManager.PrintSetup.CurrentPrintSetting.PrintParameters.PaperSize = paper;
                    try
                    {
                        pManager.PrintSetup.Save();
                    }
                    catch
                    {
                        pManager.PrintSetup.SaveAs(time);
                    }
                    break;
                }

                if (!flag)
                {
                    MessageBox.Show($"Неизвестный формат листа \"{name}\".");
                    continue;

                }
                doc.Regenerate();
                tr.Commit();
            }
            pManager.Apply();
            
            using (var tr = new Transaction(doc))
            {
                tr.Start("Refresh");
                doc.Regenerate();
                tr.Commit();
            }

            var printQueue = LocalPrintServer.GetDefaultPrintQueue();
            if (printQueue is null)
                throw new NullReferenceException();

            while (printQueue.IsBusy || printQueue.IsNotAvailable || printQueue.IsPrinting || printQueue.IsProcessing)
            {
                Thread.Sleep(100);
            }
            var result = pManager.SubmitPrint(view);
        }
    }
    private static Tuple<PageOrientationType, string> GetListParams(double width, double height)
    {
        var square = width * height;
        var k = (int)Math.Round(square / 210 / 297);
        string format;
        var kk = 1;
        var orientation = height > width ? PageOrientationType.Portrait : PageOrientationType.Landscape;

        var min = Math.Min(width, height);
        var tolerance = 10;
        if (k == 1)
        {
            format = "A4";
        }
        else if (k == 2)
        {
            format = "A3";
        }
        else if (k == 3 || k == 5 || k == 7 || k == 9)
        {
            format = "A4";
            kk = k;
        }
        else if (k == 4)
        {
            if (Math.Abs(min - 297) < tolerance)
            {
                format = "A4";
                kk = 4;
            }
            else
            {
                format = "A2";
                kk = 1;
            }
        }
        else if (k == 6)
        {
            if (Math.Abs(min - 297) < tolerance)
            {
                format = "A4";
                kk = 6;
            }
            else
            {
                format = "A3";
                kk = 3;
            }
        }
        else if (k == 8)
        {
            if (Math.Abs(min - 297) < tolerance)
            {
                format = "A4";
                kk = 8;
            }
            else if (Math.Abs(min - 420) < tolerance)
            {
                format = "A3";
                kk = 4;
            }
            else
            {
                format = "A1";
                kk = 1;
            }
        }
        else if (k == 10 || k == 14)
        {
            format = "A3";
            kk = k / 2;
        }
        else if (k == 12)
        {
            if (Math.Abs(min - 420) < tolerance)
            {
                format = "A3";
                kk = 6;
            }
            else
            {
                format = "A2";
                kk = 3;
            }
        }
        else if (k == 16)
        {
            if (Math.Abs(min - 594) < tolerance)
            {
                format = "A2";
                kk = 4;
            }
            else
            {
                format = "A0";
                kk = 1;
            }
        }
        else if (k == 20)
        {
            format = "A2";
            kk = 5;
        }
        else if (k == 24)
        {
            format = "A1";
            kk = 3;
        }
        else if (k == 32)
        {
            if (Math.Abs(min - 841) < tolerance)
            {
                format = "A1";
                kk = 4;
            }
            else
            {
                format = "A0";
                kk = 2;
            }
        }
        else if (k == 48)
        {
            format = "A0";
            kk = 2;
        }
        else
        {
            throw new ArgumentException();
        }
        return new Tuple<PageOrientationType, string>(orientation, $"{format}" + (kk != 1 ? $"x{kk}" : ""));

    }

}
