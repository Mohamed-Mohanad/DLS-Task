using DLS.Application.Specifications.CategorySpecs;

namespace DLS.Application.Features.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    private readonly IGenericRepository<Category> _categoryRepo;

    public UpdateProductCommandValidator(
        IGenericRepository<Product> productRepo,
        IGenericRepository<Category> categoryRepo)
    {
        _categoryRepo = categoryRepo;

        RuleFor(x => x.Id)
            .EntityExist(productRepo);

        When(x => !string.IsNullOrEmpty(x.Name), () =>
        {
            RuleFor(x => x.Name)
                .MaximumLength(MaxLength.Name)
                    .WithMessage(string.Format(ErrorMessages.MaxLength, MaxLength.Name));
        });

        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(MaxLength.Description)
                    .WithMessage(string.Format(ErrorMessages.MaxLength, MaxLength.Description));
        });

        When(x => x.Price.HasValue, () =>
        {
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage(string.Format(ErrorMessages.GreaterThan, 0));
        });

        When(x => x.CategoryId.HasValue, () =>
        {
            RuleFor(x => x.CategoryId)
                .MustAsync(CategoryExists).WithMessage(ErrorMessages.NotFound);
        });
    }

    private async Task<bool> CategoryExists(long? categoryId, CancellationToken cancellationToken)
    {
        if (!categoryId.HasValue) return true;

        var spec = new GetCategoryByIdSpecification(categoryId.Value);
        var category = await _categoryRepo.GetEntityWithSpecAsync(spec, cancellationToken);

        return category is not null;
    }
}