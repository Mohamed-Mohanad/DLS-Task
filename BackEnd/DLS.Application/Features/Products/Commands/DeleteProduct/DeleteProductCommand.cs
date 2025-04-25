namespace DLS.Application.Features.Products.Commands.DeleteProduct;

public sealed record DeleteProductCommand : ICommand<bool>
{
    public long Id { get; init; }
} 