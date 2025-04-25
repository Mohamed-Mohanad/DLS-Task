using DLS.Domain.Enum;
using Domain.Primitives;

namespace DLS.Domain.Entities;

public class User : AggregateRoot<long>, IAuditableEntity
{
    public string Username { get; private set; } = null!;
    public string Password { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public Role Role { get; private set; }
    public DateTime CreatedOnUtc { get; }
    public DateTime? ModifiedOnUtc { get; }

    private User() { }

    public static User Create(string username, string password, string name, Role role)
    {
        return new User
        {
            Username = username,
            Password = password,
            Name = name,
            Role = role,
        };
    }
}