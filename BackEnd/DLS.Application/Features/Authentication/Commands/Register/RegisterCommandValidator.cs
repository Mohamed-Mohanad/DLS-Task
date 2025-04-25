using DLS.Application.Specifications.UserSpecs;

namespace DLS.Application.Features.Authentication.Commands.Register;

internal sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private readonly IGenericRepository<User> _userRepo;
    private readonly IPasswordService _passwordService;

    public RegisterCommandValidator(
        IGenericRepository<User> userRepo,
        IPasswordService passwordService)
    {
        _userRepo = userRepo;
        _passwordService = passwordService;

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(string.Format(ErrorMessages.Required))
            .MaximumLength(MaxLength.Username)
                .WithMessage(string.Format(ErrorMessages.MaxLength, MaxLength.Username))
            .MustAsync(BeUniqueUsername);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(string.Format(ErrorMessages.Required))
            .Must(passwordService.IsValidPassword)
                .WithMessage(string.Format(ErrorMessages.InvalidPassword))
            .MaximumLength(MaxLength.Password)
                .WithMessage(string.Format(ErrorMessages.MaxLength, MaxLength.Password));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(string.Format(ErrorMessages.Required))
            .MaximumLength(MaxLength.Name)
                .WithMessage(string.Format(ErrorMessages.MaxLength, MaxLength.Name));
    }

    private async Task<bool> BeUniqueUsername(string username, CancellationToken cancellationToken)
    {
        var spec = new GetUserByUsernameSpecification(username);
        var user = await _userRepo.GetEntityWithSpecAsync(spec);

        return user is null;
    }
}