using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using ElectricityRevitPlugin.UpdateParametersInCircuits;

namespace ElectricityRevitPlugin.Updaters
{
    public class LengthOfElectricalSystem : MyUpdater
    {
        public LengthOfElectricalSystem(AddInId id) : base(id)
        {
        }
        readonly Guid _isUnEditable = new Guid("be64f474-c030-40cf-9975-6eaebe087a84");
        protected override Guid UpdaterGuid { get; } = new Guid("3018BA6E-9571-4EB9-9D7A-75DDB66CDC85");
        protected override void ExecuteInner(UpdaterData data)
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
                    var isUnEditable = system.get_Parameter(_isUnEditable).AsInteger() == 1;
                    if (isUnEditable)
                        continue;
                    command.UpdateParameters(system);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}\n{e.StackTrace}");
            }
        }

        protected override string Name { get; } = "Обновление длины электрической цепи";
        protected override ChangePriority ChangePriority { get; } = ChangePriority.MEPSystems;
        protected override string AdditionalInformation { get; } = "Обновление длины электрической цепи";
        public override ElementFilter ElementFilter { get; } = new ElementCategoryFilter(BuiltInCategory.OST_ElectricalCircuit);
    }
}
