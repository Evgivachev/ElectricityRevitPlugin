namespace CountFixturesInSpaceCmd;

using Autodesk.Revit.DB;

public static class ElementExtensions
{
    /// <summary>
    /// Возвращает высоту установки элемента относительно уровня,
    /// по умолчанию во внутренних единицах
    /// </summary>
    /// <param name="element"></param>
    /// <param name="forgeTypeId"></param>
    public static double GetInstallationHeightRelativeToLevel(this Element element, ForgeTypeId? forgeTypeId = null)
    {
        //Смещение по высоте, есть не у всех элементов, но работает
        var shiftParam = element.get_Parameter(BuiltInParameter.INSTANCE_ELEVATION_PARAM);
        var shift = shiftParam.AsDouble();
        if (forgeTypeId is not null && forgeTypeId.IsValidObject)
            shift = UnitUtils.ConvertFromInternalUnits(shift, forgeTypeId);
        return shift;
    }
}
