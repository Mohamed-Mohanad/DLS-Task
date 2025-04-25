namespace DLS.Domain.Constants;

public static class ErrorMessages
{
    public static readonly string NotFound = "The requested content was not found.";
    public static readonly string InternalServer = "An unexpected error occurred. Please try later.";
    public static readonly string Required = "is required.";
    public static readonly string MaxLength = "must not exceed {0} characters.";
    public static readonly string MinLength = "must be at least {0} characters.";
    public static readonly string GreaterThan = "must be at least {0} characters.";
    public static readonly string LessThan = "must be at least {0} characters.";
    public static readonly string InvalidCredentials = "Invalid username or password.";
    public static readonly string Unauthorized = "You are not authorized to access this resource.";
    public static readonly string InvalidPassword = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.";
    public static readonly string InvalidEnum = "Invalid value provided.";
    public static readonly string InvalidEmail = "Invalid email address format.";
}
