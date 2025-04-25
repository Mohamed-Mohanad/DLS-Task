namespace DLS.Application.Specifications.CartSpecs;

internal class GetUserCartSpecification : Specification<Cart>
{
    public GetUserCartSpecification(long userId, bool isActive = true)
    {
        AddCriteria(x => x.UserId == userId && x.IsActive == isActive);
        AddInclude(nameof(Cart.Items));
        AddInclude($"{nameof(Cart.Items)}.{nameof(CartItem.Product)}");
    }
}