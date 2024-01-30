namespace CommonUtils.Extensions
{
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
                StorageType.String => parameter.AsString(),
                StorageType.ElementId => parameter.AsElementId(),
                _ => parameter.AsValueString()
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
            switch (type)
            {
                case StorageType.Double:
                    return parameter.Set((double) value);
                case StorageType.Integer:
                    return parameter.Set((int) value);

                case StorageType.String:
                {
                    if (value is string q)
                        return parameter.Set(q);
                    return false;
                }

                case StorageType.ElementId:
                    return parameter.Set((ElementId) value);
                case StorageType.None:
                default:
                    return parameter.SetValueString(value.ToString());
            }
        }
    }
}