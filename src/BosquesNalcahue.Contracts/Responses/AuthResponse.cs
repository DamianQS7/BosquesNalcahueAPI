namespace BosquesNalcahue.Contracts.Responses;

public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public UserDto? UserInfo { get; set; }
}

public record UserDto(string Email, string Name, bool IsAdmin);