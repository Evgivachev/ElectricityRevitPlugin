namespace MarkingElectricalSystems.Infrastructure;

using System;
using System.Collections.Generic;
using System.Linq;
using Application;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Domain;
using Models;
using Element = Domain.Element;

public class ElementsRepository(Application application, UIApplication uiApplication, UIDocument uiDocument) : IElementsRepository
{
    private readonly Document _doc = uiDocument.Document;
    private readonly List<int> _addedElementIds = [];

    public IReadOnlyCollection<ElectricalSystem> GetSelectedElectricalSystems()
    {
        var categoryIds = new[]
            {
                BuiltInCategory.OST_ElectricalEquipment,
                BuiltInCategory.OST_LightingFixtures,
                BuiltInCategory.OST_ElectricalFixtures,
                BuiltInCategory.OST_GenericAnnotation,
                BuiltInCategory.OST_LightingDevices
            }
            .Cast<int>()
            .ToHashSet();

        var doc = uiDocument.Document;
        var currentSelection = uiDocument.Selection;

        var elSystems = currentSelection.GetElementIds()
            .Where(id =>
            {
                var el = doc.GetElement(id);
                return el.Category?.Id.IntegerValue is not null && categoryIds.Contains(el.Category.Id.IntegerValue);
            })
            .Select(id => doc.GetElement(id))
            .OfType<FamilyInstance>()
            .SelectMany(el =>
                {
                    if (el is not AnnotationSymbol)
                        return el.MEPModel?
                                   .GetElectricalSystems()
                                   .Where(s => s.BaseEquipment.Id.IntegerValue != el.Id.IntegerValue)
                               ?? [];
                    var ids = el.LookupParameter("ID цепей")?.AsString();
                    if (string.IsNullOrEmpty(ids))
                        return [];
                    var systemsFromAnnotation = ids?
                        .Split("\n\r".ToCharArray())
                        .Select(uid => doc.GetElement(uid))
                        .OfType<Autodesk.Revit.DB.Electrical.ElectricalSystem>();
                    return systemsFromAnnotation;
                }
            )
            .Where(x => x != null)
            .Distinct(new ElementEqualityComparer<Autodesk.Revit.DB.Electrical.ElectricalSystem>())
            .Select(x => new ElectricalSystem()
            {
                Id = x.Id.IntegerValue,
                CategoryId = x.Category.Id.IntegerValue,
            })
            .ToArray();
        return elSystems;
    }
    public IReadOnlyCollection<Element> GetSelectedElements(IReadOnlyCollection<int>? categoryIds)
    {
        var doc = uiDocument.Document;
        var currentSelection = uiDocument.Selection;
        var categoryIdSet = categoryIds is null
            ? null
            : new HashSet<int>(categoryIds);
        var selectedElements = currentSelection.GetElementIds()
            .Where(id =>
            {
                var el = doc.GetElement(id);
                return categoryIdSet is null
                       || el.Category?.Id.IntegerValue is not null && categoryIdSet.Contains(el.Category.Id.IntegerValue);
            })
            .Select(id => doc.GetElement(id) as FamilyInstance)
            .Where(x => x != null)
            .Select(x => new Element()
            {
                Id = x!.Id.IntegerValue,
                CategoryId = x.Category.Id.IntegerValue,
            })
            .ToArray();
        return selectedElements;
    }
    
    public int GetFamilySymbolId(string name)
    {
        using var fc = new FilteredElementCollector(_doc);
        return fc.OfClass(typeof(FamilySymbol))
            .First(el => el.Name == name)
            .Id.IntegerValue;
    }
    public IReadOnlyCollection<int> PromptForFamilyInstancePlacement(int familySymbolId)
    {
        application.DocumentChanged += OnDocumentChanged;
        _addedElementIds.Clear();
        var familySymbol = _doc.GetElement(new ElementId(familySymbolId)) as FamilySymbol;
        try
        {
            uiDocument.PromptForFamilyInstancePlacement(familySymbol);
        }
        catch (Exception)
        {
            // ignored
        }
        finally
        {
            application.DocumentChanged -= OnDocumentChanged;
        }
        
        return _addedElementIds;
    }
    
    private void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
    {
        _addedElementIds.AddRange(
            e.GetAddedElementIds()
                .Select(id => id.IntegerValue));
    }
}
