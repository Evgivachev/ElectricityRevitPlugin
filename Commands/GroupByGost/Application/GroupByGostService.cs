﻿namespace GroupByGost.Application;

using System.Collections.Generic;
using System.Linq;
using CommonUtils;
using Domain;
using MoreLinq;

public class GroupByGostService : IGroupByGostService
{
    private readonly ITransactionsService _transactionsService;
    private readonly IDbRepository _dbRepository;

    public GroupByGostService(ITransactionsService transactionsService, IDbRepository dbRepository)
    {
        _transactionsService = transactionsService;
        _dbRepository = dbRepository;
    }
    public void Execute()
    {
        var shields = _dbRepository.GetShields();
        var allElements = _dbRepository.GetElectricalElements();
        var shieldsCircuits = shields
            .SelectMany(s => s.GetCircuits())
            .ToArray();
        var electricalSystems = shieldsCircuits
            .Concat(_dbRepository
                .GetElectricalSystems())
            .DistinctBy(c => c.Id)
            .ToDictionary(s => s.Id);
        foreach (var shield in shields)
        {
            shield.SortCircuits();
        }

        ProcessElements(allElements, electricalSystems);

        using var tr = _transactionsService.StartTransaction("Группы по ГОСТ");
        _dbRepository.UpdateCircuits(shieldsCircuits);
        _dbRepository.UpdateElements(allElements);
        _transactionsService.Commit();
    }

    private void ProcessElements(IReadOnlyCollection<Element> allElements, Dictionary<int, ElectricalCircuit> electricalSystems)
    {
        foreach (var element in allElements)
        {
            if (element.PowerCableId is not null && electricalSystems.TryGetValue(element.PowerCableId.Value, out var system))
            {
                element.GroupByGost = system.GroupByGost;
            }
            else if (element.ParentElementId is not null && electricalSystems.TryGetValue(element.ParentElementId.Value, out system))
            {
                element.GroupByGost = system.GroupByGost;
            }
        }
    }
}
