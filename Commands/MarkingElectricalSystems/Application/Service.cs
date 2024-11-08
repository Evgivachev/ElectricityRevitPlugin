namespace MarkingElectricalSystems.Application;

using System;
using CommonUtils;

public class Service
{
    private readonly IElementsRepository _repository;
    private readonly ITransactionsService _transactionsService;
    private readonly IParameterSettingService _parameterSettingService;
    public Service(IElementsRepository repository,
        ITransactionsService transactionsService,
        IParameterSettingService parameterSettingService)
    {
        _repository = repository;
        _transactionsService = transactionsService;
        _parameterSettingService = parameterSettingService;
    }
    
    public void DoSomething()
    {
        var elSystems = _repository.GetSelectedElectricalSystems();
        if (elSystems.Count == 0)
        {
            throw new Exception("Отсутствуют выделенные подключенные элементы электрических цепей");
        }

        var symbol = _repository.GetFamilySymbolId("Марка групп цепей");
        var createdElements = _repository.PromptForFamilyInstancePlacement(symbol);

        using var _ = _transactionsService.StartTransaction("Установка параметров");
        _parameterSettingService.SetParameters(createdElements, elSystems);
        _transactionsService.Commit();
    }
}
