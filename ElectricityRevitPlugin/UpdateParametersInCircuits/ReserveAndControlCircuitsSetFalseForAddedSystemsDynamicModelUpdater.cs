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
    public class ReserveAndControlCircuitsSetFalseForAddedSystemsDynamicModelUpdater : IUpdater
    {
        readonly Guid _isReserveCircuitGuid = new Guid("cd2dc469-276a-40f4-bd34-c6ab2ae05348");
        readonly Guid _isControlCircuitGuid = new Guid("0f13e1e5-71bb-4b0f-b3dc-18054c25e1ee");

        private static AddInId _appId;
        private static UpdaterId _updaterId;

        public ElementFilter GetElementFilter()
        {
            var ef = new ElementCategoryFilter(BuiltInCategory.OST_ElectricalCircuit);
            return ef;
        }

        public ReserveAndControlCircuitsSetFalseForAddedSystemsDynamicModelUpdater(AddInId id)
        {
            _appId = id;
            _updaterId = new UpdaterId(_appId, new Guid("E4B5B915-4274-42E7-BE4E-AC3866326E92"));
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
                    var isReserveParameter = system.get_Parameter(_isReserveCircuitGuid);
                    var isControlParameter = system.get_Parameter(_isControlCircuitGuid);
                    if(isReserveParameter is null || isControlParameter is null)
                        throw new NullReferenceException("Отсутствуют общие параметры \"Резервная группа\" или \"Контрольные цепи\" у Электрических цепей");
                    if (!isReserveParameter.HasValue)
                        isReserveParameter.Set(0);
                    if (!isControlParameter.HasValue)
                        isControlParameter.Set(0);
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
            return nameof(ReserveAndControlCircuitsSetFalseForAddedSystemsDynamicModelUpdater);
        }

        public string GetAdditionalInformation()
        {
            return "Установка значения параметров Резервные и контрольные цепи в False";
        }


    }
}
