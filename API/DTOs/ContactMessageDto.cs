namespace API.DTOs;

public class ContactMessageDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Interest { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string MessageStatus { get; set; } = string.Empty;

    public List<string> Statuses { get; set; } = [];

    public string? AdminResponse { get; set; }

    public DateTime? RespondedAt { get; set; }

    public DateTime SubmittedAt { get; set; }
}