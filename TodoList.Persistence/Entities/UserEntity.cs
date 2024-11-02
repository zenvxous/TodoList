namespace TodoList.Persistence.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string HashedPassword { get; set; } = string.Empty;

    public List<NoteEntity> Notes { get; set; } = [];
}