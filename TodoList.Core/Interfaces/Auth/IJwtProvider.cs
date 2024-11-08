using TodoList.Core.Models;

namespace TodoList.Core.Interfaces.Auth;

public interface IJwtProvider
{
    string GenerateToken(User user);
}