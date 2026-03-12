namespace StarMoonLand.Core.Entities;

public class Page
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string? Content { get; set; }
    public int SortOrder { get; set; }
    public bool IsVisible { get; set; } = true;
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public PageCategory? Category { get; set; }
    public ICollection<PageSlide> Slides { get; set; } = [];
    public ICollection<PageTab> Tabs { get; set; } = [];
}
