/*using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace MarkingElectricalSystems
{
    public class ElectricalElementsSelector
    {
        private readonly UIDocument _uiDoc;
        public ElectricalElementsSelector(UIDocument uiDoc)
        {
            _uiDoc = uiDoc;
            

        }
        public IList<Reference> GetElements()
        {
            var selection = _uiDoc.Selection;
            var elements = selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, new ElectricalDevicesSelectionFilter());
            return elements;
        }
    }
}*/


