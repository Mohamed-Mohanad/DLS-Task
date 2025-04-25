namespace DLS.Application.Features.Authentication.Commands.Register;

internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, string>
{
    private readonly IGenericRepository<User> _userRepo;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordService _passwordService;

    public RegisterCommandHandler(
        IGenericRepository<User> userRepo,
        IJwtProvider jwtProvider,
        IPasswordService passwordService)
    {
        _userRepo = userRepo;
        _jwtProvider = jwtProvider;
        _passwordService = passwordService;
    }

    public async Task<Result<string>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var hashedPassword = _passwordService.HashPassword(request.Password);

        var user = User.Create(
            request.Username,
            hashedPassword,
            request.Name,
            request.Role);

        await _userRepo.AddAsync(user, cancellationToken);
        await _userRepo.SaveChangesAsync(cancellationToken);

        var token = await _jwtProvider.Generate(user);

        return token;
    }
}