namespace MarkingElectricalSystems;

using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Models;
using PikTools.LogWindow.Abstractions;
using PikTools.LogWindow.Models;
using RxBim.Command.Revit;
using RxBim.Shared;
using Services;

/// <inheritdoc />
[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class Cmd : RxBimCommand
{
    private readonly List<ElementId> _addedElementIds = new();

    /// <summary>
    /// Команда плагина
    /// </summary>
    public PluginResult ExecuteCommand(Application app, UIDocument uiDoc, IDisplayLogger displayLogger)
    {
        var doc = uiDoc.Document;
        var result = PluginResult.Failed;
        var trueCategories = new HashSet<BuiltInCategory>()
        {
            BuiltInCategory.OST_ElectricalEquipment,
            BuiltInCategory.OST_LightingFixtures,
            BuiltInCategory.OST_ElectricalFixtures,
            BuiltInCategory.OST_GenericAnnotation,
            BuiltInCategory.OST_LightingDevices
        };
        var currentSelection = uiDoc.Selection;
        var selectedElements = currentSelection.GetElementIds()
            .Where(id =>
            {
                var el = doc.GetElement(id);
                return Enum.TryParse<BuiltInCategory>(el.Category?.Id.IntegerValue.ToString(), out var category)
                       && trueCategories.Contains(category);
            })
            .Select(id => doc.GetElement(id) as FamilyInstance)
            .Where(x => x != null);
        var elSystems = selectedElements
            .SelectMany(el =>
                {
                    if (el is not AnnotationSymbol)
                        return
                            el?.MEPModel?.ElectricalSystems?
                                .OfType<ElectricalSystem>()
                                .Where(s => s.BaseEquipment.Id.IntegerValue != el.Id.IntegerValue)
                            ?? Enumerable.Empty<ElectricalSystem>();
                    var ids = el.LookupParameter("ID цепей")?.AsString();
                    if (string.IsNullOrEmpty(ids))
                        return Enumerable.Empty<ElectricalSystem>();
                    var systemsFromAnnotation = ids?
                        .Split("\n\r".ToCharArray())
                        .Select(uid => doc.GetElement(uid))
                        .OfType<ElectricalSystem>();
                    return systemsFromAnnotation;
                }
            )
            .Where(x => x != null)
            .Distinct(new ElementEqualityComparer<ElectricalSystem>())
            .ToArray();
        if (elSystems.Length == 0)
        {
            displayLogger.AddMessage(new CommonErrorMessage("Отсутствуют выделенные подключенные элементы электрических цепей", "Ошибка"));
            displayLogger.Show(GetType().Name, true);
            return result;
        }

        var symbol = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .First(el => el.Name == "Марка групп цепей")
            as FamilySymbol;
        app.DocumentChanged += OnDocumentChanged;
        _addedElementIds.Clear();
        try
        {
            uiDoc.PromptForFamilyInstancePlacement(symbol);
        }
        catch (Exception)
        {
            // ignored
        }
        finally
        {
            app.DocumentChanged -= OnDocumentChanged;
        }

        var parameterSetter = new MarkParameterSetter();
        using var tr = new Transaction(doc);
        tr.Start("Установка параметров");
        parameterSetter.SetParameters(doc, _addedElementIds, elSystems);
        result = tr.Commit() == TransactionStatus.Committed
            ? PluginResult.Succeeded
            : PluginResult.Failed;
        return result;
    }

    private void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
    {
        _addedElementIds.AddRange(
            e.GetAddedElementIds());
    }
}
