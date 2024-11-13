namespace TodoList.API.Contracts.Users;

public record LoginUserRequest(
    string Login,
    string Password);
