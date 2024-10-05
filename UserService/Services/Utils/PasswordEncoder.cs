namespace UserService.Services.Utils;

using BCrypt.Net;

public class PasswordEncoder
{
    public static string HashPassword(string password)
    {
        return BCrypt.HashPassword(password);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Verify(password, hashedPassword);
    }
}