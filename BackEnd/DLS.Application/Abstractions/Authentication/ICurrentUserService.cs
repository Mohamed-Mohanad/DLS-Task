namespace Application.Abstractions.Authentication;

public interface ICurrentUserService
{
    long? UserId { get; }
    bool IsAuthenticated { get; }
}