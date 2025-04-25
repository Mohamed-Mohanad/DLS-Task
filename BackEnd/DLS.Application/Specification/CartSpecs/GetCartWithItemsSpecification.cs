namespace DLS.Application.Specifications.CartSpecs;

internal class GetCartWithItemsSpecification : Specification<Cart>
{
    public GetCartWithItemsSpecification(long cartId)
    {
        AddCriteria(x => x.Id == cartId);
        AddInclude(nameof(Cart.Items));
        AddInclude($"{nameof(Cart.Items)}.{nameof(CartItem.Product)}");
    }
}