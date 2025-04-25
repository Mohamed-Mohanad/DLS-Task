using DLS.Application.Specifications.CartItemSpecs;
using DLS.Application.Specifications.CartSpecs;
using DLS.Application.Specifications.ProductSpecs;

namespace DLS.Application.Features.Cart.Commands.AddToCart;

internal sealed class AddToCartCommandHandler : ICommandHandler<AddToCartCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddToCartCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        AddToCartCommand request,
        CancellationToken cancellationToken)
    {
        var productSpec = new GetProductByIdSpecification(request.ProductId);
        var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(productSpec, cancellationToken);

        var cartSpec = new GetUserCartSpecification(request.UserId, true);
        var cart = await _unitOfWork.Repository<DLS.Domain.Entities.Cart>().GetEntityWithSpecAsync(cartSpec, cancellationToken);

        if (cart is null)
        {
            cart = DLS.Domain.Entities.Cart.Create(request.UserId);
            await _unitOfWork.Repository<DLS.Domain.Entities.Cart>().AddAsync(cart, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        var existingItem = await _unitOfWork.Repository<CartItem>().GetEntityWithSpecAsync(
            new GetCartItemSpecification(cart.Id, request.ProductId),
            cancellationToken);

        if (existingItem is not null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + request.Quantity);
        }
        else
        {
            var cartItem = CartItem.Create(
                request.ProductId,
                cart.Id,
                request.Quantity,
                product!.Price);

            await _unitOfWork.Repository<CartItem>().AddAsync(cartItem, cancellationToken);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }
}