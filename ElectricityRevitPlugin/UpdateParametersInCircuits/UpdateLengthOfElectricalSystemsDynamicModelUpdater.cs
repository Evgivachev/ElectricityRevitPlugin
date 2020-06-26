using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Macros;

namespace ElectricityRevitPlugin.UpdateParametersInCircuits
{
    public class UpdateLengthOfElectricalSystemsDynamicModelUpdater : IUpdater
    {
        private static AddInId _appId;
        private static UpdaterId _updaterId;

        public ElementFilter GetElementFilter()
        {
            var ef = new ElementCategoryFilter(BuiltInCategory.OST_ElectricalCircuit);
            return ef;
        }

        public UpdateLengthOfElectricalSystemsDynamicModelUpdater(AddInId id)
        {
            _appId = id;
            _updaterId = new UpdaterId(_appId, new Guid("3018BA6E-9571-4EB9-9D7A-75DDB66CDC85"));
        }

        public void Execute(UpdaterData data)
        {
            try
            {
                var doc = data.GetDocument();
                var command = new SetLengthForElectricalSystemsExternalCommand();
                command.Doc = doc;
                var systems = data
                    .GetModifiedElementIds()
                    .Select(x => doc.GetElement(x) as ElectricalSystem)
                    .Concat(data.GetAddedElementIds().Select(x => doc.GetElement(x) as ElectricalSystem));
                foreach (var system in systems)
                {
                    command.UpdateParameters(system);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}\n{e.StackTrace}");
            }
            
        }

        public UpdaterId GetUpdaterId()
        {
            return _updaterId;
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.MEPSystems;
        }

        public string GetUpdaterName()
        {
            return nameof(UpdateLengthOfElectricalSystemsDynamicModelUpdater);
        }

        public string GetAdditionalInformation()
        {
            return "Обновление длин цепей";
        }
    }
}
