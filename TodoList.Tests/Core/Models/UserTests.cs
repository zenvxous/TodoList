using TodoList.Core.Models;

namespace TodoList.Tests.Core.Models;

public class UserTests
{
    [Fact]
    public void Create_User_ValidInput_ReturnsUser()
    {
        // Arrange
        var id = Guid.NewGuid();
        var username = "testUser";
        var email = "test@example.com";
        var hashedPassword = "hashedPassword123";
        var notes = new List<Note>();

        // Act
        var (error, user) = User.Create(id, username, email, hashedPassword, notes);

        // Assert
        Assert.Empty(error);
        Assert.NotNull(user);
        Assert.Equal(id, user.Id);
        Assert.Equal(username, user.Name);
        Assert.Equal(email, user.Email);
        Assert.Equal(hashedPassword, user.HashedPassword);
        Assert.Equal(notes, user.Notes);
    }
    
    [Fact]
    public void Create_User_EmptyUsername_ReturnsError()
    {
        // Arrange
        var id = Guid.NewGuid();
        var username = ""; // Empty username
        var email = "test@example.com";
        var hashedPassword = "hashedPassword123";
        var notes = new List<Note>();

        // Act
        var (error, user) = User.Create(id, username, email, hashedPassword, notes);

        // Assert
        Assert.NotEmpty(error);
        Assert.Equal("Username and/or email address are required!", error);
        Assert.Null(user);
    }
    
    [Fact]
    public void Create_User_EmptyEmail_ReturnsError()
    {
        // Arrange
        var id = Guid.NewGuid();
        var username = "testUser"; 
        var email = ""; // Empty email
        var hashedPassword = "hashedPassword123";
        var notes = new List<Note>();

        // Act
        var (error, user) = User.Create(id, username, email, hashedPassword, notes);

        // Assert
        Assert.NotEmpty(error);
        Assert.Equal("Username and/or email address are required!", error);
        Assert.Null(user);
    }

    [Fact]
    public void Create_User_LongUsername_ReturnsError()
    {
        // Arrange
        var id = Guid.NewGuid();
        var username = "Long usernameLong usernameLong usernameLong username"; // Long username
        var email = "test@example.com";
        var hashedPassword = "hashedPassword123";
        var notes = new List<Note>();
        
        // Act
        var (error, user) = User.Create(id, username, email, hashedPassword, notes);
        
        // Assert
        Assert.NotEmpty(error);
        Assert.Equal("Username is too long!", error);
        Assert.Null(user);
    }
    
    [Fact]
    public void Create_User_InvalidEmail_ReturnsError()
    {
        // Arrange
        var id = Guid.NewGuid();
        var username = "testUser";
        var email = "invalidEmail"; // Invalid email
        var hashedPassword = "hashedPassword123";
        var notes = new List<Note>();

        // Act
        var (error, user) = User.Create(id, username, email, hashedPassword, notes);

        // Assert
        Assert.NotEmpty(error);
        Assert.Equal("Invalid email address!", error);
        Assert.Null(user);
    }
}