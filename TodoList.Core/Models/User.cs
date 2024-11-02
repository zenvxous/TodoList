using TodoList.Core.Validators;

namespace TodoList.Core.Models;

public class User
{
    private User(Guid id, string name, string email, string hashedPassword, List<Note> notes)
    {
        Id = id;
        Name = name;
        Email = email;
        HashedPassword = hashedPassword;
        Notes = notes;
    }
    public Guid Id { get; private set; }
    
    public string Name { get; private set; }
    
    public string Email { get; private set; }
    
    public string HashedPassword { get; private set; }

    public List<Note> Notes { get; private set; }

    public static (string Error, User? User) Create(Guid id, string username, string email, string hashedPassword, List<Note> notes)
    {
        var error = UsersValidator.Validate(username, email);

        if (error != string.Empty)
            return (error, null);
        
        var user = new User(id, username, email, hashedPassword, notes);

        return (error, user);
    }
}