using Microsoft.AspNetCore.Http;

namespace TodoList.Core.Interfaces.Services;

public interface IUsersService
{
    Task<string> RegisterUser(string username, string password, string email);
    Task<string> LoginUserByUsername(string username, string password, HttpContext context);
    Task<string> LoginUserByEmail(string email, string password, HttpContext context);
}