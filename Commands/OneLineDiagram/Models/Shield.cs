namespace Diagrams.Models
{
    public record Shield
    {

        public int Id { get; init; }

        public string Name { get; init; }

        public required string UniqueId { get; init; }
    }
}
