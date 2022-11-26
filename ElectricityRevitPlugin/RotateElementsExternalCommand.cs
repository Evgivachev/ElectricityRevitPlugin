namespace ElectricityRevitPlugin
{
    using System;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class RotateElementsExternalCommand : IExternalCommand
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
                    tr.Start("Установка поворота элементов");
                    var selection = uiDoc.Selection;
                    var elementIds = selection.GetElementIds();
                    foreach (var elId in elementIds)
                    {
                        try
                        {
                            var el = doc.GetElement(elId);
                            var location = el.Location as LocationPoint;
                            if (location is null) continue;
                            var line = Line.CreateUnbound(location.Point, new XYZ(0, 0, 1));
                            var k = Math.Round(location.Rotation / Math.PI * 2);
                            var angle = k * Math.PI / 2;
                            el.Location.Rotate(line, -location.Rotation + angle);
                        }
                        catch
                        {
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
