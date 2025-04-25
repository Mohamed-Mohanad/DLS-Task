namespace DLS.Application.Features.Products.Queries.GetProduct;

public sealed record GetProductQuery : IQuery<ProductResponse>
{
    public long Id { get; init; }
}