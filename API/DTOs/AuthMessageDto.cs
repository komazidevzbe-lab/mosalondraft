namespace API.DTOs;

public class AuthMessageDto
{
    public string Message { get; set; } = string.Empty;

    public string? DevelopmentResetCode { get; set; }
}