namespace ElectricityRevitPlugin.Updaters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Autodesk.Revit.DB;

public abstract class MyUpdater : IUpdater
{
    public static List<IUpdater> RegistredUpdaters = new();

    private UpdaterId _updaterId;
    protected AddInId AppId;

    protected MyUpdater(AddInId id)
    {
        AppId = id;
    }

    protected UpdaterId UpdaterId => _updaterId ?? (_updaterId = new UpdaterId(AppId, UpdaterGuid));

    protected abstract Guid UpdaterGuid { get; }
    protected abstract string Name { get; }
    protected abstract ChangePriority ChangePriority { get; }

    protected abstract string AdditionalInformation { get; }

    public abstract ElementFilter ElementFilter { get; }

    public void Execute(UpdaterData data)
    {
        var doc = data.GetDocument();
        var updateLocker = UpdateLocker.GetUpdateLocker();
        if (!updateLocker.IsLocked(doc))
            using (updateLocker.Lock())
            {
#if DEBUG
                var infoStringBuilder = new StringBuilder();
                infoStringBuilder.AppendLine($"Запуск обновления {Name}");
                infoStringBuilder.AppendLine($"Добавленные элементы:");
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
                MessageBox.Show($"Конец обновления {Name}");
#endif
                ExecuteInner(data);
            }
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

    protected abstract void ExecuteInner(UpdaterData data);

    public void RegisterUpdater(Document doc)
    {
        //зарегистрированнык обновления
        var registredUpdaters =
            UpdaterRegistry.GetRegisteredUpdaterInfos(doc)
                .ToDictionary(x => x.UpdaterName);
        if (registredUpdaters.ContainsKey(Name))
            UpdaterRegistry.UnregisterUpdater(UpdaterId);
        registredUpdaters =
            UpdaterRegistry.GetRegisteredUpdaterInfos()
                .ToDictionary(x => x.UpdaterName);
        if (registredUpdaters.ContainsKey(Name))
            UpdaterRegistry.UnregisterUpdater(UpdaterId);
        try
        {
            UpdaterRegistry.RegisterUpdater(this, doc, true);
            RegistredUpdaters.Add(this);
        }
        catch (Exception e)
        {
            MessageBox.Show($"Не удалось зарегестрировать средство обновления {Name}");
        }
    }
}
