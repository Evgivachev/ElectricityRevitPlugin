namespace CommonUtils.Extensions
{
    using System;
    using System.Linq;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;
    using Comparer;

    public static class FamilyInstanceExtension
    {
        //Установленная мощность
        private static Guid _installedPower = new Guid("9ebba55d-0d75-4556-8fcf-93b5362c3e27");

        //Активная мощность в сетях
        private static readonly Guid _installedPowerShield = new Guid("1a63996b-777a-471f-aa56-b91d1c1f7232");

        private static readonly Guid _powerFactor = new Guid("2ca28edf-3aaf-486a-830a-fae82079832d");

        //Косинус в щитах
        private static readonly Guid _powerFactorShield = new Guid("ddc9d5f2-202b-4f16-a5d8-f1180c8b7984");
        private static Guid _loadClassificationGuid = new Guid("fa6ed58d-6a37-4060-9cd2-a16c5d110afe");

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
                var assignedElectricalSystemsIds =
                    familyInstance.MEPModel!.GetAssignedElectricalSystems()
                        .Select(es => es.Id.IntegerValue)
                        .ToHashSet();
                if (assignedElectricalSystemsIds.Count == 0)
                    return allSystems.First();
                if (allSystems.Count == assignedElectricalSystemsIds.Count)
                    return null;
                return allSystems
                    .First(x => !assignedElectricalSystemsIds.Contains(x.Id.IntegerValue));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool TryGetElectricalParameters(
            this FamilyInstance familyInstance,
            out double activePower,
            out double powerFactor,
            out ElementId loadClassification)
        {
            var doc = familyInstance.Document;
            var typeId = familyInstance.GetTypeId();
            var type = doc.GetElement(typeId);
            Parameter? activePowerParameter;
            Parameter? powerFactorParameter;
            Parameter loadClassificationParameter;
            loadClassification = ElementId.InvalidElementId;
            //Коэффициент для перевода кВт в Вт
            if (familyInstance.Category.Id.IntegerValue == (int)BuiltInCategory.OST_ElectricalEquipment)
            {
                activePowerParameter = familyInstance.get_Parameter(_installedPowerShield);
                powerFactorParameter = familyInstance.get_Parameter(_powerFactorShield);
                activePower = activePowerParameter.AsDouble() * 1000;
                powerFactor = powerFactorParameter.AsDouble();
                return true;
            }

            var instanceActivePowerParameter = familyInstance.get_Parameter(_installedPower);
            var typeActivePowerParameter = type.get_Parameter(_installedPower);
            if (instanceActivePowerParameter is { HasValue: true })
                activePowerParameter = instanceActivePowerParameter;
            else
                activePowerParameter = typeActivePowerParameter;
            var instancePowerFactorParameter = familyInstance.get_Parameter(_powerFactor);
            var typePowerFactorParameter = type.get_Parameter(_powerFactor);
            if (instancePowerFactorParameter is { HasValue: true })
                powerFactorParameter = instancePowerFactorParameter;
            else
                powerFactorParameter = typePowerFactorParameter;
            var instanceLoadClassificationParameter = familyInstance.get_Parameter(_loadClassificationGuid);
            var typeLoadClassificationParameter = type.get_Parameter(_loadClassificationGuid);
            if (instanceLoadClassificationParameter is { HasValue: true })
                loadClassificationParameter = instanceLoadClassificationParameter;
            else
                loadClassificationParameter = typeLoadClassificationParameter;
            if (activePowerParameter is null || powerFactorParameter is null)
            {
                activePower = 0;
                powerFactor = 0;
                return false;
            }

            activePower =
                UnitUtils.ConvertFromInternalUnits(activePowerParameter.AsDouble(), UnitTypeId.Watts);
            powerFactor = powerFactorParameter.AsDouble();
            loadClassification = loadClassificationParameter.AsElementId();
            return true;
        }
    }
}
