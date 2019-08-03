using Autodesk.Revit.DB;

namespace ElectricityRevitPlugin.Extensions
{
    public static class ViewSheetExtension
    {
        public static string GetSheetNumberManuallyString(this ViewSheet viewSheet)
        {
            return viewSheet.LookupParameter("Номер листа (вручную)").AsInteger().ToString();
        }
        
    }
}