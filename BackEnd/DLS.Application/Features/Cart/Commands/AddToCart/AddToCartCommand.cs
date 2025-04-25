namespace DLS.Application.Features.Cart.Commands.AddToCart;

public sealed record AddToCartCommand : ICommand
{
    public long UserId { get; init; }
    public long ProductId { get; init; }
    public int Quantity { get; init; } = 1;
}