namespace StarMoonLand.Core.Entities;

public class PageCategory
{
    public int Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string TitleZh { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public string? BannerImage { get; set; }
    public int SortOrder { get; set; }
    public bool IsVisible { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Page> Pages { get; set; } = [];
}
