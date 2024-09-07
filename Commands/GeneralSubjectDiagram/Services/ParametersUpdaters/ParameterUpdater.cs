namespace GeneralSubjectDiagram.Services.ParametersUpdaters;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Autodesk.Revit.DB;
using CommonUtils.Extensions;
using CommonUtils.Helpers;
using ViewModels;

public abstract class ParameterUpdater
{
    public static Guid ReflectionClassNameGuid = new("6c36d5e8-7863-4efb-accf-894a5aa95cc1");
    protected Dictionary<string, Func<object, dynamic>> FuncParametricDictionary = new();

    protected Dictionary<dynamic, dynamic> ParametersDictionary = new();


    public abstract ObservableCollection<CheckableItem> GetValidateElements(Document document);

    /// <summary>
    /// Имя семейство для вставки
    /// </summary>
    public abstract string FamilyNameToInsert { get; }

    public virtual FamilyInstance InsertInstance(FamilySymbol familySymbol, XYZ point)
    {
        var instance = familySymbol.Document.Create.NewFamilyInstance(point, familySymbol, familySymbol.Document.ActiveView);
        return instance;
    }

    public void SetParameters(Element toElement, Element baseElement)
    {
        var p = toElement.get_Parameter(SharedParametersFile.Zapretit_Izmenenie)?.AsInteger() == 1;
        if (p)
            return;
        SetSameParameters(toElement, baseElement);
        toElement.Document.Regenerate();
        SetParametersFromParametersDictionary(toElement, baseElement);
        SetParametersFromFuncDictionary(toElement, baseElement);
    }

    protected virtual void SetParametersFromParametersDictionary(Element toElement, Element baseElement)
    {
        foreach (var pair in ParametersDictionary)
        {
            var fromP = (Parameter)baseElement.get_Parameter(pair.Key);
            var toP = (Parameter)toElement.get_Parameter(pair.Value);
            if (fromP is null || toP is null)
                throw new NullReferenceException();
            toP.Set(fromP.GetValueDynamic());
        }
    }

    private void SetParametersFromFuncDictionary(Element toElement, Element baseElement)
    {
        foreach (var func in FuncParametricDictionary)
        {
            var toP = toElement.LookupParameter(func.Key);
            if (toP is null)
                throw new NullReferenceException();
            var value = func.Value.Invoke(baseElement);
            if (value is null)
                toP.ResetValue();
            else
                toP.Set(value);
        }
    }

    private void SetSameParameters(Element toElement, Element baseElement)
    {
        foreach (Parameter fromP in baseElement.Parameters)
        {
            if (!fromP.IsShared)
                continue;
            var fromGuid = fromP.GUID;
            var toP = toElement.get_Parameter(fromGuid);
            if (toP is null || toP.IsReadOnly || !toP.IsShared)
                continue;
            var flag = toP.Set(fromP.GetValueDynamic());
            if (!flag)
                Debug.Print($"{toP.Definition.Name} is wrong");
        }

        toElement
            .get_Parameter(SharedParametersFile.ID_Svyazannogo_Elementa)
            .Set(baseElement.Id.IntegerValue.ToString());
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return FamilyNameToInsert;
    }
}
