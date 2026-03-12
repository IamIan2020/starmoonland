namespace StarMoonLand.Core.Entities;

public class PageTab
{
    public int Id { get; set; }
    public int PageId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public int SortOrder { get; set; }

    public Page? Page { get; set; }
}
