using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityRevitPlugin
{
    public static class ElementExtension
    {
        /// <summary>
        /// Возвращает высоту установки элемента относительно уровня в мм,
        /// если уровня нет то координату z в м
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static double GettInstallationHeightRelativeToLevel(this Element element)
        {
            var elementPoint = element.Location as LocationPoint;
            if (elementPoint is null)
                throw new ArgumentException($"У элемента {element.Id} LocationPoint is null");

            var doc = element.Document;
            var levelId = element.LevelId;
            var level = doc.GetElement(levelId) as Level;
            if (level is null)
            {
                var familyInstance = element as FamilyInstance;
                var host = familyInstance?.Host;
                if (host is Level levelHost)
                {
                    level = levelHost;
                }
                else
                {
                    var hostLevelId = host?.LevelId;
                    if (host is null || hostLevelId is null)
                        return UnitUtils.ConvertFromInternalUnits(elementPoint.Point.Z, DisplayUnitType.DUT_MILLIMETERS);
                    level = doc.GetElement(hostLevelId) as Level;
                }
            }
            if(level is null)
            {

            }
            var elevation = level.Elevation;
            var height = elementPoint.Point.Z - elevation;
            var heigthInMillimeters = UnitUtils.ConvertFromInternalUnits(height, DisplayUnitType.DUT_MILLIMETERS);
            return heigthInMillimeters;
        }
    }
}
