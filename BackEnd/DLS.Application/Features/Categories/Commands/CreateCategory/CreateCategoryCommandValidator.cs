namespace DLS.Application.Features.Categories.Commands.CreateCategory;

internal sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    private readonly IGenericRepository<Category> _categoryRepo;

    public CreateCategoryCommandValidator(IGenericRepository<Category> categoryRepo)
    {
        _categoryRepo = categoryRepo;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(string.Format(ErrorMessages.Required))
            .MaximumLength(MaxLength.Name)
                .WithMessage(string.Format(ErrorMessages.MaxLength, MaxLength.Name));

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(string.Format(ErrorMessages.Required))
            .MaximumLength(MaxLength.Description)
                .WithMessage(string.Format(ErrorMessages.MaxLength, MaxLength.Description));

        RuleFor(x => x.ParentId)
            .GreaterThan(0)
            .MustAsync(async (id, CancellationToken) => await _categoryRepo.IsExistAsync(a => a.Id == id))
            .When(x => x.ParentId.HasValue);
    }
}