namespace API.Entities;

public class SalonServiceType
{
    public int Id { get; set; }

    public int SalonServiceId { get; set; }
    public SalonService SalonService { get; set; } = null!;

    public required string Name { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}