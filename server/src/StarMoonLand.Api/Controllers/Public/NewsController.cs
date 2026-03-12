using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarMoonLand.Api.DTOs;
using StarMoonLand.Infrastructure.Data;

namespace StarMoonLand.Api.Controllers.Public;

[ApiController]
[Route("api/news")]
public class NewsController : ControllerBase
{
    private readonly AppDbContext _db;
    public NewsController(AppDbContext db) => _db = db;

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _db.NewsCategories.OrderBy(c => c.SortOrder)
            .Select(c => new { c.Id, c.Name, c.Slug, c.SortOrder })
            .ToListAsync();
        return Ok(ApiResponse<object>.Ok(categories));
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? category = null)
    {
        var query = _db.News.Include(n => n.Category).Where(n => n.IsPublished).AsQueryable();
        if (!string.IsNullOrEmpty(category))
            query = query.Where(n => n.Category.Slug == category);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(n => n.IsPinned).ThenByDescending(n => n.PublishDate)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(n => new
            {
                n.Id, n.CategoryId, CategoryName = n.Category.Name, n.Title, n.Summary,
                n.CoverImage, n.PublishDate, n.IsPublished, n.IsPinned, n.ViewCount, n.CreatedAt
            })
            .ToListAsync();

        return Ok(new { success = true, data = items, totalCount, totalPages = (int)Math.Ceiling((double)totalCount / pageSize), currentPage = page, pageSize });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDetail(int id)
    {
        var news = await _db.News.Include(n => n.Category)
            .FirstOrDefaultAsync(n => n.Id == id && n.IsPublished);
        if (news == null) return NotFound(ApiResponse.Fail("新聞不存在"));

        news.ViewCount++;
        await _db.SaveChangesAsync();

        var result = new
        {
            news.Id, news.CategoryId, CategoryName = news.Category.Name, news.Title, news.Summary,
            news.Content, news.CoverImage, news.PublishDate, news.IsPublished, news.IsPinned, news.ViewCount, news.CreatedAt
        };
        return Ok(ApiResponse<object>.Ok(result));
    }
}
