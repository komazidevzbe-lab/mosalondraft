using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace API.DTOs;

public class CreateClientReviewDto
{
    [MaxLength(160, ErrorMessage = "Location cannot be longer than 160 characters.")]
    public string? Location { get; set; }

    [Required(ErrorMessage = "Review text is required.")]
    [MinLength(5, ErrorMessage = "Review text must be at least 5 characters.")]
    [MaxLength(1000, ErrorMessage = "Review text cannot be longer than 1000 characters.")]
    public string ReviewText { get; set; } = string.Empty;

    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; set; }

    public IFormFile? Image { get; set; }
}