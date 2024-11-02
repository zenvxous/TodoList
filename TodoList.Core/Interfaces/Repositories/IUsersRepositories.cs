using TodoList.Core.Models;

namespace TodoList.Core.Interfaces.Repositories;

public interface IUsersRepositories
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task CreateAsync(User user);
}