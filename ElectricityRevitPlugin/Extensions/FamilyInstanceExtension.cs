using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Exceptions;
using ArgumentException = System.ArgumentException;

namespace ElectricityRevitPlugin.Extensions
{
    public static class FamilyInstanceExtension
    {
        //Установленная мощность
        private static Guid _installedPower = new Guid("9ebba55d-0d75-4556-8fcf-93b5362c3e27");
        //Активная мощность в сетях
        private static Guid _installedPowerShield = new Guid("1a63996b-777a-471f-aa56-b91d1c1f7232");
        private static Guid _powerFactor = new Guid("2ca28edf-3aaf-486a-830a-fae82079832d");
        //Косинус в щитах
        private static Guid _powerFactorShield = new Guid("ddc9d5f2-202b-4f16-a5d8-f1180c8b7984");
        private static Guid _loadClassificationGuid = new Guid("fa6ed58d-6a37-4060-9cd2-a16c5d110afe");
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
            catch (Exception)
            {
                return null;
            }
        }

        public static void GetElectricalParameters(this FamilyInstance familyInstance, out double activePower,
            out double powerFactor,
            out ElementId loadClassification)
        {
            var doc = familyInstance.Document;
            var typeId = familyInstance.GetTypeId();
            var type = doc.GetElement(typeId);
            Parameter activePowerParameter = null;
            Parameter powerFactorParameter = null;
            Parameter loadClassificationParameter;
            loadClassification = ElementId.InvalidElementId;

            if (familyInstance.Category.Id.IntegerValue == (int)BuiltInCategory.OST_ElectricalEquipment)
            {
                activePowerParameter = familyInstance.get_Parameter(_installedPowerShield);
                powerFactorParameter = familyInstance.get_Parameter(_powerFactorShield);
                activePower = activePowerParameter.AsDouble();
                powerFactor = powerFactorParameter.AsDouble();
                return;
            }

            var instanceActivePowerParameter = familyInstance.get_Parameter(_installedPower);
            var typeActivePowerParameter = type.get_Parameter(_installedPower);
            if (instanceActivePowerParameter != null && instanceActivePowerParameter.HasValue)
                activePowerParameter = instanceActivePowerParameter;
            else
                activePowerParameter = typeActivePowerParameter;

            var instancePowerFactorParameter = familyInstance.get_Parameter(_powerFactor);
            var typePowerFactorParameter = type.get_Parameter(_powerFactor);
            if (instancePowerFactorParameter != null && instancePowerFactorParameter.HasValue)
                powerFactorParameter = instancePowerFactorParameter;
            else
                powerFactorParameter = typePowerFactorParameter;

            var instanceLoadClassificationParameter = familyInstance.get_Parameter(_loadClassificationGuid);
            var typeLoadClassificationParameter = type.get_Parameter(_loadClassificationGuid);
            if (instanceLoadClassificationParameter != null && instanceLoadClassificationParameter.HasValue)
                loadClassificationParameter = instanceLoadClassificationParameter;
            else
                loadClassificationParameter = typeLoadClassificationParameter;

            if (activePowerParameter is null || powerFactorParameter is null)
            {

                activePower = 0;
                powerFactor = 0;
                return;
                throw new ArgumentException($"Ошибка в электрическом соединителе семейства id=\"{familyInstance.Id}\"");
            }
            activePower =
                UnitUtils.ConvertFromInternalUnits(activePowerParameter.AsDouble(), DisplayUnitType.DUT_WATTS);
            powerFactor = powerFactorParameter.AsDouble();
            loadClassification = loadClassificationParameter.AsElementId();
        }

    }
}
