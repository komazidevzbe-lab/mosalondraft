namespace API.DTOs;

public class SalonServiceLengthOptionDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal PriceAddOn { get; set; }

    public int DisplayOrder { get; set; }
}