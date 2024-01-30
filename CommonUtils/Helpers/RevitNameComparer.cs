namespace CommonUtils.Helpers
{
    using System.Collections.Generic;
    using Autodesk.Revit.DB;

    /// <summary>
    /// <see cref="IComparer{T}"/>
    /// </summary>
    public class RevitNameComparer : IComparer<string>
    {
        /// <inheritdoc />
        public int Compare(string? x, string? y)
        {
            if (x is null || y is null)
                return 0;
            return NamingUtils.CompareNames(x, y);
        }
    }
}