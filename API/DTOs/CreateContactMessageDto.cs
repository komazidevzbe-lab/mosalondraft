using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CreateContactMessageDto
{
    // ===============================
    // Contact message form data
    // The logged-in user's name, email, and phone number come from AppUser.
    // ===============================

    [Required(ErrorMessage = "Interest is required.")]
    [MaxLength(80, ErrorMessage = "Interest cannot be longer than 80 characters.")]
    public string Interest { get; set; } = string.Empty;

    [Required(ErrorMessage = "Message is required.")]
    [MinLength(5, ErrorMessage = "Message must be at least 5 characters.")]
    [MaxLength(1000, ErrorMessage = "Message cannot be longer than 1000 characters.")]
    public string Message { get; set; } = string.Empty;
}