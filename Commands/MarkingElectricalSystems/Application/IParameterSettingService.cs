namespace MarkingElectricalSystems.Application;

using System.Collections.Generic;
using Domain;

public interface IParameterSettingService
{ 
    void SetParameters(IReadOnlyCollection<int> createdElements, IReadOnlyCollection<ElectricalSystem> elSystems);
}