using DLS.Application.Specifications.ProductSpecs;

namespace DLS.Application.Features.Products.Queries.GetProduct;

internal sealed class GetProductQueryHandler : IQueryHandler<GetProductQuery, ProductResponse>
{
    private readonly IGenericRepository<Product> _productRepo;

    public GetProductQueryHandler(IGenericRepository<Product> productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<Result<ProductResponse>> Handle(
        GetProductQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new GetProductByIdSpecification(request.Id);
        var product = await _productRepo.GetEntityWithSpecAsync(spec, cancellationToken);

        if (product is null)
            return Result.Failure<ProductResponse>(Error.NotFound());

        var response = new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty
        };

        return response;
    }
}