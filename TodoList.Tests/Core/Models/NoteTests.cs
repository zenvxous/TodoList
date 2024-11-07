using TodoList.Core.Models;

namespace TodoList.Tests.Core.Models;

public class NoteTests
{
    [Fact]
    public void Note_Create_ValidInput_ReturnsNote()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = User.Create(userId, "username", "email@email.com", "password", []).User!;
        var noteId = Guid.NewGuid();
        var title = "Test note";
        var description = "This is a test note";
        var creationTime = DateTime.Now;
        
        // Act
        var (error, note) = Note.Create(noteId, title, description, creationTime, user);
        
        // Assert
        Assert.Empty(error);
        Assert.NotNull(note);
        Assert.Equal(noteId, note.Id);
        Assert.Equal(title, note.Title);
        Assert.Equal(description, note.Description);
        Assert.Equal(creationTime, note.CreationTime);
        Assert.Equal(userId, note.UserId);
        Assert.Equal(user, note.User);
        Assert.Contains(note, user.Notes);
    }
    
    [Fact]
    public void Create_Note_EmptyTitle_ReturnsError()
    {
        // Arrange
        var user = User.Create(Guid.NewGuid(), "testUser", "test@example.com", "hashedPassword123", []).User!;
        var noteId = Guid.NewGuid();
        var title = ""; // Empty title
        var description = "This is a test note.";
        var creationTime = DateTime.Now;

        // Act
        var (error, note) = Note.Create(noteId, title, description, creationTime, user);

        // Assert
        Assert.NotEmpty(error);
        Assert.Equal("Title and/or description are required!", error);
        Assert.Null(note);
    }
    
    [Fact]
    public void Create_Note_EmptyDescription_ReturnsError()
    {
        // Arrange
        var user = User.Create(Guid.NewGuid(), "testUser", "test@example.com", "hashedPassword123", []).User!;
        var noteId = Guid.NewGuid();
        var title = "Test Note";
        var description = ""; // Empty description
        var creationTime = DateTime.Now;

        // Act
        var (error, note) = Note.Create(noteId, title, description, creationTime, user);

        // Assert
        Assert.NotEmpty(error);
        Assert.Equal("Title and/or description are required!", error);
        Assert.Null(note);
    }

    [Fact]
    public void Create_Note_LongDescription_ReturnsError()
    {
        // Arrange
        var user = User.Create(Guid.NewGuid(), "testUser", "test@example.com", "hashedPassword123", []).User!;
        var noteId = Guid.NewGuid();
        var title = "Test Note";
        var description = "Very long descriptionVery long descriptionVery long descriptionVery long descriptionVery long descriptionVery long descriptionVery long descriptionVery long descriptionVery long descriptionVery long descriptionVery long description";  // Long description
        var creationTime = DateTime.Now;
        
        // Act
        var (error, note) = Note.Create(noteId, title, description, creationTime, user);
        
        // Assert
        Assert.NotEmpty(error);
        Assert.Equal("Description is too long!", error);
        Assert.Null(note);
    }

    [Fact]
    public void Create_Note_LongTitle_ReturnsError()
    {
        // Arrange
        var user = User.Create(Guid.NewGuid(), "testUser", "test@example.com", "hashedPassword123", []).User!;
        var noteId = Guid.NewGuid();
        var title = "Long titleLong titleLong titleLong titleLong title "; // Long title
        var description = "This is a test note.";
        var creationTime = DateTime.Now;
        
        // Act
        var (error, note) = Note.Create(noteId, title, description, creationTime, user);

        // Assert
        Assert.NotEmpty(error);
        Assert.Equal("Title is too long!", error);
        Assert.Null(note);
    }
}