using DLS.Application.Specifications.CategorySpecs;

namespace DLS.Application.Features.Categories.Commands.UpdateCategory;

internal sealed class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly IGenericRepository<Category> _categoryRepo;

    public UpdateCategoryCommandHandler(IGenericRepository<Category> categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<Result> Handle(
        UpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var spec = new GetCategoryByIdSpecification(request.Id);
        var category = await _categoryRepo.GetEntityWithSpecAsync(spec, cancellationToken);

        if (category is null)
            return Result.Failure<bool>(Error.NotFound());

        category.Update(
            request.Name,
            request.Description,
            request.ParentId);

        await _categoryRepo.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}