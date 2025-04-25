namespace DLS.Application.Specifications.ProductSpecs;

internal class GetAllProductsSpecification : Specification<Product>
{
    public GetAllProductsSpecification(int page = 1, int pageSize = 10)
    {
        AddInclude(nameof(Product.Category));

        ApplyPaging(pageSize, page);
        AddOrderByDescending(x => x.CreatedOnUtc);
    }
}