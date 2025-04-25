namespace DLS.Application.Features.Products.Queries.GetProductsList;

public sealed record GetProductsListQuery : IPaginateQuery<GetProductsListQueryResponse>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public long? CategoryId { get; init; }
}