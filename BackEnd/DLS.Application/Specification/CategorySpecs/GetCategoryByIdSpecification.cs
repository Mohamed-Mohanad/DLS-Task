namespace DLS.Application.Specifications.CategorySpecs;

internal class GetCategoryByIdSpecification : Specification<Category>
{
    public GetCategoryByIdSpecification(long id)
    {
        AddCriteria(x => x.Id == id);
        AddInclude(nameof(Category.Products));
        AddInclude(nameof(Category.Children));
    }
}