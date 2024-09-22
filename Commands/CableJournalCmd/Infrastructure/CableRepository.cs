namespace CableJournalCmd.Infrastructure;

using Application;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using CommonUtils.Helpers;
using CommonUtils.Services;
using Domain;

public class CableRepository(Document document) : TransactionRepository(document), ICableRepository
{
    public IReadOnlyCollection<Cable> GetAll()
    {
        using var collector = new FilteredElementCollector(document);
        var cables = collector
            .WhereElementIsNotElementType()
            .OfCategory(BuiltInCategory.OST_ElectricalCircuit)
            .OfType<ElectricalSystem>()
            .Select(e => e.ToBll());

        return cables.ToList();
    }

    public void UpdateCables(IEnumerable<Cable> cables)
    {
        foreach (var cable in cables)
        {
            var element = document.GetElement(new ElementId(cable.id));
            var nameInCableScheduleParameter =
                element.get_Parameter(SharedParametersFile.Oboznachenie_Kabelya_V_KZH);
            nameInCableScheduleParameter.Set(cable.InJournalName);

            var nameTubeInCableScheduleParameter =
                element.get_Parameter(SharedParametersFile.Oboznachenie_Dlya_Trub_V_KZH);
            nameTubeInCableScheduleParameter.Set(cable.InTubeJournalName);
        }
    }
}
