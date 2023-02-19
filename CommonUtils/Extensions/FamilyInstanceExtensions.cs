namespace CommonUtils.Extensions;

using System;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Comparer;

/// <summary>
/// Методы расширения для <see cref="FamilyInstance"/>
/// </summary>
public static class FamilyInstanceExtensions
{
    /// <summary>
    /// Возвращает питающую сеть элемента
    /// </summary>
    /// <param name="familyInstance">"Элемент</param>
    public static ElectricalSystem? GetPowerElectricalSystem(this FamilyInstance familyInstance)
    {
        try
        {
            var allSystems = familyInstance.MEPModel?.GetElectricalSystems();
            if (allSystems is null
                || allSystems.Count == 0)
                return null;
            var assignedElectricalSystems =
                familyInstance.MEPModel!.GetAssignedElectricalSystems().ToHashSet(new ElectricalSystemsComparer());
           
            if (assignedElectricalSystems is null)
                return allSystems.First();
            if (allSystems.Count == assignedElectricalSystems.Count)
                return null;
            return allSystems
                .First(x => !assignedElectricalSystems.Contains(x));
        }
        catch (Exception e)
        {
            return null;
        }
    }
}
