namespace DLS.Application.Features.Products.Commands.CreateProduct;

public sealed record CreateProductCommand : ICommand<long>
{
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public decimal Price { get; init; }
    public long CategoryId { get; init; }
}