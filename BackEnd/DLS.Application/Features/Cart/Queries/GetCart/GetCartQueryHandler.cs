using DLS.Application.Specifications.CartSpecs;
using Microsoft.Extensions.Logging;

namespace DLS.Application.Features.Cart.Queries.GetCart;

internal sealed class GetCartQueryHandler : IQueryHandler<GetCartQuery, CartResponse>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IGenericRepository<DLS.Domain.Entities.Cart> _cartRepo;
    private readonly ILogger<GetCartQueryHandler> _logger;

    public GetCartQueryHandler(
        IMapper mapper,
        ICurrentUserService currentUserService,
        IGenericRepository<DLS.Domain.Entities.Cart> _cartRepo,
        ILogger<GetCartQueryHandler> logger)
    {
        _mapper = mapper;
        _currentUserService = currentUserService;
        this._cartRepo = _cartRepo;
        _logger = logger;
    }

    public async Task<Result<CartResponse>> Handle(
        GetCartQuery request,
        CancellationToken cancellationToken)
    {
        var cartSpec = new GetUserCartSpecification((long)_currentUserService.UserId!, true);
        var cart = await _cartRepo.GetEntityWithSpecAsync(cartSpec, cancellationToken);

        if (cart is null)
            return Result.Failure<CartResponse>(Error.NotFound());

        // Debug logging to check if items collection is properly loaded
        _logger.LogInformation("Cart items count: {ItemsCount}", cart.Items.Count);
        foreach (var item in cart.Items)
        {
            _logger.LogInformation("CartItem {ItemId} for product {ProductId} with quantity {Quantity}",
                item.Id, item.ProductId, item.Quantity);
        }

        var response = _mapper.Map<CartResponse>(cart);

        return response;
    }
}