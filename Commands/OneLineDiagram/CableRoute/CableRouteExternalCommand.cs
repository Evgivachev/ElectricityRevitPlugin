namespace Diagrams.CableRoute
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;

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
