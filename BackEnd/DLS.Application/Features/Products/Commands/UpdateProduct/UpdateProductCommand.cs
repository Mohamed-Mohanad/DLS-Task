namespace DLS.Application.Features.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand : ICommand
{
    public long Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public decimal? Price { get; init; }
    public long? CategoryId { get; init; }
}