namespace CommonUpdateCmd.Infrastructure.UpdateElectricalSystem;

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Electrical;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
class UpdateCableManagementMethodExternalCommand : IUpdaterParameters<ElectricalSystem>
{
    public string UpdateParameters(ElectricalSystem els)
    {
        //Способ прокладки кабелей для ОС
        var markParam = els.get_Parameter(new Guid("914fd7c8-80ed-4e93-9461-13e8c8fec57d"));
        var fromParam = els.LookupParameter("Способ прокладки для схем").AsString();
        markParam.Set(fromParam);
        return fromParam;
    }
}
