namespace DLS.Application.Features.Products.Queries.GetProduct;

public sealed record ProductResponse
{
    public long Id { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public decimal Price { get; init; }
    public long CategoryId { get; init; }
    public string CategoryName { get; init; } = null!;
}