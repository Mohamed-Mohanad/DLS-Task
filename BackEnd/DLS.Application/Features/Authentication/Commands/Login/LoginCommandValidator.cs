using DLS.Application.Specifications.UserSpecs;

namespace DLS.Application.Features.Authentication.Commands.Login;

internal sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    private readonly IGenericRepository<User> _userRepo;
    private readonly IPasswordService _passwordService;

    public LoginCommandValidator(IGenericRepository<User> userRepo, IPasswordService passwordService)
    {
        _userRepo = userRepo;
        _passwordService = passwordService;

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(string.Format(ErrorMessages.Required))
            .MaximumLength(MaxLength.Username)
                .WithMessage(string.Format(ErrorMessages.MaxLength, MaxLength.Username));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(string.Format(ErrorMessages.Required))
            .Must(passwordService.IsValidPassword)
                .WithMessage(string.Format(ErrorMessages.InvalidPassword))
            .MaximumLength(MaxLength.Password)
                .WithMessage(string.Format(ErrorMessages.MaxLength, MaxLength.Password));

        RuleFor(x => x)
            .MustAsync(ValidCredentials).WithMessage(ErrorMessages.InvalidCredentials);
    }

    private async Task<bool> ValidCredentials(LoginCommand command, CancellationToken cancellationToken)
    {
        var spec = new GetUserByUsernameSpecification(command.Username);
        var user = await _userRepo.GetEntityWithSpecAsync(spec);

        if (user is null || !_passwordService.VerifyPassword(command.Password, user.Password)) return false;

        return true;
    }


}