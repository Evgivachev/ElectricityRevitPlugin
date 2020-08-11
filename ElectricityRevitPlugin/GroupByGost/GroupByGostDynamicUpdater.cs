using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using MoreLinq;

namespace ElectricityRevitPlugin.GroupByGost
{
    class GroupByGostDynamicUpdater : IUpdater
    {
        private UpdaterId _updaterId;
        private static AddInId _appId;

        public GroupByGostDynamicUpdater(AddInId id)
        {
            _appId = id;
            _updaterId = new UpdaterId(_appId, new Guid("4A58C581-9304-4DDD-8606-6E5B0F4E252C"));
        }

        public ElementFilter GetElementFilter()
        {
            var f = new ElementParameterFilter(
                ParameterFilterRuleFactory.CreateSharedParameterApplicableRule("Номер группы по ГОСТ"));
            return f;
        }

        public void Execute(UpdaterData data)
        {
            try
            {
                var doc = data.GetDocument();
                var ids = data.GetModifiedElementIds();
                var updater = new GroupByGostExternalCommand();
                updater.Doc = doc;
                //using (var tr = new Transaction(doc))
                //{
                //    tr.Start("Обновление группы по ГОСТ");
                var fis = new List<FamilyInstance>();
                //Сначала обновить цепи потом семейства

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
                //}
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
            return "Обновление групп по ГОСТ";
        }

        public string GetAdditionalInformation()
        {
            return "Обновление групп по ГОСТ";
        }
    }
}
