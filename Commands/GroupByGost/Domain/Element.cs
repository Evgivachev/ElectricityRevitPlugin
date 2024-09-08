namespace GroupByGost.Domain;

public class Element
{
    public int Id { get; init; }

    public int? PowerCableId { get; init; }

    public int? ParentElementId { get; init; }

    public string GroupByGost { get; set; } = "???";
}
