using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;

namespace ElectricityRevitPlugin.Updaters
{
    public abstract class MyUpdater : IUpdater
    {
        public MyUpdater(AddInId id)
        {
            AppId = id;
        }
        protected AddInId AppId;

        private UpdaterId _updaterId;

        protected UpdaterId UpdaterId => _updaterId ?? (_updaterId = new UpdaterId(AppId, UpdaterGuid));

        protected abstract Guid UpdaterGuid { get; }
        protected abstract void ExecuteInner(UpdaterData data);
        protected abstract string Name { get; }
        protected abstract ChangePriority ChangePriority { get; }

        protected abstract string AdditionalInformation { get; }

        public abstract ElementFilter ElementFilter { get; }


        public void Execute(UpdaterData data)
        {
#if DEBUG
            
            var infoStringBuilder = new StringBuilder();
            infoStringBuilder.AppendLine($"Запуск обновления {Name}");
            infoStringBuilder.AppendLine($"Добавленные элементы:");
            var doc = data.GetDocument();
            foreach (var addedElementId in data.GetAddedElementIds())
            {
                var el = doc.GetElement(addedElementId);
                infoStringBuilder.AppendLine($"id \"{addedElementId}\", name: \"{el.Name}\"");
            }
            infoStringBuilder.AppendLine($"Модифицированные элементы:");
            foreach (var addedElementId in data.GetModifiedElementIds())
            {
                var el = doc.GetElement(addedElementId);
                infoStringBuilder.AppendLine($"id \"{addedElementId}\", name: \"{el.Name}\"");
            }
            infoStringBuilder.AppendLine($"Удаленные элементы:");
            foreach (var addedElementId in data.GetDeletedElementIds())
            {
                var el = doc.GetElement(addedElementId);
                infoStringBuilder.AppendLine($"id \"{addedElementId}\", name: \"{el.Name}\"");
            }
            MessageBox.Show(infoStringBuilder.ToString());


#endif
            ExecuteInner(data);

#if DEBUG
            MessageBox.Show($"Конец обновления {Name}");
#endif


        }

        public UpdaterId GetUpdaterId()
        {
            return UpdaterId;
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority;
        }

        public string GetUpdaterName()
        {
            return Name;
        }

        public string GetAdditionalInformation()
        {
            return AdditionalInformation;
        }
    }
}
