using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;

namespace ElectricityRevitPlugin.UpdateParametersInCircuits
{
    class UpdateLossVoltageOfElectricalCircuitsDynamicModelUpdater :IUpdater
    {
        private static AddInId _appId;
        private static UpdaterId _updaterId;

        public ElementFilter GetElementFilter()
        {
            var ef = new ElementCategoryFilter(BuiltInCategory.OST_ElectricalCircuit);
            return ef;
        }

        public UpdateLossVoltageOfElectricalCircuitsDynamicModelUpdater(AddInId id)
        {
            _appId = id;
            _updaterId = new UpdaterId(_appId, new Guid("92CA0F1E-7BB8-4249-9064-751C8691BA90"));
        }

        public void Execute(UpdaterData data)
        {
            try
            {
                var doc = data.GetDocument();
                var command = new LossVoltageOfElectricalSystemExternalCommand();
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
