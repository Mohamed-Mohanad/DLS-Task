namespace DLS.Application.Features.Categories.Commands.CreateCategory;

internal sealed class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, long>
{
    private readonly IGenericRepository<Category> _categoryRepo;

    public CreateCategoryCommandHandler(IGenericRepository<Category> categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<Result<long>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = Category.Create(
            request.Name,
            request.Description,
            request.ParentId);

        await _categoryRepo.AddAsync(category, cancellationToken);
        await _categoryRepo.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}