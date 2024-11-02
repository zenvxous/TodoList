using TodoList.Core.Models;

namespace TodoList.Core.Interfaces.Repositories;

public interface INotesRepositories
{
    Task<Note?> GetByIdAsync(Guid id);
    Task<List<Note>> GetAllAsync();
    Task CreateAsync(Note note);
    Task UpdateAsync(Guid id, string title, string description);
    Task DeleteAsync(Guid id);
}