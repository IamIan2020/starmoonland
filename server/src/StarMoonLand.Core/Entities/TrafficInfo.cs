namespace StarMoonLand.Core.Entities;

public class TrafficInfo
{
    public int Id { get; set; }
    public string TabName { get; set; } = string.Empty;
    public string? Content { get; set; }
    public int SortOrder { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
