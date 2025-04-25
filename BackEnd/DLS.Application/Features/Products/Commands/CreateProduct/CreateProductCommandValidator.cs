namespace DLS.Application.Features.Products.Commands.CreateProduct;

internal sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(IGenericRepository<Category> categoryRepo)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(string.Format(ErrorMessages.Required))
            .MaximumLength(MaxLength.Name)
                .WithMessage(string.Format(ErrorMessages.MaxLength, MaxLength.Name));

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(string.Format(ErrorMessages.Required))
            .MaximumLength(MaxLength.Description)
                .WithMessage(string.Format(ErrorMessages.MaxLength, MaxLength.Description));

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage(string.Format(ErrorMessages.GreaterThan, 0));

        RuleFor(x => x.CategoryId)
            .EntityExist(categoryRepo);
    }
}