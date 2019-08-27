using Autodesk.Revit.DB;

namespace ElectricityRevitPlugin
{
    public static class ParameterExtension
    {
        public static dynamic GetValueDynamic(this Parameter parameter)
        {
            var type = parameter.StorageType;
            switch (type)
            {
                case StorageType.Double:
                    return parameter.AsDouble();
                case StorageType.Integer:
                    return parameter.AsInteger();
                case StorageType.String:
                    return parameter.AsString();
                case StorageType.ElementId:
                    return parameter.AsElementId();
                default:
                    return parameter.AsValueString();
                    
            }
        }
        
    }
}