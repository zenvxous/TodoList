namespace TodoList.API.Contracts.Users;

public record RegisterUserRequest(
    string Username,
    string Email,
    string Password);
