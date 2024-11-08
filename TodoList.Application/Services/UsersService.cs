using Microsoft.AspNetCore.Http;
using TodoList.Core.Interfaces.Auth;
using TodoList.Core.Interfaces.Repositories;
using TodoList.Core.Interfaces.Services;
using TodoList.Core.Models;

namespace TodoList.Application.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public UsersService(
        IUsersRepository usersRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _usersRepository = usersRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    private bool CheckUserLogin(User? user, string password)
    {
        if (user == null)
            return false;
        
        if(!_passwordHasher.VerifyHashedPassword(user.HashedPassword, password))
            return false;
        
        return true;
    }
    
    private async Task<bool> CheckUserExists(string username, string email)
    {
        if(await _usersRepository.GetByEmailAsync(email) == null && await _usersRepository.GetByUsernameAsync(username) == null)
            return false;
        
        return true;
    }
    
    public async Task<string> Register(string username, string password, string email)
    {
        if(await CheckUserExists(username, email))
            return "User already exists";
        
        var hashedPassword = _passwordHasher.HashPassword(password);
        
        var (error, user) = User.Create(Guid.NewGuid(), username, email, hashedPassword, []);
        
        if (!string.IsNullOrEmpty(error))
            return error;

        await _usersRepository.CreateAsync(user!);
        
        return string.Empty;
    }
    
    public async Task<string> LoginUserByUsername(string username, string password, HttpContext context)
    {
        var user  = await _usersRepository.GetByUsernameAsync(username);

        if (!CheckUserLogin(user, password))
            return string.Empty;
        
        var token = _jwtProvider.GenerateToken(user!);
        
        context.Response.Cookies.Append("Token", token);
        
        return token;
    }
    
    public async Task<string> LoginUserByEmail(string email, string password, HttpContext context)
    {
        var user = await _usersRepository.GetByEmailAsync(email);
        
        if (!CheckUserLogin(user, password))
            return string.Empty;
        
        var token = _jwtProvider.GenerateToken(user!);
        
        context.Response.Cookies.Append("Token", token);
        
        return token;
    }
}