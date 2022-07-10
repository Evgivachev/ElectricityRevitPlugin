using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalLoadsImportToExcel
{
    public static class FamilyInstanceExtension
    {
        public static ElectricalSystem? GetPowerElectricalSystem(this FamilyInstance familyInstance)
        {
            if (familyInstance == null)
                throw new ArgumentNullException(nameof(familyInstance));
            try
            {
                var allElectricalSystem = familyInstance.MEPModel?.GetElectricalSystems();
                var assignedElectricalSystem = familyInstance.MEPModel?.GetAssignedElectricalSystems();
                
                if (allElectricalSystem is null || allElectricalSystem.Count == 0)
                    return null;
                if (assignedElectricalSystem is null)
                    return allElectricalSystem.First();
                if (allElectricalSystem.Count == assignedElectricalSystem.Count)
                    return null;
                var assignedElectricalSystemIds = assignedElectricalSystem
                    .Select(x => x.Id.IntegerValue)
                    .ToHashSet();
                return allElectricalSystem
                    .First(x => !assignedElectricalSystemIds.Contains(x.Id.IntegerValue));
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
