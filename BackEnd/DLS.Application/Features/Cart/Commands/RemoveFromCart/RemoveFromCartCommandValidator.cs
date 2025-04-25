using Application.Extensions.FluentValidator;

namespace DLS.Application.Features.Cart.Commands.RemoveFromCart;

internal class RemoveFromCartCommandValidator : AbstractValidator<RemoveFromCartCommand>
{
    public RemoveFromCartCommandValidator(
        IGenericRepository<User> userRepo,
        IGenericRepository<Product> productRepo)
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage(string.Format(ErrorMessages.GreaterThan, 0))
            .EntityExist(userRepo);

        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage(string.Format(ErrorMessages.GreaterThan, 0))
            .EntityExist(productRepo);
    }
}
