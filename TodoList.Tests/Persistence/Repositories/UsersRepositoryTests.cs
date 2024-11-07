using Microsoft.EntityFrameworkCore;
using TodoList.Persistence;
using TodoList.Persistence.Entities;
using TodoList.Persistence.Repositories;

namespace TodoList.Tests.Persistence.Repositories;

public class UsersRepositoryTests : IDisposable
{
    private const string CONNECTION_STRING = "Server=localhost,1433;Database=TodoList_Tests;User Id=sa;Password=Tod0List!;TrustServerCertificate=True;";
    
    private readonly TodoListDbContext _context;
    private readonly UsersRepository _repository;
    
    public UsersRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TodoListDbContext>()
            .UseSqlServer(CONNECTION_STRING)
            .Options;
        
        _context = new TodoListDbContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.Migrate();
        
        _repository = new UsersRepository(_context);
    }
    
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task GetByIdAsync_UserExists_ReturnsUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userEntity = new UserEntity
        {
            Id = userId,
            Name = "Test Username",
            Email = "test@test.com",
            HashedPassword = "test password",
            Notes = []
        };

        var noteEntity = new NoteEntity
        {
            Id = Guid.NewGuid(),
            CreationTime = DateTime.Now,
            Description = "description",
            Title = "Test note",
            User = userEntity,
            UserId = userId
        };
        userEntity.Notes.Add(noteEntity);
        
        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
        
        // Act
        var user = await _repository.GetByIdAsync(userId);
        
        // Assert
        Assert.NotNull(user);
        Assert.Equal(userId, user.Id);
        Assert.Equal(userEntity.Name, user.Name);
        Assert.Equal(userEntity.Email, user.Email);
        Assert.Equal(userEntity.HashedPassword, user.HashedPassword);
        Assert.NotEmpty(user.Notes);
        Assert.Equal(userEntity.Notes.Count, user.Notes.Count);
        Assert.Equal(userEntity.Notes[0].Id, user.Notes[0].Id);
        Assert.Equal(userEntity.Notes[0].Title, user.Notes[0].Title);
        Assert.Equal(userEntity.Notes[0].Description, user.Notes[0].Description);
        Assert.Equal(userEntity.Notes[0].CreationTime, user.Notes[0].CreationTime);
        Assert.Equal(userEntity.Notes[0].UserId, user.Notes[0].UserId);
    }

    [Fact]
    public async Task GetByIdAsync_UserDoesNotExist_ReturnsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        
        // Act
        var user = await _repository.GetByIdAsync(userId);
        
        // Assert
        Assert.Null(user);
    }

    [Fact]
    public async Task GetByEmailAsync_UserExists_ReturnsUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@test.com";
        var userEntity = new UserEntity
        {
            Id = userId,
            Name = "Test Username",
            Email = email,
            HashedPassword = "test password",
            Notes = []
        };

        var noteEntity = new NoteEntity
        {
            Id = Guid.NewGuid(),
            CreationTime = DateTime.Now,
            Description = "description",
            Title = "Test note",
            User = userEntity,
            UserId = userId
        };
        userEntity.Notes.Add(noteEntity);
        
        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
        
        // Act
        var user = await _repository.GetByEmailAsync(email);
        
        // Assert
        Assert.NotNull(user);
        Assert.Equal(userId, user.Id);
        Assert.Equal(userEntity.Name, user.Name);
        Assert.Equal(userEntity.Email, user.Email);
        Assert.Equal(userEntity.HashedPassword, user.HashedPassword);
        Assert.NotEmpty(user.Notes);
        Assert.Equal(userEntity.Notes.Count, user.Notes.Count);
        Assert.Equal(userEntity.Notes[0].Id, user.Notes[0].Id);
        Assert.Equal(userEntity.Notes[0].Title, user.Notes[0].Title);
        Assert.Equal(userEntity.Notes[0].Description, user.Notes[0].Description);
        Assert.Equal(userEntity.Notes[0].CreationTime, user.Notes[0].CreationTime);
        Assert.Equal(userEntity.Notes[0].UserId, user.Notes[0].UserId);
    }

    [Fact]
    public async Task GetByEmailAsync_UserDoesNotExist_ReturnsNull()
    {
        // Arrange
        var email = "test@test.com";
        
        // Act
        var user = await _repository.GetByEmailAsync(email);
        
        // Assert
        Assert.Null(user);
    }
    
    [Fact]
    public async Task GetByUsernameAsync_UserExists_ReturnsUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var username = "Test Username";
        var userEntity = new UserEntity
        {
            Id = userId,
            Name = username,
            Email = "test@test.com",
            HashedPassword = "test password",
            Notes = []
        };

        var noteEntity = new NoteEntity
        {
            Id = Guid.NewGuid(),
            CreationTime = DateTime.Now,
            Description = "description",
            Title = "Test note",
            User = userEntity,
            UserId = userId
        };
        userEntity.Notes.Add(noteEntity);
        
        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
        
        // Act
        var user = await _repository.GetByUsernameAsync(username);
        
        // Assert
        Assert.NotNull(user);
        Assert.Equal(userId, user.Id);
        Assert.Equal(userEntity.Name, user.Name);
        Assert.Equal(userEntity.Email, user.Email);
        Assert.Equal(userEntity.HashedPassword, user.HashedPassword);
        Assert.NotEmpty(user.Notes);
        Assert.Equal(userEntity.Notes.Count, user.Notes.Count);
        Assert.Equal(userEntity.Notes[0].Id, user.Notes[0].Id);
        Assert.Equal(userEntity.Notes[0].Title, user.Notes[0].Title);
        Assert.Equal(userEntity.Notes[0].Description, user.Notes[0].Description);
        Assert.Equal(userEntity.Notes[0].CreationTime, user.Notes[0].CreationTime);
        Assert.Equal(userEntity.Notes[0].UserId, user.Notes[0].UserId);
    }

    [Fact]
    public async Task GetByUsernameAsync_UserDoesNotExist_ReturnsNull()
    {
        // Arrange
        var username = "Test Username";
        
        // Act
        var user = await _repository.GetByUsernameAsync(username);
        
        // Assert
        Assert.Null(user);
    }   
}