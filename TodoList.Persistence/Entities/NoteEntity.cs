namespace TodoList.Persistence.Entities;

public class NoteEntity
{
    public Guid Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public DateTime CreationTime { get; set; }
    
    public Guid UserId { get; set; }
    
    public UserEntity User { get; set; }
    
}