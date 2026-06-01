using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class ForgotPasswordDto
{
    [Required(ErrorMessage = "Email address or phone number is required.")]
    public string EmailOrPhone { get; set; } = string.Empty;
}