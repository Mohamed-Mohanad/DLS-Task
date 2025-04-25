using DLS.Application.Specifications.UserSpecs;

namespace DLS.Application.Features.Authentication.Commands.Login;

internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, string>
{
    private readonly IGenericRepository<User> _userRepo;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(
        IGenericRepository<User> userRepo,
        IJwtProvider jwtProvider)
    {
        _userRepo = userRepo;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<string>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var spec = new GetUserByUsernameSpecification(request.Username);
        var user = await _userRepo.GetEntityWithSpecAsync(spec);

        var token = await _jwtProvider.Generate(user!);

        return token;
    }
}