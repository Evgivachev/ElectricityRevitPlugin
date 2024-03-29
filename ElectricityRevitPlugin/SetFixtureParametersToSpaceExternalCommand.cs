namespace ElectricityRevitPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Mechanical;
    using Autodesk.Revit.UI;
    using Extensions;

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SetFixtureParametersToSpaceExternalCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var doc = uiDoc.Document;
            var app = uiApp.Application;
            var result = Result.Succeeded;
            try
            {
                using (var tr = new Transaction(doc))
                {
                    tr.Start("Количество светильников в пространствах");
                    var spaces = new Dictionary<int, Dictionary<string, (int Count, double Heigth)>>();
                    var allFixtures = new FilteredElementCollector(doc)
                        .OfCategory(BuiltInCategory.OST_LightingFixtures)
                        .WhereElementIsNotElementType()
                        .Cast<FamilyInstance>()
                        .Where(x =>
                        {
                            return true;
                            //var flag = x.MEPModel?.ElectricalSystems?.IsEmpty;
                            //return flag.HasValue && !flag.Value;
                        });
                    var viewphaseId = doc.ActiveView.get_Parameter(BuiltInParameter.VIEW_PHASE).AsElementId();
                    var viewPhase = (Phase)doc.GetElement(viewphaseId);
                    foreach (var element in allFixtures)
                    {
                        var fixture = (FamilyInstance)element;
                        var fixtureName = fixture.Name;
                        var fixturePhaseCreated =
                            doc.GetElement(element.get_Parameter(BuiltInParameter.PHASE_CREATED).AsElementId()) as Phase;
                        //Стадия сноса
                        var fixturePhaseDemolished =
                            (Phase)doc.GetElement(element.get_Parameter(BuiltInParameter.PHASE_DEMOLISHED).AsElementId());
                        if (fixturePhaseDemolished != null && viewPhase.get_Parameter(BuiltInParameter.PHASE_SEQUENCE_NUMBER).AsInteger() >
                            fixturePhaseDemolished.get_Parameter(BuiltInParameter.PHASE_SEQUENCE_NUMBER).AsInteger())
                            continue;
                        //var space = fixture.Space;
                        var space = fixture.get_Space(viewPhase);
                        if (space is null)
                            continue;
                        var spaceIdInt = space.Id.IntegerValue;
                        if (!spaces.ContainsKey(spaceIdInt))
                            spaces[spaceIdInt] = new Dictionary<string, (int Count, double Heigth)>();
                        if (!spaces[spaceIdInt].ContainsKey(fixtureName))
                            spaces[spaceIdInt][fixtureName] = (0,
                                Heigth: element.GetInstallationHeightRelativeToLevel(UnitTypeId.Millimeters));
                        spaces[spaceIdInt][fixtureName] =
                            (spaces[spaceIdInt][fixtureName].Count + 1, spaces[spaceIdInt][fixtureName].Heigth);
                    }

                    var allSpaces = new FilteredElementCollector(doc)
                        .OfCategory(BuiltInCategory.OST_MEPSpaces)
                        .OfType<Space>();
                    foreach (var space in allSpaces)
                    {
                        if (spaces.ContainsKey(space.Id.IntegerValue))
                            continue;
                        spaces[space.Id.IntegerValue] = null;
                    }

                    var names = spaces.Select(x => doc.GetElement(new ElementId(x.Key)).Name).OrderBy(x => x).ToArray();
                    foreach (var pair in spaces)
                    {
                        var spaceId = pair.Key;
                        var space = doc.GetElement(new ElementId(pair.Key));
                        var types = new[]
                        {
                            space.LookupParameter("Тип светильников"),
                            space.LookupParameter("Тип светильников 2"),
                        };
                        var counts = new[]
                        {
                            space.LookupParameter("Количество светильников"),
                            space.LookupParameter("Количество светильников 2")
                        };
                        var countsLamp = new[]
                        {
                            space.LookupParameter("Количество ламп"),
                            space.LookupParameter("Количество ламп 2")
                        };
                        var fixtureHeight = new[]
                        {
                            space.LookupParameter("Высота светильников"),
                            space.LookupParameter("Высота светильников 2")
                        };
                        var fixtures = pair.Value?.Take(2).ToArray();
                        for (var i = 0; i < 2; i++)
                        {
                            var flag1 = new[]
                            {
                                types[i].SetEmptyValue(),
                                counts[i].SetEmptyValue(),
                                countsLamp[i].SetEmptyValue(),
                                fixtureHeight[i].SetEmptyValue()
                            };
                            if (fixtures is null || i == 1 && fixtures.Length < 2)
                                continue;
                            var type = fixtures[i].Key;
                            var count = fixtures[i].Value.Count;
                            var h = fixtures[i].Value.Heigth;
                            var flag = new[]
                            {
                                types[i].Set(type),
                                counts[i].Set(count),
                                countsLamp[i].Set(1),
                                fixtureHeight[i].Set(h)
                            };
                        }
                    }

                    tr.Commit();
                }
            }
            catch (Exception e)
            {
                message += e.Message + '\n' + e.StackTrace;
                result = Result.Failed;
            }
            finally
            {
            }

            return result;
        }
    }
}
