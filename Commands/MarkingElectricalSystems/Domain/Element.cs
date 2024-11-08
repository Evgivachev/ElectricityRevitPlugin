namespace MarkingElectricalSystems.Domain;

public record Element
{
    public long Id { get; init; }
    
    public int CategoryId { get; init; }
}
