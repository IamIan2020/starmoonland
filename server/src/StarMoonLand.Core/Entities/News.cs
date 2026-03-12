namespace StarMoonLand.Core.Entities;

public class News
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Content { get; set; }
    public string? CoverImage { get; set; }
    public DateTime PublishDate { get; set; } = DateTime.UtcNow;
    public bool IsPublished { get; set; }
    public bool IsPinned { get; set; }
    public int ViewCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public NewsCategory Category { get; set; } = null!;
}
