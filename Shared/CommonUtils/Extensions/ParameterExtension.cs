namespace CommonUtils.Extensions;

using System;
using Autodesk.Revit.DB;

/// <summary>
/// Методы расширения для <see cref="Parameter"/>
/// </summary>
public static class ParameterExtension
{
    /// <summary>
    /// Возвращает значение параметра в dynamic.
    /// </summary>
    /// <param name="parameter"></param>
    public static dynamic GetValueDynamic(this Parameter parameter)
    {
        var type = parameter.StorageType;
        return type switch
        {
            StorageType.Double => parameter.AsDouble(),
            StorageType.Integer => parameter.AsInteger(),
            StorageType.String => parameter.AsString() ?? string.Empty,
            StorageType.ElementId => parameter.AsElementId() ?? ElementId.InvalidElementId,
            _ => parameter.AsValueString() ?? string.Empty
        };
    }

    /// <summary>
    /// Сбрасывает значение параметра
    /// </summary>
    /// <param name="parameter">Параметр</param>
    public static bool ResetValue(this Parameter parameter)
    {
        return parameter.StorageType switch
        {
            StorageType.None => parameter.SetValueString(string.Empty),
            StorageType.Integer => parameter.Set(default(int)),
            StorageType.Double => parameter.Set(default(double)),
            StorageType.String => parameter.Set(string.Empty),
            StorageType.ElementId => parameter.Set(ElementId.InvalidElementId),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <summary>
    /// Устанавливает значение параметра.
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="value"></param>
    public static bool SetDynamicValue(this Parameter parameter, dynamic value)
    {
        var type = parameter.StorageType;
        dynamic? converted;
        switch (type)
        {
            case StorageType.Double:
                converted = ConvertM((object)value, Convert.ToDouble, 0);
                return parameter.Set((double)converted);
            case StorageType.Integer:
                converted = ConvertM((object)value, Convert.ToInt32, 0);
                return parameter.Set((int)converted);

            case StorageType.String:
            {
                if (value is string q)
                    return parameter.Set(q);
                return parameter.Set(value.ToString());
            }

            case StorageType.ElementId:
            {
                if (value is ElementId elementId)
                    return parameter.Set(elementId);
                converted = ConvertM((object)value, Convert.ToInt32, 0);
                return parameter.Set(new ElementId((int)converted));
            }

            case StorageType.None:
            default:
                return parameter.SetValueString(value.ToString());
        }
    }

    private static T ConvertM<T>(object value, Func<IConvertible, T> action, T defaultValue)
    {
        return value switch
        {
            T t => t,
            IConvertible convertible => action(convertible),
            _ => defaultValue
        };
    }
}
