namespace ElectricityRevitPlugin
{
    using System.Collections.Generic;
    using Autodesk.Revit.DB;

    class RevitNameComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x is null || y is null)
                return 0;
            return NamingUtils.CompareNames(x, y);
        }
    }
}
