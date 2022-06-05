namespace Diagrams
{
    using System;
    using System.Linq;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;

    public static class FamilyInstanceExtension
    {
        public static ElectricalSystem GetPowerElectricalSystem(this FamilyInstance familyInstance)
        {
            try
            {
                if (familyInstance?.MEPModel?.GetElectricalSystems() is null
                    || familyInstance.MEPModel.GetElectricalSystems().Count == 0)
                    return null;
                if (familyInstance.MEPModel.GetAssignedElectricalSystems() is null)
                    return familyInstance.MEPModel.GetElectricalSystems().First();
                if (familyInstance.MEPModel.GetElectricalSystems().Count
                    == familyInstance.MEPModel.GetAssignedElectricalSystems().Count)
                    return null;
                return familyInstance.MEPModel
                    .GetElectricalSystems()
                    .First(x => !familyInstance.MEPModel.GetAssignedElectricalSystems().Contains(x));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static bool IsShield(this FamilyInstance familyInstance)
        {
            try
            {
                var flag = familyInstance.Category.Id.IntegerValue == (int)BuiltInCategory.OST_ElectricalEquipment;
                if (!flag)
                    return false;
                var voltage = UnitUtils.ConvertFromInternalUnits(
                    familyInstance.LookupParameter("Напряжение в щите").AsDouble(),
                    UnitTypeId.Volts);
                if (voltage < 100)
                    return false;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
