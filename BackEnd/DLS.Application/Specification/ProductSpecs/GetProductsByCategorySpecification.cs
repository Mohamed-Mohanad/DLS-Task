namespace DLS.Application.Specifications.ProductSpecs;

internal class GetProductsByCategorySpecification : Specification<Product>
{
    public GetProductsByCategorySpecification(long categoryId, int page = 1, int pageSize = 10)
    {
        AddCriteria(x => x.CategoryId == categoryId);
        AddInclude(nameof(Product.Category));

        ApplyPaging(pageSize, page);
    }
}