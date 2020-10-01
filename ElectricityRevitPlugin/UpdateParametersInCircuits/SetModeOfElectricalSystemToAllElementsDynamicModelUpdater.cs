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
    public class SetModeOfElectricalSystemToAllElementsDynamicModelUpdater : IUpdater
    {
        private static AddInId _appId;
        private static UpdaterId _updaterId;
        public SetModeOfElectricalSystemToAllElementsDynamicModelUpdater(AddInId id)
        {
            _appId = id;
            _updaterId = new UpdaterId(_appId, new Guid("265CFA1D-4FB1-4A8E-8B85-9FAD661844DB"));
        }

        public void Execute(UpdaterData data)
        {
            try
            {
                var doc = data.GetDocument();
                var command = new SetModeOfElectricalSystemToAllElementsExternalCommand();
                var systems = data.GetAddedElementIds()
                    .Select(x => doc.GetElement(x) as ElectricalSystem);
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
            return nameof(SetModeOfElectricalSystemToAllElementsDynamicModelUpdater);
        }

        public string GetAdditionalInformation()
        {
            return "Обновление траектории электрической цепи на все элементы";
        }

        public ElementFilter GetElementFilter()
        {
            var ef = new ElementCategoryFilter(BuiltInCategory.OST_ElectricalCircuit);
            return ef;
        }
    }
}
