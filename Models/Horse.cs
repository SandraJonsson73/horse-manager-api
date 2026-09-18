namespace HorseManager.Api.Models;

public class Horse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;
    public int BirthYear { get; set; }
    public string? Breeder { get; set; }
    public string Owner { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
