namespace ElectricityRevitPluginApp;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using CommonUtils;
using JetBrains.Annotations;
using RxBim.Application.Revit;
using RxBim.Shared;
using Triggers;

/// <summary>
/// app
/// </summary>
[PublicAPI]
public class App : RxBimApplication
{
    private UpdaterBase[] _updaters = null!;
    private IUpdaterTrigger[] _triggers = null!;

    /// <summary>
    /// start
    /// </summary>
    public PluginResult Start(
        UIApplication uiApplication,
        IEnumerable<UpdaterBase> updaters,
        IEnumerable<IUpdaterTrigger> triggers)
    {
        Debugger.Launch();
        _updaters = updaters.ToArray();
        _triggers = triggers.ToArray();
        uiApplication.Application.DocumentOpened += (sender, e) => RegisterUpdatersOnDocumentOpened(sender, e);
        return PluginResult.Succeeded;
    }

    /// <summary>
    /// shutdown
    /// </summary>
    public PluginResult Shutdown()
    {
        return PluginResult.Succeeded;
    }

    private void RegisterUpdatersOnDocumentOpened(object sender, DocumentOpenedEventArgs e)
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
