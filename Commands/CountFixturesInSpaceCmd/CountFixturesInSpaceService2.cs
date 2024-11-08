namespace CountFixturesInSpaceCmd;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommonUtils;
using CommonUtils.Extensions;

/// <inheritdoc />
public class CountFixturesInSpaceService2 : DefaultUseCase
{

    /// <inheritdoc />
    public override Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiApp = commandData.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        var result = Result.Succeeded;
        var adsk_zonaGuid = new Guid("c78f0a7d-b68b-4d21-a247-1c8c6ced8bc5");
        try
        {
            using var tr = new Transaction(doc);
            tr.Start("Количество светильников в пространствах 2");
            var allFixtures = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_LightingFixtures)
                .WhereElementIsNotElementType()
                .Cast<FamilyInstance>();
            var viewphaseId = doc.ActiveView.get_Parameter(BuiltInParameter.VIEW_PHASE).AsElementId();
            var viewPhase = (Phase)doc.GetElement(viewphaseId);
            foreach (var element in allFixtures)
            {
                var fixture = element;
                 
                //Стадия сноса
                var fixturePhaseDemolished =
                    (Phase)doc.GetElement(element.get_Parameter(BuiltInParameter.PHASE_DEMOLISHED).AsElementId());
                if (fixturePhaseDemolished != null && viewPhase.get_Parameter(BuiltInParameter.PHASE_SEQUENCE_NUMBER).AsInteger() >
                    fixturePhaseDemolished.get_Parameter(BuiltInParameter.PHASE_SEQUENCE_NUMBER).AsInteger())
                    continue;
                //var space = fixture.Space;
                var space = fixture.get_Space(viewPhase);
                var adsk_zonaParameter = fixture.get_Parameter(adsk_zonaGuid);
                if (space is null)
                {
                    adsk_zonaParameter.ResetValue();
                    continue;
                }

                adsk_zonaParameter.Set($"Пом. {space.Number}");
            }

            tr.Commit();
        }
        catch (Exception e)
        {
            message += e.Message + '\n' + e.StackTrace;
            result = Result.Failed;
        }

        return result;
    }
}
