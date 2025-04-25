using Domain.Primitives;

namespace DLS.Domain.Entities;

public class Category : AggregateRoot<long>, IAuditableEntity
{
    private readonly List<Product> _products = new();
    private readonly List<Category> _children = new();

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public long? ParentId { get; set; }
    public virtual Category? Parent { get; set; } = null!;
    public virtual IReadOnlyList<Category> Children => _children.AsReadOnly();
    public virtual IReadOnlyList<Product> Products => _products.AsReadOnly();
    public DateTime CreatedOnUtc { get; }
    public DateTime? ModifiedOnUtc { get; }

    public Category() { }

    public static Category Create(string name, string description, long? parentId = null)
    {
        return new Category
        {
            Name = name,
            Description = description,
            ParentId = parentId,
        };
    }

    public void Update(
        string? name = null,
        string? description = null,
        long? parentId = null)
    {
        if (!string.IsNullOrEmpty(name))
            Name = name;

        if (!string.IsNullOrEmpty(description))
            Description = description;

        if (parentId.HasValue)
            ParentId = parentId.Value;
    }
}
