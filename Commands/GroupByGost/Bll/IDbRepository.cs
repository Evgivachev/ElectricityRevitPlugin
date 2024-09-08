namespace GroupByGost.Bll;

using System.Collections.Generic;
using Domain;

public interface IDbRepository
{
    IReadOnlyCollection<Shield> GetShields();

    IReadOnlyCollection<Element> GetElectricalElements();

    void UpdateCircuits(IReadOnlyCollection<ElectricalCircuit> circuits);
    
    void UpdateElements(IReadOnlyCollection<Element> elements);
    
    IReadOnlyCollection<ElectricalCircuit> GetElectricalSystems();
}
