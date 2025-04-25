using Domain.Primitives;

namespace DLS.Domain.Entities;

public class Product : AggregateRoot<long>, IAuditableEntity
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public decimal Price { get; private set; }
    public long CategoryId { get; private set; }
    public virtual Category? Category { get; private set; } = null!;
    public DateTime CreatedOnUtc { get; }
    public DateTime? ModifiedOnUtc { get; }

    public Product() { }

    public static Product Create(string name, string description, decimal price, long categoryId)
    {
        return new Product
        {
            Name = name,
            Description = description,
            Price = price,
            CategoryId = categoryId,
        };
    }

    public void Update(
        string? name = null,
        string? description = null,
        decimal? price = null,
        long? categoryId = null)
    {
        if (!string.IsNullOrEmpty(name))
            Name = name;

        if (!string.IsNullOrEmpty(description))
            Description = description;

        if (price.HasValue)
            Price = price.Value;

        if (categoryId.HasValue)
            CategoryId = categoryId.Value;
    }
}
