namespace Diagrams.CableRoute
{
    using System.Linq;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Selection;

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class CableRouteExternalCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData?.Application;
            var uiDoc = uiApp?.ActiveUIDocument;
            var app = uiApp?.Application;
            var doc = uiDoc?.Document;
            var director = new CableRouterDirector();
            director.ElectricalSystemsFinder = new ElectricalSystemsFinderBySelection();
            var result = director.DoWork(commandData);
            return result;
        }
    }
}
