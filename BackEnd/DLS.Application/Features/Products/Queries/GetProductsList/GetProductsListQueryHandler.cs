using DLS.Application.Specifications.ProductSpecs;

namespace DLS.Application.Features.Products.Queries.GetProductsList;

internal sealed class GetProductsListQueryHandler : IPaginateQueryHandler<GetProductsListQuery, GetProductsListQueryResponse>
{
    private readonly IGenericRepository<Product> _productRepo;

    public GetProductsListQueryHandler(IGenericRepository<Product> productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<Pagination<GetProductsListQueryResponse>> Handle(
        GetProductsListQuery request,
        CancellationToken cancellationToken)
    {
        Specification<Product> spec;

        if (request.CategoryId.HasValue)
        {
            spec = new GetProductsByCategorySpecification(
                request.CategoryId.Value,
                request.Page,
                request.PageSize);
        }
        else
        {
            spec = new GetAllProductsSpecification(
                request.Page,
                request.PageSize);
        }

        var (queryResult, totalCount) = await _productRepo.GetWithSpecAsync(spec, cancellationToken);

        var products = queryResult.Select(p => new GetProductsListQueryResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            CategoryId = p.CategoryId,
            CategoryName = p.Category!.Name ?? string.Empty
        }).ToList();

        var result = new Pagination<GetProductsListQueryResponse>(request.Page, request.PageSize, totalCount, products);

        return result;
    }
}