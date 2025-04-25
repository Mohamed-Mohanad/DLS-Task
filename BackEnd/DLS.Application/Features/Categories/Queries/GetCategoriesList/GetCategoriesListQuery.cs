namespace DLS.Application.Features.Categories.Queries.GetCategoriesList;

public sealed record GetCategoriesListQuery : IPaginateQuery<GetCategoriesListQueryResponse>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}