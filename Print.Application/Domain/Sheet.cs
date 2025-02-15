namespace Print.Application.Domain;

public record Sheet
{
    public int Id { get; init; }
    
    public string Name { get; init; }

    public int? ParentId { get; init; }
}
