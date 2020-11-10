using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using ElectricityRevitPlugin.GroupByGost;

namespace ElectricityRevitPlugin.Updaters
{
    class GroupByGost :MyUpdater
    {
        public GroupByGost(AddInId id) : base(id)
        {
        }

        protected override Guid UpdaterGuid { get; } = new Guid("4A58C581-9304-4DDD-8606-6E5B0F4E252C");
        protected override void ExecuteInner(UpdaterData data)
        {
            try
            {
                var doc = data.GetDocument();
                var ids = data.GetModifiedElementIds();
                var updater = new GroupByGostExternalCommand {Doc = doc};
                var fis = new List<FamilyInstance>();

                foreach (var id in ids)
                {
                    var el = doc.GetElement(id);
                    if (el is ElectricalSystem es)
                    {
                        updater.SetValuesToElement(es);

                    }
                    else if (el is FamilyInstance fi)
                    {
                        fis.Add(fi);
                    }
                }
                doc.Regenerate();
                foreach (var fi in fis)
                {
                    updater.SetValuesToElement(fi);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}\n{e.StackTrace}");
            }
        }

        protected override string Name { get; } = "Обновление групп по ГОСТ";
        protected override ChangePriority ChangePriority { get; } = ChangePriority.Annotations;
        protected override string AdditionalInformation { get; } = "Обновление групп по ГОСТ";

        public override ElementFilter ElementFilter =>
            new ElementParameterFilter(
                ParameterFilterRuleFactory.CreateSharedParameterApplicableRule("Номер группы по ГОСТ"));
    }
}
