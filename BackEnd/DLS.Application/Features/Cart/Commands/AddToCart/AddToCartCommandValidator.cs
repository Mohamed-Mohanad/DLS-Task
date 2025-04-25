namespace DLS.Application.Features.Cart.Commands.AddToCart;

internal sealed class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
{
    public AddToCartCommandValidator(
        IGenericRepository<Product> productRepo,
        IGenericRepository<User> userRepo)
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage(string.Format(ErrorMessages.GreaterThan, 0))
            .EntityExist(userRepo);

        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage(string.Format(ErrorMessages.GreaterThan, 0))
            .EntityExist(productRepo);

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage(string.Format(ErrorMessages.GreaterThan, 0));
    }
}