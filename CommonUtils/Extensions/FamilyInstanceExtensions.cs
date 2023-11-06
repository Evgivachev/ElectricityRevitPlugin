namespace CommonUtils.Extensions
{
    using System;
    using System.Linq;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;
    using Comparer;
    using Helpers;

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
                activePowerParameter =
                    familyInstance.get_Parameter(SharedParametersFile.Aktivnaya_Moshchnost_V_SHCHitakh);
                powerFactorParameter = familyInstance.get_Parameter(SharedParametersFile.Kosinus_V_SHCHitakh);
                activePower = activePowerParameter.AsDouble() * 1000;
                powerFactor = powerFactorParameter.AsDouble();
                return true;
            }

            var instanceActivePowerParameter =
                familyInstance.get_Parameter(SharedParametersFile.Ustanovlennaya_Moshchnost);
            var typeActivePowerParameter = type.get_Parameter(SharedParametersFile.Ustanovlennaya_Moshchnost);
            if (instanceActivePowerParameter is { HasValue: true })
                activePowerParameter = instanceActivePowerParameter;
            else
                activePowerParameter = typeActivePowerParameter;
            var instancePowerFactorParameter = familyInstance.get_Parameter(SharedParametersFile.cos_F);
            var typePowerFactorParameter = type.get_Parameter(SharedParametersFile.cos_F);
            if (instancePowerFactorParameter is { HasValue: true })
                powerFactorParameter = instancePowerFactorParameter;
            else
                powerFactorParameter = typePowerFactorParameter;
            var instanceLoadClassificationParameter =
                familyInstance.get_Parameter(SharedParametersFile.Klassifikatsiya_Nagruzki);
            var typeLoadClassificationParameter = type.get_Parameter(SharedParametersFile.Klassifikatsiya_Nagruzki);
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
