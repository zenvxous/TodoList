namespace TodoList.Infrastructure.Auth;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;

    public int ExpiresMinutes { get; set; }
}