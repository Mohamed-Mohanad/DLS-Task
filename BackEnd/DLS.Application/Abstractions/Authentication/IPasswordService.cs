namespace Application.Abstractions.Authentication;

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
    bool IsValidPassword(string password);
}