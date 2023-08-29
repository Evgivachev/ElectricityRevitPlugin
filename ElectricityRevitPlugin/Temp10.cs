namespace ElectricityRevitPlugin
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using CommonUtils.Helpers;

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Temp10 : DefaultExternalCommand
    {
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            using (var tr = new Transaction(Doc, "Rename"))
            {
                tr.Start();
                var fm = Doc.FamilyManager;
                foreach (FamilyType type in fm.Types)
                {
                    var typeName = type.Name;
                    fm.CurrentType = type;
                    var m = fm.get_Parameter(SharedParametersFile.ADSK_Marka);
                    fm.Set(m, typeName);
                }

                tr.Commit();
            }

            return Result.Succeeded;
        }
    }
}
