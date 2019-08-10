using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;

namespace ElectricityRevitPlugin
{
    public static class ElementExtension
    {
        /// <summary>
        /// Возвращает высоту установки элемента относительно уровня,
        /// по умолчанию во внутренних единицах
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static double GetInstallationHeightRelativeToLevel(this Element element, DisplayUnitType? displayUnitType=null)
        {
            var elementPoint = element.Location as LocationPoint;
            if (elementPoint is null)
                throw new ArgumentException($"У элемента {element.Id} LocationPoint is null");
            var shiftParam = element.get_Parameter(BuiltInParameter.INSTANCE_FREE_HOST_OFFSET_PARAM).AsDouble();
            if (displayUnitType.HasValue)
                shiftParam = UnitUtils.ConvertFromInternalUnits(shiftParam, displayUnitType.Value);
            return shiftParam;

                


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
                throw new NullReferenceException($"level is null {element.Id.IntegerValue}");

            }
            var elevation = level.Elevation;
            var height = elementPoint.Point.Z - elevation;
            var heigthInMillimeters = UnitUtils.ConvertFromInternalUnits(height, DisplayUnitType.DUT_MILLIMETERS);
            return heigthInMillimeters;
        }

        public static Result SetElementCoordinate(this Element element,XYZ point, bool openTransaction = true)
        {
            var result = Result.Failed;
            if (openTransaction)
            {
                using (var tr = new Transaction(element.Document))
                {
                    tr.Start("SetElementCoordinate");
                    result = element.SetElementCoordinate(point);
                    tr.Commit();
                }
            }
            else
            {
                result = element.SetElementCoordinate(point);
            }

            return result;
        }
        private static Result SetElementCoordinate(this Element element, XYZ point)
        {
            if (!(element.Location is LocationPoint currentLocation))
                    return Result.Failed;

            var flag = element.Location.Move(point.Subtract(currentLocation.Point));
            return flag ? Result.Succeeded : Result.Failed;
        }
        
    }
}
