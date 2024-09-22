namespace CableJournalCmd.Domain;

using System.Collections;

public class CableJournal : IEnumerable<Cable>
{
    private readonly List<Cable> _cables = [];

    public void Add(Cable cable)
    {
        _cables.Add(cable);
    }
    
    public void SortCables()
    {
        var groups = _cables
            .GroupBy(x => x.Chapter);
        foreach (var group in groups)
        {
            var number = 1;
            foreach (var electricalSystem in group
                         .OrderBy(x => x.PanelName)
                         .ThenBy(x => x.qf))
            {
                // var nameInCableScheduleParameter =
                //     electricalSystem.get_Parameter(SharedParametersFile.Oboznachenie_Kabelya_V_KZH);
                // var nameTubeInCableScheduleParameter =
                //     electricalSystem.get_Parameter(SharedParametersFile.Oboznachenie_Dlya_Trub_V_KZH);
                var groupByGost = electricalSystem.groupGost;
                var isNotInSchedule = !electricalSystem.ShouldBeAddedToJournal();
                if (isNotInSchedule)
                {
                    electricalSystem.ClearName();
                    continue;
                }

                electricalSystem.SetNames(groupByGost, number++);
            }
        }
    }
    public IEnumerator<Cable> GetEnumerator()
    {
        return _cables.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
