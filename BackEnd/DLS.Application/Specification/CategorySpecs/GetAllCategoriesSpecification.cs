namespace DLS.Application.Specifications.CategorySpecs;

internal class GetAllCategoriesSpecification : Specification<Category>
{
    public GetAllCategoriesSpecification(int page = 1, int pageSize = 10)
    {
        AddInclude(nameof(Category.Parent));
        AddInclude(nameof(Category.Children));

        ApplyPaging(pageSize, page);
    }
}