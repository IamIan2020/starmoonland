using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarMoonLand.Api.DTOs;
using StarMoonLand.Core.Entities;
using StarMoonLand.Infrastructure.Data;

namespace StarMoonLand.Api.Controllers.Admin;

[ApiController]
[Route("api/admin")]
[Authorize]
public class AdminPagesController : ControllerBase
{
    private readonly AppDbContext _db;
    public AdminPagesController(AppDbContext db) => _db = db;

    // Page Categories
    [HttpGet("page-categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _db.PageCategories
            .OrderBy(c => c.SortOrder)
            .Select(c => new
            {
                c.Id, c.Slug, c.TitleZh, c.TitleEn, c.BannerImage, c.SortOrder, c.IsVisible,
                Pages = c.Pages.OrderBy(p => p.SortOrder).Select(p => new
                {
                    p.Id, p.Slug, p.Title, p.Subtitle, p.SortOrder, p.IsVisible
                })
            })
            .ToListAsync();
        return Ok(ApiResponse<object>.Ok(categories));
    }

    [HttpPost("page-categories")]
    public async Task<IActionResult> CreateCategory([FromBody] PageCategory category)
    {
        if (string.IsNullOrWhiteSpace(category.Slug) || string.IsNullOrWhiteSpace(category.TitleZh))
            return BadRequest(ApiResponse.Fail("Slug 和中文標題不可為空"));

        _db.PageCategories.Add(category);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(category));
    }

    [HttpPut("page-categories/{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] PageCategory input)
    {
        var category = await _db.PageCategories.FindAsync(id);
        if (category == null) return NotFound(ApiResponse.Fail("分類不存在"));

        if (string.IsNullOrWhiteSpace(input.Slug) || string.IsNullOrWhiteSpace(input.TitleZh))
            return BadRequest(ApiResponse.Fail("Slug 和中文標題不可為空"));

        category.Slug = input.Slug;
        category.TitleZh = input.TitleZh;
        category.TitleEn = input.TitleEn;
        category.BannerImage = input.BannerImage;
        category.SortOrder = input.SortOrder;
        category.IsVisible = input.IsVisible;
        category.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(category));
    }

    [HttpDelete("page-categories/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _db.PageCategories.FindAsync(id);
        if (category == null) return NotFound(ApiResponse.Fail("分類不存在"));
        _db.PageCategories.Remove(category);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok("已刪除"));
    }

    // Pages
    [HttpGet("pages")]
    public async Task<IActionResult> GetPages([FromQuery] int? categoryId)
    {
        var query = _db.Pages.AsQueryable();
        if (categoryId.HasValue) query = query.Where(p => p.CategoryId == categoryId);
        var pages = await query.OrderBy(p => p.SortOrder)
            .Select(p => new
            {
                p.Id, p.CategoryId, p.Slug, p.Title, p.Subtitle, p.Content,
                p.SortOrder, p.IsVisible, p.MetaTitle, p.MetaDescription,
                Slides = p.Slides.OrderBy(s => s.SortOrder).Select(s => new
                {
                    s.Id, s.ImageUrl, s.Title, s.Description, s.SortOrder
                }),
                Tabs = p.Tabs.OrderBy(t => t.SortOrder).Select(t => new
                {
                    t.Id, t.Title, t.Content, t.SortOrder
                })
            })
            .ToListAsync();
        return Ok(ApiResponse<object>.Ok(pages));
    }

    [HttpPost("pages")]
    public async Task<IActionResult> CreatePage([FromBody] Page page)
    {
        if (string.IsNullOrWhiteSpace(page.Title) || string.IsNullOrWhiteSpace(page.Slug))
            return BadRequest(ApiResponse.Fail("標題和 Slug 不可為空"));

        _db.Pages.Add(page);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(page));
    }

    [HttpPut("pages/{id}")]
    public async Task<IActionResult> UpdatePage(int id, [FromBody] Page input)
    {
        var page = await _db.Pages.FindAsync(id);
        if (page == null) return NotFound(ApiResponse.Fail("頁面不存在"));

        if (string.IsNullOrWhiteSpace(input.Title) || string.IsNullOrWhiteSpace(input.Slug))
            return BadRequest(ApiResponse.Fail("標題和 Slug 不可為空"));

        page.CategoryId = input.CategoryId;
        page.Slug = input.Slug;
        page.Title = input.Title;
        page.Subtitle = input.Subtitle;
        page.Content = input.Content;
        page.SortOrder = input.SortOrder;
        page.IsVisible = input.IsVisible;
        page.MetaTitle = input.MetaTitle;
        page.MetaDescription = input.MetaDescription;
        page.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(page));
    }

    [HttpDelete("pages/{id}")]
    public async Task<IActionResult> DeletePage(int id)
    {
        var page = await _db.Pages.FindAsync(id);
        if (page == null) return NotFound(ApiResponse.Fail("頁面不存在"));
        _db.Pages.Remove(page);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok("已刪除"));
    }

    [HttpPut("pages/{id}/slides")]
    public async Task<IActionResult> UpdateSlides(int id, [FromBody] List<PageSlide> slides)
    {
        if (slides == null) return BadRequest(ApiResponse.Fail("輪播資料不可為空"));

        var page = await _db.Pages.Include(p => p.Slides).FirstOrDefaultAsync(p => p.Id == id);
        if (page == null) return NotFound(ApiResponse.Fail("頁面不存在"));

        _db.PageSlides.RemoveRange(page.Slides);
        foreach (var slide in slides) { slide.PageId = id; slide.Id = 0; }
        _db.PageSlides.AddRange(slides);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(slides));
    }

    [HttpPut("pages/{id}/tabs")]
    public async Task<IActionResult> UpdateTabs(int id, [FromBody] List<PageTab> tabs)
    {
        if (tabs == null) return BadRequest(ApiResponse.Fail("Tab 資料不可為空"));

        var page = await _db.Pages.Include(p => p.Tabs).FirstOrDefaultAsync(p => p.Id == id);
        if (page == null) return NotFound(ApiResponse.Fail("頁面不存在"));

        _db.PageTabs.RemoveRange(page.Tabs);
        foreach (var tab in tabs) { tab.PageId = id; tab.Id = 0; }
        _db.PageTabs.AddRange(tabs);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(tabs));
    }
}
