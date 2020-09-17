using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;

namespace ElectricityRevitPlugin.UpdateParametersInCircuits
{
    public class SetToFalseBooleanParametersForAddedElementsDynamicModelUpdater : IUpdater 
    {
        private static AddInId _appId;
        private static UpdaterId _updaterId;

        public ElementFilter GetElementFilter()
        {
            var ef = new ElementCategoryFilter(BuiltInCategory.OST_ElectricalCircuit);
            return ef;
        }

        public SetToFalseBooleanParametersForAddedElementsDynamicModelUpdater(AddInId id)
        {
            _appId = id;
            _updaterId = new UpdaterId(_appId, new Guid("B2F79AB5-47F3-4674-8505-BB8D21351CBA"));
        }

        public void Execute(UpdaterData data)
        {
            try
            {
                var doc = data.GetDocument();
                var systems = data
                    .GetAddedElementIds()
                    .Select(id => doc.GetElement(id))
                    .OfType<ElectricalSystem>();

                foreach (var system in systems)
                {
                    foreach (Parameter parameter in system.Parameters)
                    {
                        if (!parameter.HasValue && parameter.StorageType == StorageType.Integer &&
                            parameter.UserModifiable && parameter.Definition.ParameterType == ParameterType.YesNo)
                            parameter.Set(0);
                    }
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
            return nameof(SetToFalseBooleanParametersForAddedElementsDynamicModelUpdater);
        }

        public string GetAdditionalInformation()
        {
            return "Установка значения параметров Резервные и контрольные цепи в False";
        }
    }
}
