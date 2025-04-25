using DLS.Domain.Entities;

namespace Application.Abstractions.Authentication;

public interface IJwtProvider
{
    Task<string> Generate(User user);
}
