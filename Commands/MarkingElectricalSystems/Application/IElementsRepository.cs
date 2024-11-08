namespace MarkingElectricalSystems.Application;

using System.Collections.Generic;
using Domain;

public interface IElementsRepository
{
    IReadOnlyCollection<ElectricalSystem> GetSelectedElectricalSystems();
    IReadOnlyCollection<Element> GetSelectedElements(IReadOnlyCollection<int> categoryIds);
    
    int GetFamilySymbolId(string name);
    
    IReadOnlyCollection<int> PromptForFamilyInstancePlacement(int familySymbolId);
}
