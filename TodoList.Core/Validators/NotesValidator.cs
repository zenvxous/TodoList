namespace TodoList.Core.Validators;

public static class NotesValidator
{
    private const int MAX_TITLE_LENGTH = 50;
    private const int MAX_DESCRIPTION_LENGTH = 200;
    
    public static string Validate(string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description))
            return "Title and/or description are required!";
        
        if (title.Length > MAX_TITLE_LENGTH || string.IsNullOrWhiteSpace(title))
            return "Title is too long!";

        if (description.Length > MAX_DESCRIPTION_LENGTH)
            return "Description is too long!";
        
        return string.Empty;
    }
}