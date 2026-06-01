using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class VerifyResetCodeDto
{
    [Required(ErrorMessage = "Email address or phone number is required.")]
    public string EmailOrPhone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Verification code is required.")]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "Verification code must be 6 digits.")]
    public string VerificationCode { get; set; } = string.Empty;
}