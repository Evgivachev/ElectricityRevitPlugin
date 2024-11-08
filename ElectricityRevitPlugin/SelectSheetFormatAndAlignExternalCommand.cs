namespace ElectricityRevitPlugin;

using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

[Regeneration(RegenerationOption.Manual)]
[Transaction(TransactionMode.Manual)]
public class SelectSheetFormatAndAlignExternalCommand : IExternalCommand
{
    protected XYZ ShiftTitleBlock = new(
        UnitUtils.ConvertToInternalUnits(-10, UnitTypeId.Millimeters), 0,
        0);

    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiApp = commandData.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        var result = Result.Succeeded;
        try
        {
            using (var trGr = new TransactionGroup(doc))
            {
                trGr.Start(ToString());
                var selection = uiDoc.Selection
                    .GetElementIds();
                var sheets = selection.Select(x => doc.GetElement(x))
                    .OfType<ViewSheet>()
                    .ToArray();
                if (!sheets.Any())
                    throw new Exception("Следует выделить листы в диспетчере проекта");
                foreach (var view in sheets)
                    ExecuteOnTheViewSheet(commandData, view, ShiftTitleBlock);

                trGr.Assimilate();
            }
        }
        catch (Exception e)
        {
            message += e.Message + '\n' + e.StackTrace;
            result = Result.Failed;
        }

        return result;
    }

    private void ExecuteOnTheViewSheet(
        ExternalCommandData commandData,
        ViewSheet view,
        XYZ shiftTitleBlock)
    {
        var uiApp = commandData.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        using var tr = new Transaction(doc);
        tr.Start("Move Title Block");
        if (view is null)
            throw new Exception("Active View is not ViewSheet");
        var allElementsOnView = new FilteredElementCollector(doc, view.Id).ToElements();
        var titleBlock = (FamilyInstance)allElementsOnView
            .FirstOrDefault(x => x.Category.Id.IntegerValue == (int)BuiltInCategory.OST_TitleBlocks);
        if (titleBlock is null)
            throw new Exception("TitleBlock is null");
        var bb = GetBoundingBoxXyz(view, allElementsOnView);
        var betterFormat = ChooseTitleBlockFormat(titleBlock, bb);
        if (betterFormat != null)
        {
            titleBlock.get_Parameter(BuiltInParameter.ELEM_TYPE_PARAM).Set(betterFormat);
            doc.Regenerate();
        }

        var viewPortCenter = bb.Max.Add(bb.Min).Multiply(0.5);
        var titleBlockBoundingBox = titleBlock.get_BoundingBox(view);
        var titleBlockCenter = titleBlockBoundingBox.Min.Add(titleBlockBoundingBox.Max).Multiply(0.5);
        titleBlock.Location.Move(viewPortCenter - titleBlockCenter);
        if (shiftTitleBlock != null)
            titleBlock.Location.Move(shiftTitleBlock);
        tr.Commit();
    }

    private ElementId ChooseTitleBlockFormat(FamilyInstance titleBlock, BoundingBoxXYZ box)
    {
        var doc = titleBlock.Document;
        var allTypes = titleBlock.Symbol.Family.GetFamilySymbolIds()
            .Select(x => doc.GetElement(x));
        var length0 = box.Max.X - box.Min.X;
        var height0 = box.Max.Y - box.Min.Y;
        var value0 = double.MaxValue;
        ElementId result = null;
        foreach (var type in allTypes)
        {
            var length = type.LookupParameter("Ширина").AsDouble() -
                         UnitUtils.ConvertToInternalUnits(25, UnitTypeId.Millimeters);
            var height = type.LookupParameter("Высота").AsDouble() -
                         UnitUtils.ConvertToInternalUnits(10, UnitTypeId.Millimeters);
            var dl = length - length0;
            var dh = height - height0;
//                var value = Math.Abs(dl) * Math.Abs(dh);
            var value = dl * dl + dh * dh * 10;
            if (dl < 0)
                value *= 1000;
            if (dh < 0)
                value *= 1000;
            if (value > 0 && value < value0)
            {
                value0 = value;
                result = type.Id;
            }
        }

        return result;
    }

    protected virtual BoundingBoxXYZ GetBoundingBoxXyz(ViewSheet viewSheet, IEnumerable<Element> elements)
    {
        var allElements = elements.Where(x => x.Category.Id.IntegerValue != (int)BuiltInCategory.OST_TitleBlocks);
        var bbs = allElements.Select(x => x.get_BoundingBox(viewSheet)).ToArray();
        if (!bbs.Any())
            return new BoundingBoxXYZ();
        var minX = double.MaxValue;
        var minY = double.MaxValue;
        var maxX = double.MinValue;
        var maxY = double.MinValue;
        foreach (var bb in bbs)
        {
            minX = Math.Min(minX, bb.Min.X);
            minY = Math.Min(minY, bb.Min.Y);
            maxX = Math.Max(maxX, bb.Max.X);
            maxY = Math.Max(maxY, bb.Max.Y);
        }

        var result = new BoundingBoxXYZ { Min = new XYZ(minX, minY, 0), Max = new XYZ(maxX, maxY, 0) };
        return result;
    }
}

[Regeneration(RegenerationOption.Manual)]
[Transaction(TransactionMode.Manual)]
public class SelectSheetFormatAndAlignOnDiagramExternalCommand : SelectSheetFormatAndAlignExternalCommand
{
    public SelectSheetFormatAndAlignOnDiagramExternalCommand()
    {
        ShiftTitleBlock = new XYZ(
            UnitUtils.ConvertToInternalUnits(-10, UnitTypeId.Millimeters),
            UnitUtils.ConvertToInternalUnits(-55.0 / 2, UnitTypeId.Millimeters),
            0);
    }
}
