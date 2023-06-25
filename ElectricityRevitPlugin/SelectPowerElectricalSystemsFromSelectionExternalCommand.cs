namespace ElectricityRevitPlugin
{
    using System;
    using System.Linq;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using CommonUtils.Extensions;
    using Extensions;

    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    class SelectPowerElectricalSystemsFromSelectionExternalCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var app = uiApp.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var doc = uiDoc.Document;
            var result = Result.Succeeded;
            try
            {
                var selection = uiDoc.Selection;
                var elementIds = selection
                    .GetElementIds();
                var elSystems = elementIds
                    .Select(e => doc.GetElement(e))
                    .Where(e => e != null)
                    .OfType<FamilyInstance>()
                    .Select(f => FamilyInstanceExtension.GetPowerElectricalSystem(f))
                    .Where(x => x != null);
                var elSystemIds = elSystems
                    .Select(x => x.Id)
                    .ToArray();
                selection.SetElementIds(elSystemIds);
            }
            catch (Exception e)
            {
                message += e.Message + '\n' + e.StackTrace;
                result = Result.Failed;
            }

            return result;
        }
    }
}
