namespace ElectricityRevitPlugin.Updaters.Application;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using CommonUtils;
using Triggers;

public class RegisterUpdatersService : ISyncBackGroundService
{
    private readonly UIControlledApplication _uiControlledApplication;
    private readonly UpdaterBase[] _updaters;
    private readonly IUpdaterTrigger[] _triggers;

    public RegisterUpdatersService(
        UIControlledApplication uiControlledApplication,
        IEnumerable<UpdaterBase> updaters,
        IEnumerable<IUpdaterTrigger> triggers)
    {
        _uiControlledApplication = uiControlledApplication;
        _updaters = updaters.ToArray();
        _triggers = triggers.ToArray();
        
    }

    public void Execute()
    {
        _uiControlledApplication.ControlledApplication.DocumentOpened += RegisterUpdatersOnDocumentOpened;
    }
    
    private void RegisterUpdatersOnDocumentOpened(object? sender, DocumentOpenedEventArgs e)
    {
        var triggersMap = _triggers
            .SelectMany(trigger =>
            {
                var linkedUpdaterTypes = trigger
                    .GetType().GetCustomAttributes(true)
                    .OfType<UsedWithAttribute>()
                    .Select(a => a.UpdaterName)
                    .Select(updaterType => new { updaterType, trigger });

                return linkedUpdaterTypes;
            })
            .ToLookup(x => x.updaterType, x => x.trigger);

        var doc = e.Document;
        foreach (var updater in _updaters)
        {
            try
            {
                var updaterType = updater.GetType();
                var triggers = triggersMap.Contains(updaterType.Name)
                    ? triggersMap[updaterType.Name]
                    : [];
                RegisterUpdater(doc, updater, triggers);
            }
            catch (Exception exception)
            {
                TaskDialog.Show("Ошибка", exception.Message + "\n" + exception.StackTrace);
            }
        }
    }

    private void RegisterUpdater(Document doc, UpdaterBase updater, IEnumerable<IUpdaterTrigger> triggers)
    {
        var registeredUpdaters = UpdaterRegistry.GetRegisteredUpdaterInfos(doc)
            .ToDictionary(x => x.UpdaterName);
        if (registeredUpdaters.ContainsKey(updater.GetUpdaterName()))
            UpdaterRegistry.UnregisterUpdater(updater.GetUpdaterId());

        try
        {
            UpdaterRegistry.RegisterUpdater(updater, doc, updater.IsOptional);
            foreach (var updaterTrigger in triggers)
            {
                UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), updaterTrigger.ElementFilter, updaterTrigger.ChangeType);
            }

        }
        catch (Exception e)
        {
            TaskDialog.Show("Ошибка",
                $"Не удалось зарегистрировать средство обновления {updater.GetUpdaterName()}" + "\n" + $"{e.Message}");
        }
    }
}