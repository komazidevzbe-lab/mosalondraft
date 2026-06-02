namespace API.Entities;

public class ContactMessage
{
    public int Id { get; set; }

    public required string FullName { get; set; }

    public required string EmailAddress { get; set; }

    public required string PhoneNumber { get; set; }

    public required string Interest { get; set; }

    public required string Message { get; set; }

    public int? UserId { get; set; }

    public AppUser? User { get; set; }

    public required string MessageStatus { get; set; }

    public string? AdminResponse { get; set; }

    public DateTime? RespondedAt { get; set; }

    public DateTime SubmittedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}