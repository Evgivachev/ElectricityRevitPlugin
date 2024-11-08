namespace ElectricityRevitPlugin;

using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class SetHeightForSpaceExternalCommand : IExternalCommand
{
    private IList<Room> _rooms;

    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiApp = commandData.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        var result = Result.Succeeded;
        try
        {
            var spaces = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_MEPSpaces)
                .OfType<Space>();

            //var spaceWithRooms = spaces.Where(s => s.Room != null)
            //    .Select(x=>x.Name)
            //    .ToArray();
            SelectLink(uiDoc);
            using (var tr = new Transaction(doc))
            {
                tr.Start("Установка высоты пространств");
                foreach (var space in spaces)
                    SetHeightOfSpace(space);

                tr.Commit();
            }
        }
        catch (Exception e)
        {
            message += e.Message + '\n' + e.StackTrace;
            result = Result.Failed;
        }

        return result;
    }

    private void SelectLink(UIDocument uiDoc)
    {
        var doc = uiDoc.Document;
        var selection = uiDoc.Selection;
        var reference = selection.PickObject(ObjectType.Element, new RevitLinkSelectionFilter());
        var linkInstance = doc.GetElement(reference.ElementId) as RevitLinkInstance;
        var linkedDoc = linkInstance.GetLinkDocument();
        var roomsInLinkDoc = new FilteredElementCollector(linkedDoc)
            .OfCategory(BuiltInCategory.OST_Rooms)
            .OfType<Room>();
        _rooms = roomsInLinkDoc.ToArray();
    }

    private void SetHeightOfSpace(Space space)
    {
        var spaceNumber = space.Number;
        var spaceName = space.Name;
        if (string.IsNullOrEmpty(spaceName) || string.IsNullOrEmpty(spaceNumber))
            return;
        var room = _rooms.FirstOrDefault(r =>
        {
            if (string.IsNullOrEmpty(r.Name) || string.IsNullOrEmpty(r.Number))
                return false;
            return r.Name == spaceName && spaceNumber.StartsWith(r.Number);
        });
        if (room is null)
            return;
        _ = space.get_Parameter(BuiltInParameter.ROOM_LOWER_OFFSET).Set(room.BaseOffset) &&
            space.get_Parameter(BuiltInParameter.ROOM_UPPER_OFFSET).Set(room.LimitOffset);
    }
}
