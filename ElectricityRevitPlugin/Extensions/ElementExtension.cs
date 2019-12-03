using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            //var elementPoint = element.Location as LocationPoint;
            //if (elementPoint is null)
            //    throw new ArgumentException($"У элемента {element.Id} LocationPoint is null");
            //Смещение 
            var shiftParam = element.get_Parameter(BuiltInParameter.INSTANCE_FREE_HOST_OFFSET_PARAM);
            //Смещение по высоте, есть не у всех элементов но работает
            shiftParam = element.get_Parameter(BuiltInParameter.INSTANCE_ELEVATION_PARAM);

            var shift = shiftParam.AsDouble();
            if (displayUnitType.HasValue)
                shift = UnitUtils.ConvertFromInternalUnits(shift, displayUnitType.Value);
            return shift;

                


            //var doc = element.Document;
            //var levelId = element.LevelId;
            //var level = doc.GetElement(levelId) as Level;
            //if (level is null)
            //{
            //    var familyInstance = element as FamilyInstance;
            //    var host = familyInstance?.Host;
            //    if (host is Level levelHost)
            //    {
            //        level = levelHost;
            //    }
            //    else
            //    {
            //        var hostLevelId = host?.LevelId;
            //        if (host is null || hostLevelId is null)
            //            return UnitUtils.ConvertFromInternalUnits(elementPoint.Point.Z, DisplayUnitType.DUT_MILLIMETERS);
            //        level = doc.GetElement(hostLevelId) as Level;
            //    }
            //}
            //if(level is null)
            //{
            //    throw new NullReferenceException($"level is null {element.Id.IntegerValue}");

            //}
            //var elevation = level.Elevation;
            //var height = elementPoint.Point.Z - elevation;
            //var heigthInMillimeters = UnitUtils.ConvertFromInternalUnits(height, DisplayUnitType.DUT_MILLIMETERS);
            //return heigthInMillimeters;
        }
        public static Result SetInstallationHeightRelativeToLevel(this Element element, double heigth, DisplayUnitType? displayUnitType = null, bool openTransaction = false)
        {

            var result = Result.Failed;
            if (openTransaction)
            {
                using (var tr = new Transaction(element.Document))
                {
                    tr.Start("SetElementShift");
                    result = element.SetInstallationHeightRelativeToLevel(heigth,displayUnitType);
                    tr.Commit();
                }
            }
            else
            {
                result = element.SetInstallationHeightRelativeToLevel(heigth, displayUnitType);
            }

            return result;
        }
        private static Result SetInstallationHeightRelativeToLevel(this Element element,double heigth, DisplayUnitType? displayUnitType = null)
        {
            var elementPoint = element.Location as LocationPoint;
            if (elementPoint is null)
                throw new ArgumentException($"У элемента {element.Id} LocationPoint is null");
            var shiftParam = element.get_Parameter(BuiltInParameter.INSTANCE_FREE_HOST_OFFSET_PARAM);
            if (displayUnitType.HasValue)
                heigth = UnitUtils.ConvertToInternalUnits(heigth, displayUnitType.Value);
            var flag =  shiftParam.Set(heigth);
            return flag?Result.Succeeded:Result.Failed;

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
        /// <summary>
        /// Установить поворот элемента в градусах
        /// </summary>
        /// <param name="element"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Result SetElementRotation(this Element element, double rotation, Line line , bool openTransaction = false)
        {
            var result = Result.Failed;
            if (openTransaction)
            {
                using (var tr = new Transaction(element.Document))
                {
                    tr.Start("SetElementRotation");
                    result = element.SetElementRotation(rotation,line);
                    tr.Commit();
                }
            }
            else
            {
                result = element.SetElementRotation(rotation, line);
            }

            return result;
        }
        private static Result SetElementRotation(this Element element, double rotation, Line line)
        {
            if (!(element.Location is LocationPoint currentLocation))
                return Result.Failed;
            var currentRotation = currentLocation.Rotation;
            if (line is null)
            {
                var locatiom = element.Location as LocationPoint;
                line = Line.CreateUnbound(locatiom.Point, new XYZ(0, 0, 1));
            }
                
            var flag = element.Location.Rotate(line, rotation / 180 * Math.PI - currentRotation);
            return flag ? Result.Succeeded : Result.Failed;
        }

        //private static Result CopyParameters(this Element to, Element from, params string[] excludedParams)
        //{
        //    var result = Result.Succeeded;
        //    var fromParametersMap = from.ParametersMap;
        //    var toParameterMap = to.ParametersMap;
        //    foreach (Parameter toParam in toParameterMap)
        //    {
        //        if(toParam.IsReadOnly)
        //            continue;
        //        if(!fromParametersMap.Contains(toParam.Definition.Name))
        //            continue;
        //        var fromParam = fromParametersMap.get_Item(toParam.Definition.Name);
        //        var value = fromParam.GetValueDynamic();
        //        if(value is null)
        //            continue;
        //        var flag = toParam.Set(value);
        //    }
        //    return result;
        //}

        public static Result CopyParameters(this Element to, Element from, bool openTransaction = false, params string[] excludedParams)
        {
            var result = Result.Failed;
            if (openTransaction)
            {
                using (var tr = new Transaction(from.Document))
                {
                    tr.Start("CopyParameters");
                    result = to.CopyParameters(from,false, excludedParams);
                    tr.Commit();
                }
            }
            else
            {
                var fromParametersMap = from.ParametersMap;
                var toParameterMap = to.ParametersMap;
                foreach (Parameter toParam in toParameterMap)
                {
                    if (toParam.IsReadOnly)
                        continue;
                    if (!fromParametersMap.Contains(toParam.Definition.Name))
                        continue;
                    if (excludedParams.Contains(toParam.Definition.Name))
                        continue;
                    var fromParam = fromParametersMap.get_Item(toParam.Definition.Name);
                    var value = fromParam.GetValueDynamic();
                    if (value is null)
                        continue;
                    var flag = toParam.Set(value);
                }
            }
            return result;
        }
    }
}
