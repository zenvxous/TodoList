using Microsoft.EntityFrameworkCore;
using TodoList.Core.Interfaces.Repositories;
using TodoList.Core.Models;
using TodoList.Persistence.Entities;

namespace TodoList.Persistence.Repositories;

public class NotesRepository : INotesRepository
{
    private readonly TodoListDbContext _dbContext;

    public NotesRepository(TodoListDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Note?> GetByIdAsync(Guid id)
    {
        var noteEntity = await _dbContext.Notes
            .Include(n => n.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Id == id);
        
        return noteEntity != null ? Map(noteEntity) : null;
    }
    
    public async Task<List<Note>> GetAllAsync()
    {
        var notesEntities = await _dbContext.Notes
            .Include(n => n.User)
            .AsNoTracking()
            .ToListAsync();
        
        return notesEntities.Select(Map).ToList();
    }
    
    public async Task CreateAsync(Note note)
    {
        var noteEntity = new NoteEntity
        {
            Id = note.Id,
            Title = note.Title,
            Description = note.Description,
            CreationTime = note.CreationTime,
            UserId = note.UserId,
        };

        await _dbContext.Notes.AddAsync(noteEntity);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Guid id, string title, string description)
    {
        await _dbContext.Notes
            .Where(n => n.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(n => n.Title, title)
                .SetProperty(n => n.Description, description));
    }

    public async Task DeleteAsync(Guid id)
    {
        await _dbContext.Notes
            .Where(n => n.Id == id)
            .ExecuteDeleteAsync();
    }

    private Note Map(NoteEntity noteEntity)
    {
        var user = User.Create(
            noteEntity.User.Id,
            noteEntity.User.Name,
            noteEntity.User.Email,
            noteEntity.User.HashedPassword,
            []).User!;
        
        return Note.Create(
            noteEntity.Id,
            noteEntity.Title,
            noteEntity.Description,
            noteEntity.CreationTime,
            user).Note!;
    }
}