using DLS.Application.Specifications.CartItemSpecs;
using DLS.Application.Specifications.CartSpecs;

namespace DLS.Application.Features.Cart.Commands.RemoveFromCart;

internal sealed class RemoveFromCartCommandHandler : ICommandHandler<RemoveFromCartCommand>
{
    private readonly IGenericRepository<DLS.Domain.Entities.Cart> _cartRepo;
    private readonly IGenericRepository<CartItem> _cartItemRepo;

    public RemoveFromCartCommandHandler(
        IGenericRepository<DLS.Domain.Entities.Cart> cartRepo,
        IGenericRepository<CartItem> cartItemRepo)
    {
        _cartRepo = cartRepo;
        _cartItemRepo = cartItemRepo;
    }

    public async Task<Result> Handle(
        RemoveFromCartCommand request,
        CancellationToken cancellationToken)
    {
        var cartSpec = new GetUserCartSpecification(request.UserId, true);
        var cart = await _cartRepo.GetEntityWithSpecAsync(cartSpec, cancellationToken);

        if (cart is null)
            return Result.Failure(Error.NotFound());

        var itemSpec = new GetCartItemSpecification(cart.Id, request.ProductId);
        var cartItem = await _cartItemRepo.GetEntityWithSpecAsync(itemSpec, cancellationToken);

        if (cartItem is null)
            return Result.Failure(Error.NotFound());

        _cartItemRepo.Delete(cartItem);
        await _cartItemRepo.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}