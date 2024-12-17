namespace MarkingElectricalSystems.Application;

using System;
using CommonUtils;

public class MarkElectricalSystemsService(
    IElementsRepository repository,
    ITransactionsService transactionsService,
    IParameterSettingService parameterSettingService) : IMarkElectricalSystemsService
{
    public void DoSomething()
    {
        var elSystems = repository.GetSelectedElectricalSystems();
        if (elSystems.Count == 0)
        {
            throw new Exception("Отсутствуют выделенные подключенные элементы электрических цепей");
        }

        var symbol = repository.GetFamilySymbolId("Марка групп цепей");
        var createdElements = repository.PromptForFamilyInstancePlacement(symbol);

        using var _ = transactionsService.StartTransaction("Установка параметров");
        parameterSettingService.SetParameters(createdElements, elSystems);
        transactionsService.Commit();
    }
}
