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
        //Установленная мощность
        private static Guid  _installedPower = new Guid("9ebba55d-0d75-4556-8fcf-93b5362c3e27");
        private static Guid _powerFactor = new Guid("2ca28edf-3aaf-486a-830a-fae82079832d");
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

        public static void GetElectricalParameters(this FamilyInstance familyInstance, out double activePower,
            out double powerFactor)
        {
            var doc = familyInstance.Document;
            var typeId = familyInstance.GetTypeId();
            var type = doc.GetElement(typeId);

            var activePowerParameter =
                familyInstance.get_Parameter(_installedPower) ?? type.get_Parameter(_installedPower);
            activePower =
                UnitUtils.ConvertFromInternalUnits(activePowerParameter.AsDouble(), DisplayUnitType.DUT_WATTS);
            var powerFactorParameter = familyInstance.get_Parameter(_powerFactor) ?? type.get_Parameter(_powerFactor);
            powerFactor = powerFactorParameter.AsDouble();

        }

    }
}
