namespace Diagrams
{
    using Autodesk.Revit.DB;

    public static class ParameterExtension
    {
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

        public static bool SetEmptyValue(this Parameter parameter)
        {
            var type = parameter.StorageType;
            switch (type)
            {
                case StorageType.Double:
                    return parameter.Set(0.0);
                case StorageType.Integer:
                    return parameter.Set(0);
                case StorageType.String:
                    return parameter.Set("");
                case StorageType.ElementId:
                    return parameter.Set(new ElementId(-1));
                default:
                    return parameter.SetValueString("");
            }
        }

        public static bool SetDynamicValue(this Parameter parameter, dynamic value)
        {
            var type = parameter.StorageType;
            switch (type)
            {
                case StorageType.Double:
                    return parameter.Set((double)value);
                case StorageType.Integer:
                    return parameter.Set((int)value);

                case StorageType.String:
                {
                    if (value is string q)
                        return parameter.Set(q);
                    return false;
                }

                case StorageType.ElementId:
                    return parameter.Set((ElementId)value);
                case StorageType.None:
                default:
                    return parameter.SetValueString(value);
            }
        }
    }
}
