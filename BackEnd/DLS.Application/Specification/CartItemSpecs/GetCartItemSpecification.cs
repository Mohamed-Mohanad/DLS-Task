using DLS.Domain.Entities;

namespace DLS.Application.Specifications.CartItemSpecs;

internal class GetCartItemSpecification : Specification<CartItem>
{
    public GetCartItemSpecification(long cartId, long productId)
    {
        AddCriteria(i => i.CartId == cartId && i.ProductId == productId);
    }
}