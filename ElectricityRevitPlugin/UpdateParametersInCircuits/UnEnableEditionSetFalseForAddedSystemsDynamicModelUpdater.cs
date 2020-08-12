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
    public class UnEnableEditionSetFalseForAddedSystemsDynamicModelUpdater : IUpdater
    {
        private static AddInId _appId;
        private static UpdaterId _updaterId;
        readonly Guid _isUnEditable = new Guid("be64f474-c030-40cf-9975-6eaebe087a84");

        public ElementFilter GetElementFilter()
        {
            var ef = new ElementCategoryFilter(BuiltInCategory.OST_ElectricalCircuit);
            return ef;
        }

        public UnEnableEditionSetFalseForAddedSystemsDynamicModelUpdater(AddInId id)
        {
            _appId = id;
            _updaterId = new UpdaterId(_appId, new Guid("54F360D4-63B6-4DAE-8A35-586B62177D6A"));
        }

        public void Execute(UpdaterData data)
        {
            try
            {
                var doc = data.GetDocument();
                var systems = data
                    .GetAddedElementIds()
                    .Select(id=>doc.GetElement(id))
                    .OfType<ElectricalSystem>();

                foreach (var system in systems)
                {
                    var isUnEditableParameter = system.get_Parameter(_isUnEditable);

                    var systemHasValueIsUnEditable = isUnEditableParameter.HasValue;
                    if (!systemHasValueIsUnEditable)
                        using (var tr = new Transaction(doc))
                        {
                            tr.Start("Установка значения параметра для цепи \"Запретить изменение\" в False");
                            isUnEditableParameter.Set(0);
                            tr.Commit();
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
            return nameof(UnEnableEditionSetFalseForAddedSystemsDynamicModelUpdater);
        }

        public string GetAdditionalInformation()
        {
            return "Установка значения параметра для цепи \"Запретить изменение\" в False";
        }
    }
}
