using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class UpdateParametersOfElectricalSystemIExernalCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var doc = uiDoc.Document;
            var app = uiApp.Application;
            var result = Result.Succeeded;

            var parameterUpdater = new UpdaterParameters<ElectricalSystem>(doc, BuiltInCategory.OST_ElectricalCircuit);
            //Длина труб для спецификации
            parameterUpdater.AddAction(new SetLenghtForTubeOfElectricalSystemsExternalCommand());
            //Режим траектории электрической цепи
            parameterUpdater.AddAction(new SetModeOfElectricalSystemToAllElementsExternalCommand());

            try
            {
                using (var tr = new Transaction(doc))
                {
                    tr.Start("UpdateParametersOfElectricalSystem");
                    parameterUpdater.Execute();
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
