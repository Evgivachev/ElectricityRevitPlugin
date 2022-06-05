namespace ElectricityRevitPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;
    using Autodesk.Revit.DB.Structure;
    using Autodesk.Revit.UI;

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    //Добавить резервные группы к электрическому щиту
    class AddReserveElectricalSystemExternalCommand : IExternalCommand
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
                    tr.Start("Temp");
                    var selection = uiDoc.Selection;
                    var selectedIds = selection.GetElementIds();
                    var selectedElements = selectedIds
                        .Select(x => doc.GetElement(x))
                        .OfType<FamilyInstance>();
                    var reserveSymbol = doc.GetElement(new ElementId(18098108)) as FamilySymbol;
                    foreach (var element in selectedElements)
                    {
                        var n = GetCountOfReserveGroup(element);
                        var locations = GetLocationOfReserveGroup(element, n);
                        foreach (var location in locations)
                        {
                            var nGr = doc.Create.NewFamilyInstance(location, reserveSymbol,
                                StructuralType.NonStructural);
                            var nEs = ElectricalSystem.Create(doc, new List<ElementId>() { nGr.Id },
                                ElectricalSystemType.PowerCircuit);
                            nEs.SelectPanel(element);
                        }

                        //var flag = element.MEPModel.AssignedElectricalSystems.Insert(nEs);
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

        private int GetCountOfReserveGroup(FamilyInstance shield)
        {
            var assigned = shield.MEPModel.GetAssignedElectricalSystems();
            if (assigned is null)
                return 0;
            var count = assigned.Count;
            var n = (int)Math.Min(4, Math.Ceiling(count / 10.0));
            return n;
        }

        private XYZ[] GetLocationOfReserveGroup(FamilyInstance shield, int n)
        {
            var d = 1;
            if (!(shield.Location is LocationPoint l))
                throw new NullReferenceException();
            var r = l.Rotation;
            var basePoint = l.Point.Add(new XYZ(d * Math.Sin(r), d * Math.Cos(r), 0));
            var dd = UnitUtils.ConvertToInternalUnits(20, UnitTypeId.Millimeters);
            var deltaAngle0 = Math.PI / 2;
            var result = new XYZ[n];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = basePoint.Add(new XYZ(dd * Math.Sin(deltaAngle0 * i), dd * Math.Cos(deltaAngle0 * i), 0));
            }

            return result;
        }
    }
}
