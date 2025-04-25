using DLS.Application.Specifications.CategorySpecs;
using DLS.Application.Specifications.ProductSpecs;

namespace DLS.Application.Features.Categories.Commands.DeleteCategory;

internal sealed class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, bool>
{
    private readonly IGenericRepository<Category> _categoryRepo;
    private readonly IGenericRepository<Product> _productRepo;

    public DeleteCategoryCommandHandler(
        IGenericRepository<Category> categoryRepo,
        IGenericRepository<Product> productRepo)
    {
        _categoryRepo = categoryRepo;
        _productRepo = productRepo;
    }

    public async Task<Result<bool>> Handle(
        DeleteCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var spec = new GetCategoryByIdSpecification(request.Id);
        var category = await _categoryRepo.GetEntityWithSpecAsync(spec, cancellationToken);

        if (category is null)
            return Result.Failure<bool>(Error.NotFound());

        var productSpec = new GetProductsByCategorySpecification(request.Id);
        var (productQuery, count) = await _productRepo.GetWithSpecAsync(productSpec, cancellationToken);

        if (count > 0)
            return Result.Failure<bool>(Error.NotFound());

        if (category.Children.Count > 0)
            return Result.Failure<bool>(Error.NotFound());

        _categoryRepo.Delete(category);
        var result = await _categoryRepo.SaveChangesAsync(cancellationToken);

        return result;
    }
}