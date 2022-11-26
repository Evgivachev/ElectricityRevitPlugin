namespace ElectricityRevitPlugin.Extensions
{
    using System;
    using Autodesk.Revit.DB;

    public static class ParameterExtension
    {
        public static dynamic GetValueDynamic(this Parameter parameter)
        {
            if (parameter is null)
                throw new NullReferenceException();
            var type = parameter.StorageType;
            switch (type)
            {
                case StorageType.Double:
                    return parameter.AsDouble();
                case StorageType.Integer:
                    return parameter.AsInteger();

                case StorageType.String:
                {
                    var result = parameter.AsString();
                    return result ?? string.Empty;
                }

                case StorageType.ElementId:
                    return parameter.AsElementId();
                default:
                    return parameter.AsValueString();
            }
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
    }
}
