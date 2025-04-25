namespace DLS.Application.Specifications.ProductSpecs;

internal class GetProductByIdSpecification : Specification<Product>
{
    public GetProductByIdSpecification(long id)
    {
        AddCriteria(x => x.Id == id);
        AddInclude(nameof(Product.Category));
    }
}