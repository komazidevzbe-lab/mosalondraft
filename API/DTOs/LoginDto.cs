using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class LoginDto
{
    // ===============================
    // Login form data
    // Users sign in with email and password.
    // ===============================

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}