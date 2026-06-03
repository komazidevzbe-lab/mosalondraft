using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class UpdateContactMessageStatusDto
{
    // ===============================
    // Admin message update data
    // Used later by the admin dashboard to update message progress.
    // ===============================

    [Required(ErrorMessage = "Message status is required.")]
    [MaxLength(40, ErrorMessage = "Message status cannot be longer than 40 characters.")]
    public string MessageStatus { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "Admin response cannot be longer than 1000 characters.")]
    public string? AdminResponse { get; set; }
}