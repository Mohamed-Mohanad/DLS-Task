namespace DLS.Application.Features.Categories.Queries.GetCategory;

public sealed record GetCategoryQuery : IQuery<CategoryResponse>
{
    public long Id { get; init; }
} 