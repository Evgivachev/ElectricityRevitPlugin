namespace ElectricityRevitPlugin.Updaters
{
    using System;
    using System.Linq;
    using System.Windows;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;
    using Extensions;
    using UpdateParametersInCircuits;

    public class LoadName : MyUpdater
    {
        private readonly Guid _isProhibitChangesGuid = new Guid("5de14719-6968-4655-9457-94825e70b623");

        public LoadName(AddInId id)
            : base(id)
        {
        }

        protected override Guid UpdaterGuid { get; } = new Guid("FEDA2F29-FFFF-4478-A095-450CF8A51AE1");

        protected override string Name { get; } = "Обновление Наименование нагрузки для электрической цепи";
        protected override ChangePriority ChangePriority { get; } = ChangePriority.Annotations;

        protected override string AdditionalInformation { get; } =
            "Обновление Наименование нагрузки для электрической цепи";

        public override ElementFilter ElementFilter { get; } = new ElementCategoryFilter(BuiltInCategory.OST_ElectricalCircuit);

        protected override void ExecuteInner(UpdaterData data)
        {
            try
            {
                var doc = data.GetDocument();
                var command = new SetLoadNameForElectricalSystemsExternalCommand();
                var elements = data
                    .GetModifiedElementIds()
                    .Concat(data.GetAddedElementIds())
                    .Select(x => doc.GetElement(x));
                foreach (var el in elements)
                {
                    ElectricalSystem system = null;
                    if (el is ElectricalSystem electricalSystem)
                        system = electricalSystem;
                    else if (el is FamilyInstance fi)
                        system = fi.GetPowerElectricalSystem();
                    if (system != null)
                    {
                        var isProhibitChanges = system.get_Parameter(_isProhibitChangesGuid);
                        if (isProhibitChanges.AsInteger() == 0)
                            command.UpdateParameters(system);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}\n{e.StackTrace}");
            }
        }
    }
}
