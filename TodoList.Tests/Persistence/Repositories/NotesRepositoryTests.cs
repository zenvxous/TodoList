using Microsoft.EntityFrameworkCore;
using TodoList.Core.Models;
using TodoList.Persistence;
using TodoList.Persistence.Entities;
using TodoList.Persistence.Repositories;

namespace TodoList.Tests.Persistence.Repositories;

public class NotesRepositoryTests : IDisposable
{
    private const string CONNECTION_STRING = "Server=localhost,1433;Database=TodoList_NotesTests;User Id=sa;Password=Tod0List!;TrustServerCertificate=True;";
    
    private readonly TodoListDbContext _context;
    private readonly NotesRepository _repository;
    
    public NotesRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TodoListDbContext>()
            .UseSqlServer(CONNECTION_STRING)
            .Options;
        
        _context = new TodoListDbContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.Migrate();
        
        _repository = new NotesRepository(_context);
    }
    
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task GetByIdAsync_NoteExists_ReturnsNote()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userEntity = new UserEntity
        {
            Id = userId,
            Name = "TestUsername",
            Email = "test@test.com",
            HashedPassword = "test password",
            Notes = []
        };
        
        var noteId = Guid.NewGuid();
        var noteEntity = new NoteEntity
        {
            Id = noteId,
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
        var note = await _repository.GetByIdAsync(noteId);
        
        // Assert
        Assert.NotNull(note);
        Assert.Equal(noteId, note.Id);
        Assert.Equal(noteEntity.CreationTime, note.CreationTime);
        Assert.Equal(noteEntity.Description, note.Description);
        Assert.Equal(noteEntity.Title, note.Title);
        Assert.Equal(userId, note.UserId);
    }

    [Fact]
    public async Task GetByIdAsync_NoteDoesNotExists_ReturnsNull()
    {
        // Arrange
        var noteId = Guid.NewGuid();
        
        // Act
        var note = await _repository.GetByIdAsync(noteId);
        
        // Assert
        Assert.Null(note);
    }

    [Fact]
    public async Task GetAllAsync_NotesExists_ReturnsAllNotes()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userEntity1 = new UserEntity
        {
            Id = userId1,
            Name = "AuthorOne",
            Email = "author1@test.com",
            HashedPassword = "password1",
            Notes = new List<NoteEntity>()
        };

        var userId2 = Guid.NewGuid();
        var userEntity2 = new UserEntity
        {
            Id = userId2,
            Name = "AuthorTwo",
            Email = "author2@test.com",
            HashedPassword = "password2",
            Notes = new List<NoteEntity>()
        };

        var noteId1 = Guid.NewGuid();
        var noteEntity1 = new NoteEntity
        {
            Id = noteId1,
            CreationTime = DateTime.Now,
            Description = "Description for note 1",
            Title = "Note 1",
            User = userEntity1,
            UserId = userId1
        };
        userEntity1.Notes.Add(noteEntity1);

        var noteId2 = Guid.NewGuid();
        var noteEntity2 = new NoteEntity
        {
            Id = noteId2,
            CreationTime = DateTime.Now.AddMinutes(1),
            Description = "Description for note 2",
            Title = "Note 2",
            User = userEntity1,
            UserId = userId1
        };
        userEntity1.Notes.Add(noteEntity2);

        var noteId3 = Guid.NewGuid();
        var noteEntity3 = new NoteEntity
        {
            Id = noteId3,
            CreationTime = DateTime.Now.AddMinutes(2),
            Description = "Description for note 3",
            Title = "Note 3",
            User = userEntity2,
            UserId = userId2
        };
        userEntity2.Notes.Add(noteEntity3);

        await _context.Users.AddRangeAsync(userEntity1, userEntity2);
        await _context.SaveChangesAsync();

        // Act
        var notes = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(notes);
        Assert.Equal(3, notes.Count);
        Assert.Contains(notes, n => n.Id == noteId1);
        Assert.Contains(notes, n => n.Id == noteId2);
        Assert.Contains(notes, n => n.Id == noteId3);
        Assert.Equal(userId1, notes.First(n => n.Id == noteId1).UserId);
        Assert.Equal(userId1, notes.First(n => n.Id == noteId2).UserId);
        Assert.Equal(userId2, notes.First(n => n.Id == noteId3).UserId);
    }
    
    [Fact]
    public async Task GetAllAsync_NoNotesExist_ReturnsEmptyList()
    {
        // Arrange

        // Act
        var notes = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(notes);
        Assert.Empty(notes);
    }
    
    [Fact]
    public async Task CreateAsync_ValidNote_NoteIsCreated()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userEntity = new UserEntity
        {
            Id = userId,
            Name = "TestUser",
            Email = "testuser@test.com",
            HashedPassword = "hashedpassword",
            Notes = []
        };
        
        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
        
        var user = User.Create(
            userId,
            userEntity.Name,
            userEntity.Email,
            userEntity.HashedPassword,
            []).User!;
        
        var noteId = Guid.NewGuid();
        var title = "Test note";
        var description = "This is a test note";
        var creationTime = DateTime.Now;
        
        var note = Note.Create(
            noteId,
            title,
            description,
            creationTime,
            user).Note!;

        // Act
        await _repository.CreateAsync(note);

        // Assert
        var createdNote = await _context.Notes.FindAsync(note.Id);
        Assert.NotNull(createdNote);
        Assert.Equal(noteId, createdNote.Id);
        Assert.Equal(title, createdNote.Title);
        Assert.Equal(description, createdNote.Description);
        Assert.Equal(creationTime, createdNote.CreationTime);
        Assert.Equal(userId, createdNote.UserId);
    }

    [Fact]
    public async Task UpdateAsync_ExistingNote_UpdatesNote()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userEntity = new UserEntity
        {
            Id = userId,
            Name = "TestUsername",
            Email = "test@test.com",
            HashedPassword = "test password",
            Notes = []
        };
        
        var noteId = Guid.NewGuid();
        var noteEntity = new NoteEntity
        {
            Id = noteId,
            CreationTime = DateTime.Now,
            Description = "description",
            Title = "Test note",
            User = userEntity,
            UserId = userId
        };
        userEntity.Notes.Add(noteEntity);
        
        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();

        var newTitle = "Updated Title";
        var newDescription = "Updated Description";

        // Act
        await _repository.UpdateAsync(noteId, newTitle, newDescription);

        // Assert
        var updatedNote = await _repository.GetByIdAsync(noteId);
        Assert.NotNull(updatedNote);
        Assert.Equal(newTitle, updatedNote.Title);
        Assert.Equal(newDescription, updatedNote.Description);
    }

    [Fact]
    public async Task DeleteAsync_ExistingNote_DeletesNote()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userEntity = new UserEntity
        {
            Id = userId,
            Name = "TestUsername",
            Email = "test@test.com",
            HashedPassword = "test password",
            Notes = []
        };
        
        var noteId = Guid.NewGuid();
        var noteEntity = new NoteEntity
        {
            Id = noteId,
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
        await _repository.DeleteAsync(noteId);
        
        // Assert
        var deletedNote = await _repository.GetByIdAsync(noteId);
        Assert.Null(deletedNote);
    }
}