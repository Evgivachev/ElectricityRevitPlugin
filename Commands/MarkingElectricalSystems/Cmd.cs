namespace MarkingElectricalSystems;

using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Models;
using PikTools.Ui.Abstractions;
using RxBim.Di;
using Services;

/// <inheritdoc />
[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class Cmd : IExternalCommand, IExternalCommandAvailability
{
    private readonly List<ElementId> _addedElementIds = new();

    /// <inheritdoc />
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var container = new SimpleInjectorContainer();
        new Config().Configure(container);
        var notificationService = container.GetService<INotificationService>();
        var uiApp = commandData.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var app = uiApp.Application;
        var doc = uiDoc.Document;
        var result = Result.Failed;
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
                        return el?.MEPModel?
                                   .GetElectricalSystems()
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
            notificationService.ShowMessage(GetType().Name, "Отсутствуют выделенные подключенные элементы электрических цепей",
                NotificationType.Error);
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
            ? Result.Succeeded
            : Result.Failed;
        return result;
    }

    private void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
    {
        _addedElementIds.AddRange(
            e.GetAddedElementIds());
    }

    /// <inheritdoc />
    public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
    {
        return applicationData.ActiveUIDocument?.Document is not null;
    }
}
