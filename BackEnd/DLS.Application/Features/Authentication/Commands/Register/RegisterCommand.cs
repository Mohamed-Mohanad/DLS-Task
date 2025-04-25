using DLS.Domain.Enum;

namespace DLS.Application.Features.Authentication.Commands.Register;

public sealed record RegisterCommand : ICommand<string>
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string Name { get; init; } = null!;
    public Role Role { get; init; }
}