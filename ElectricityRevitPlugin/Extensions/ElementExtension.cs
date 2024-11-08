namespace ElectricityRevitPlugin.Extensions;

using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

public static class ElementExtension
{
    /// <summary>
    /// Возвращает высоту установки элемента относительно уровня,
    /// по умолчанию во внутренних единицах
    /// </summary>
    /// <param name="element"></param>
    /// <param name="forgeTypeId"></param>
    /// <returns></returns>
    public static double GetInstallationHeightRelativeToLevel(this Element element, ForgeTypeId forgeTypeId = null)
    {
        //Смещение по высоте, есть не у всех элементов но работает
        var shiftParam = element.get_Parameter(BuiltInParameter.INSTANCE_ELEVATION_PARAM);
        var shift = shiftParam.AsDouble();
        if (forgeTypeId is not null && forgeTypeId.IsValidObject)
            shift = UnitUtils.ConvertFromInternalUnits(shift, forgeTypeId);
        return shift;
    }

    public static Result CopyParameters(this Element to, Element from, bool openTransaction = false, params string[] excludedParams)
    {
        var result = Result.Failed;
        if (openTransaction)
            using (var tr = new Transaction(from.Document))
            {
                tr.Start("CopyParameters");
                result = to.CopyParameters(from, false, excludedParams);
                tr.Commit();
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
                if (!fromParam.HasValue)
                    continue;
                var value = fromParam.GetValueDynamic();
                if (value is null)
                    continue;
                // TODO set parameters
            }
        }

        return result;
    }
}
