namespace DLS.Application.Features.Cart.Queries.GetCart;

public sealed record CartResponse
{
    public long Id { get; init; }
    public long UserId { get; init; }
    public decimal TotalPrice { get; init; }
    public int ItemCount { get; init; }
    public List<CartItemResponse> Items { get; init; } = new();
}

public sealed record CartItemResponse
{
    public long Id { get; init; }
    public long ProductId { get; init; }
    public string ProductName { get; init; } = null!;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal TotalPrice { get; init; }
}