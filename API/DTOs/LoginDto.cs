using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class LoginDto
{
    // ===============================
    // Login form data
    // Users sign in with email and password.
    // ===============================

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}