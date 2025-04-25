using Domain.Primitives;

namespace DLS.Domain.Entities;

public class CartItem : Entity<long>, IAuditableEntity
{
    public long ProductId { get; private set; }
    public long CartId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalPrice { get; private set; }
    public virtual Product? Product { get; private set; } = null!;
    public virtual Cart? Cart { get; private set; } = null!;
    public DateTime CreatedOnUtc { get; }
    public DateTime? ModifiedOnUtc { get; }

    public CartItem() { }

    public static CartItem Create(long productId, long cartId, int quantity, decimal unitPrice)
    {
        return new CartItem
        {
            ProductId = productId,
            CartId = cartId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            TotalPrice = unitPrice * quantity
        };
    }

    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
        TotalPrice = UnitPrice * quantity;
    }

    public void UpdateUnitPrice(decimal unitPrice)
    {
        UnitPrice = unitPrice;
        TotalPrice = unitPrice * Quantity;
    }
}
