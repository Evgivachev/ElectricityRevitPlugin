namespace Diagrams.CableRoute
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;
    using Autodesk.Revit.UI;

    public class ElectricalSystemsFinder
    {
        public virtual IEnumerable<ElectricalSystem> GetElectricalSystems(
            ExternalCommandData externalCommandData,
            Func<ElectricalSystem, bool> filter)
        {
            var document = externalCommandData.Application.ActiveUIDocument.Document;
            var es = new FilteredElementCollector(document)
                .OfClass(typeof(ElectricalSystem))
                .OfType<ElectricalSystem>();
            if (filter != null)
                es = es.Where(filter);

            return es;
        }
    }

    public class ElectricalSystemsFinderBySelection : ElectricalSystemsFinder
    {
        public override IEnumerable<ElectricalSystem> GetElectricalSystems(
            ExternalCommandData externalCommandData,
            Func<ElectricalSystem, bool> filter)
        {
            var uiDoc = externalCommandData.Application.ActiveUIDocument;
            var doc = uiDoc.Document;
            // Get the element selection of current document.
            var selection = uiDoc.Selection;
            var selectedIds = selection.GetElementIds();
            var laster = new HashSet<int>();
            foreach (var id in selectedIds)
            {
                var es = (doc.GetElement(id) as FamilyInstance)?
                    .MEPModel?
                    .GetElectricalSystems()?
                    .FirstOrDefault();
                if (es != null && !laster.Contains(es.Id.IntegerValue))
                    yield return es;
            }
        }
    }
}
