using TodoList.Core.Interfaces.Auth;

namespace TodoList.Infrastructure.Auth;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    
    public bool VerifyHashedPassword(string hashedPassword, string password) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
}