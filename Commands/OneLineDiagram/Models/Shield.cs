namespace Diagrams.Models
{
    using Autodesk.Revit.DB;
    using CommonUtils;

    public class Shield : ElementWrapperBase<FamilyInstance>
    {
        public Shield(FamilyInstance initialInstance)
            : base(initialInstance)
        {
            Name = initialInstance.Name;
        }

        public string Name { get; }
    }
}
