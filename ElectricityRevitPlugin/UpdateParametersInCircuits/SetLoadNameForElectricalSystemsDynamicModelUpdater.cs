using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using ElectricityRevitPlugin.Extensions;

namespace ElectricityRevitPlugin.UpdateParametersInCircuits
{
    class SetLoadNameForElectricalSystemsDynamicModelUpdater : IUpdater
    {
        private static AddInId _appId;
        private static UpdaterId _updaterId;
        public SetLoadNameForElectricalSystemsDynamicModelUpdater(AddInId id)
        {
            _appId = id;
            _updaterId = new UpdaterId(_appId, new Guid("FEDA2F29-FFFF-4478-A095-450CF8A51AE1"));
        }


        public void Execute(UpdaterData data)
        {
            try
            {
                var doc = data.GetDocument();
                var command = new SetLoadNameForElectricalSystemsExternalCommand();
                var elements = data
                    .GetModifiedElementIds()
                    .Select(x => doc.GetElement(x));
                foreach (var el in elements)
                {
                    ElectricalSystem system = null;
                    if (el is ElectricalSystem electricalSystem)
                        system = electricalSystem;
                    else if (el is FamilyInstance fi)
                        system = fi.GetPowerElectricalSystem();
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
            return nameof(SetLoadNameForElectricalSystemsDynamicModelUpdater);
        }

        public string GetAdditionalInformation()
        {
            return "Обновление Наименование нагрузки для электрической цепи";
        }

        public ElementFilter GetElementFilter()
        {
            var ef = new ElementCategoryFilter(BuiltInCategory.OST_ElectricalCircuit);
            return ef;
        }




    }
}
