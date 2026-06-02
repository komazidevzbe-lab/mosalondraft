namespace API.Entities;

public class HomePageContent
{
    public int Id { get; set; }

    public required string SectionKey { get; set; }

    public string? EyebrowText { get; set; }

    public required string TitleLineOne { get; set; }

    public string? TitleLineOneHighlight { get; set; }

    public string? TitleLineTwo { get; set; }

    public string? TitleLineTwoHighlight { get; set; }

    public required string Description { get; set; }

    public string? ButtonText { get; set; }

    public string? ButtonLink { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}