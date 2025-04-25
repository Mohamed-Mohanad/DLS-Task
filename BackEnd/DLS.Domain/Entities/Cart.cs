using Domain.Primitives;

namespace DLS.Domain.Entities;

public class Cart : AggregateRoot<long>, IAuditableEntity
{
    private List<CartItem> _items = new();

    public long UserId { get; private set; }
    public virtual User? User { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedOnUtc { get; }
    public DateTime? ModifiedOnUtc { get; }
    public virtual IReadOnlyList<CartItem> Items => _items.AsReadOnly();
    public Cart() { }

    public static Cart Create(long userId)
    {
        return new Cart
        {
            UserId = userId,
            IsActive = true,
        };
    }

    public void Update(long? userId = null, bool? isActive = null)
    {
        if (userId.HasValue)
            UserId = userId.Value;

        if (isActive.HasValue)
            IsActive = isActive.Value;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
    public void Activate()
    {
        IsActive = true;
    }
}
