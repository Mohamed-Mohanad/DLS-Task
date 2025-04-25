namespace DLS.Application.Features.Categories.Queries.GetCategoriesList;

public sealed record GetCategoriesListQueryResponse
{
    public long Id { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public long? ParentId { get; init; }
    public string ParentName { get; init; } = null!;
    public int ChildrenCount { get; init; }
}