namespace ElectricityRevitPlugin;

using System;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommonUtils.Helpers;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
class UpdateParameterADSK_Floor : DefaultExternalCommand
{
    private HashSet<BuiltInCategory> _categorySet = new()
    {
        BuiltInCategory.OST_LightingDevices,
        BuiltInCategory.OST_CableTray,
        BuiltInCategory.OST_CableTrayFitting,
        BuiltInCategory.OST_Conduit,
        BuiltInCategory.OST_ConduitFitting,
        BuiltInCategory.OST_LightingFixtures,
        BuiltInCategory.OST_ElectricalFixtures,
        BuiltInCategory.OST_ElectricalEquipment
    };

    protected override Result DoWork(ref string message, ElementSet elements)
    {
        var floorGuid = new Guid("9eabf56c-a6cd-4b5c-a9d0-e9223e19ea3f");
        using (var tr = new Transaction(Doc))
        {
            tr.Start("Adsk_floor");
            foreach (var builtInCategory in _categorySet)
            {
                var allElements = new FilteredElementCollector(Doc)
                    .OfCategory(builtInCategory)
                    .WhereElementIsNotElementType();
                foreach (var element in allElements)
                {
                    var adskFloor = element.get_Parameter(floorGuid)?.AsString() ?? "";
                    if (!(element is FamilyInstance fi))
                        continue;
                    foreach (var subElementId in fi.GetSubComponentIds())
                    {
                        var subElement = Doc.GetElement(subElementId);
                        var floorParameter = subElement.get_Parameter(floorGuid);
                        if (floorParameter.IsReadOnly)
                            continue;
                        floorParameter?.Set(adskFloor);
                    }
                }
            }

            tr.Commit();
        }

        return Result.Succeeded;
    }
}
