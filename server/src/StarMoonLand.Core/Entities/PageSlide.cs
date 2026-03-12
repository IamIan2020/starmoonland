namespace StarMoonLand.Core.Entities;

public class PageSlide
{
    public int Id { get; set; }
    public int PageId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int SortOrder { get; set; }

    public Page Page { get; set; } = null!;
}
