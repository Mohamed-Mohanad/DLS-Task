namespace DLS.Application.Features.Authentication.Commands.Login;

public sealed record LoginCommand : ICommand<string>
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}