using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CreateContactMessageDto
{
    [Required(ErrorMessage = "Full name is required.")]
    [MinLength(2, ErrorMessage = "Full name must be at least 2 characters.")]
    [MaxLength(120, ErrorMessage = "Full name cannot be longer than 120 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [MaxLength(160, ErrorMessage = "Email address cannot be longer than 160 characters.")]
    public string EmailAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(
        @"^(\+27|0)\s?[6-8](\s?\d){8}$",
        ErrorMessage = "Please enter a valid South African phone number.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Interest is required.")]
    [MaxLength(80, ErrorMessage = "Interest cannot be longer than 80 characters.")]
    public string Interest { get; set; } = string.Empty;

    [Required(ErrorMessage = "Message is required.")]
    [MinLength(5, ErrorMessage = "Message must be at least 5 characters.")]
    [MaxLength(1000, ErrorMessage = "Message cannot be longer than 1000 characters.")]
    public string Message { get; set; } = string.Empty;
}