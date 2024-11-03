namespace CableJournalCmd.Infrastructure;

using Autodesk.Revit.DB.Electrical;
using CommonUtils.Helpers;
using Domain;

public static class Extensions
{
    public static Cable ToBll(this ElectricalSystem system)
    {
        var panelName = system.PanelName;
        var chapter = system.get_Parameter(SharedParametersFile.Razdel_Proektirovaniya).AsString();
        var qf = system.LookupParameter("Номер QF").AsString();
        var groupGost = system.get_Parameter(SharedParametersFile.Nomer_Gruppy_Po_GOST).AsString();
        var isReserved = system.get_Parameter(SharedParametersFile.Rezervnaya_Gruppa).AsInteger() > 0;
        var isControl = system.get_Parameter(SharedParametersFile.Kontrolnye_TSepi).AsInteger() > 0;
        return new Cable(system.Id.IntegerValue, chapter, panelName, qf, groupGost, isReserved, isControl);
    }
}
