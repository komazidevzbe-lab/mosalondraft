namespace API.Entities;

public class PasswordResetCode
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public AppUser User { get; set; } = null!;

    public string CodeHash { get; set; } = string.Empty;

    public string IdentityResetToken { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }
}