namespace DLS.Application.Features.Cart.Commands.RemoveFromCart;

public sealed record RemoveFromCartCommand : ICommand
{
    public long UserId { get; init; }
    public long ProductId { get; init; }
}