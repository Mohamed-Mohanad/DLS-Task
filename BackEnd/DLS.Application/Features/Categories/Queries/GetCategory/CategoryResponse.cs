namespace DLS.Application.Features.Categories.Queries.GetCategory;

public sealed record CategoryResponse
{
    public long Id { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public long? ParentId { get; init; }
    public string ParentName { get; init; } = null!;
    public List<CategoryChildDto> Children { get; init; } = new();
}

public sealed record CategoryChildDto
{
    public long Id { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
}