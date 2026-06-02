namespace API.DTOs;

public class HomePageContentDto
{
    public string SectionKey { get; set; } = string.Empty;

    public string? EyebrowText { get; set; }

    public string TitleLineOne { get; set; } = string.Empty;

    public string? TitleLineOneHighlight { get; set; }

    public string? TitleLineTwo { get; set; }

    public string? TitleLineTwoHighlight { get; set; }

    public string Description { get; set; } = string.Empty;

    public string? ButtonText { get; set; }

    public string? ButtonLink { get; set; }
}