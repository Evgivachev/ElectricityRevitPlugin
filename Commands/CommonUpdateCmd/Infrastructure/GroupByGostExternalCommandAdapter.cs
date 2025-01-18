namespace CommonUpdateCmd.Infrastructure;

using Application;
using GroupByGost.Application;

public class GroupByGostExternalCommandAdapter(IGroupByGostService groupByGostService) : IExternalCommand
{
    public void Execute()
    {
        groupByGostService.Execute();
    }
}
