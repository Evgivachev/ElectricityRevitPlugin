using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityRevitPlugin.Extensions
{
    public static class FamilyInstanceExtension
    {
        public static ElectricalSystem GetPowerElectricalSystem(this FamilyInstance familyInstance)
        {
            try
            {
                if (familyInstance?.MEPModel?.ElectricalSystems is null
                    || familyInstance.MEPModel.ElectricalSystems.Size == 0)
                    return null;
                if (familyInstance.MEPModel.AssignedElectricalSystems is null)
                    return familyInstance.MEPModel.ElectricalSystems.OfType<ElectricalSystem>().First();
                if (familyInstance.MEPModel.ElectricalSystems.Size
                    == familyInstance.MEPModel.AssignedElectricalSystems.Size)
                    return null;

                return familyInstance.MEPModel.ElectricalSystems.OfType<ElectricalSystem>()
                    .First(x => !familyInstance.MEPModel.AssignedElectricalSystems.Contains(x));
            }
            catch (Exception )
            {
                return null;
            }

        }
    }
}
