using Microsoft.EntityFrameworkCore;
using TodoList.Core.Interfaces.Repositories;
using TodoList.Core.Models;
using TodoList.Persistence.Entities;

namespace TodoList.Persistence.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly TodoListDbContext _dbContext;

    public UsersRepository(TodoListDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<User?> GetByIdAsync(Guid id)
    {
        var userEntity = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
        
        return userEntity != null ? Map(userEntity) : null;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var userEntity = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
        
        return userEntity != null ? Map(userEntity) : null;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        var userEntity = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Name == username);
        
        return userEntity != null ? Map(userEntity) : null;
    }

    public async Task CreateAsync(User user)
    {
        var userEntity = new UserEntity
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            HashedPassword = user.HashedPassword,
            Notes = []
        };

        await _dbContext.Users.AddAsync(userEntity);
        await _dbContext.SaveChangesAsync();
    }
    
    private User Map(UserEntity entity)
    {
        return User.Create(entity.Id, entity.Name, entity.Email, entity.HashedPassword, []).User!;
    }
}