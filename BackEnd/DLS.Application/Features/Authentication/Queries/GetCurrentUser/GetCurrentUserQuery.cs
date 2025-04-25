namespace DLS.Application.Features.Authentication.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery : IQuery<CurrentUserResponse>;

public sealed record CurrentUserResponse
{
    public long Id { get; init; }
    public string Username { get; init; } = null!;
    public string Role { get; init; } = null!;
}