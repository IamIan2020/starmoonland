using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarMoonLand.Api.DTOs;
using StarMoonLand.Infrastructure.Data;

namespace StarMoonLand.Api.Controllers.Public;

[ApiController]
[Route("api/pages")]
public class PagesController : ControllerBase
{
    private readonly AppDbContext _db;
    public PagesController(AppDbContext db) => _db = db;

    [HttpGet("{categorySlug}")]
    public async Task<IActionResult> GetCategory(string categorySlug)
    {
        var category = await _db.PageCategories
            .Include(c => c.Pages.Where(p => p.IsVisible).OrderBy(p => p.SortOrder))
            .FirstOrDefaultAsync(c => c.Slug == categorySlug && c.IsVisible);

        if (category == null) return NotFound(ApiResponse.Fail("分類不存在"));

        var result = new
        {
            category.Id, category.Slug, category.TitleZh, category.TitleEn, category.BannerImage,
            Pages = category.Pages.Select(p => new { p.Id, p.Slug, p.Title, p.Subtitle, p.SortOrder })
        };
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("{categorySlug}/{pageSlug}")]
    public async Task<IActionResult> GetPage(string categorySlug, string pageSlug)
    {
        var page = await _db.Pages
            .Include(p => p.Category)
            .Include(p => p.Slides.OrderBy(s => s.SortOrder))
            .Include(p => p.Tabs.OrderBy(t => t.SortOrder))
            .FirstOrDefaultAsync(p => p.Category.Slug == categorySlug && p.Slug == pageSlug && p.IsVisible);

        if (page == null) return NotFound(ApiResponse.Fail("頁面不存在"));

        var result = new
        {
            page.Id, page.CategoryId, page.Slug, page.Title, page.Subtitle, page.Content,
            page.SortOrder, page.IsVisible, page.MetaTitle, page.MetaDescription,
            Slides = page.Slides.Select(s => new { s.Id, s.ImageUrl, s.Title, s.Description, s.SortOrder }),
            Tabs = page.Tabs.Select(t => new { t.Id, t.Title, t.Content, t.SortOrder })
        };
        return Ok(ApiResponse<object>.Ok(result));
    }
}
