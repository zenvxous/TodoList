using TodoList.Core.Validators;

namespace TodoList.Core.Models;

public class Note
{
    private Note(Guid id, string title, string description, DateTime creationTime, User user)
    {
        Id = id;
        Title = title;
        Description = description;
        CreationTime = creationTime;
        UserId = user.Id;
        User = user;
    }
    
    public Guid Id { get; private set; }
    
    public string Title { get; private set; }
    
    public string Description { get; private set; }
    
    public DateTime CreationTime { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public User User { get; private set; }

    public static (string Error, Note? Note) Create(Guid id, string title, string description, DateTime creationTime, User user)
    {
        var error = NotesValidator.Validate(title, description);

        if (error != string.Empty)
            return (error, null);
        
        var note = new Note(id, title, description, creationTime, user);
        
        user.Notes.Add(note);
        
        return (error, note);
    }
}