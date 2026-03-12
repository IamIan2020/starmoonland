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
public class AdminNewsController : ControllerBase
{
    private readonly AppDbContext _db;
    public AdminNewsController(AppDbContext db) => _db = db;

    [HttpGet("news-categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _db.NewsCategories.OrderBy(c => c.SortOrder).ToListAsync();
        return Ok(ApiResponse<object>.Ok(categories));
    }

    [HttpGet("news")]
    public async Task<IActionResult> GetList([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] int? categoryId = null, [FromQuery] string? keyword = null)
    {
        var query = _db.News.Include(n => n.Category).AsQueryable();
        if (categoryId.HasValue) query = query.Where(n => n.CategoryId == categoryId);
        if (!string.IsNullOrEmpty(keyword)) query = query.Where(n => n.Title.Contains(keyword));

        var totalCount = await query.CountAsync();
        var items = await query.OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return Ok(new { success = true, data = items.Select(n => new { n.Id, n.CategoryId, CategoryName = n.Category.Name, n.Title, n.Summary, n.Content, n.CoverImage, n.PublishDate, n.IsPublished, n.IsPinned, n.ViewCount, n.CreatedAt }), totalCount, totalPages = (int)Math.Ceiling((double)totalCount / pageSize), currentPage = page, pageSize });
    }

    [HttpPost("news")]
    public async Task<IActionResult> Create([FromBody] News news)
    {
        if (string.IsNullOrWhiteSpace(news.Title))
            return BadRequest(ApiResponse.Fail("標題不可為空"));

        _db.News.Add(news);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(news));
    }

    [HttpPut("news/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] News input)
    {
        var news = await _db.News.FindAsync(id);
        if (news == null) return NotFound(ApiResponse.Fail("新聞不存在"));

        if (string.IsNullOrWhiteSpace(input.Title))
            return BadRequest(ApiResponse.Fail("標題不可為空"));

        news.CategoryId = input.CategoryId;
        news.Title = input.Title;
        news.Summary = input.Summary;
        news.Content = input.Content;
        news.CoverImage = input.CoverImage;
        news.PublishDate = input.PublishDate;
        news.IsPublished = input.IsPublished;
        news.IsPinned = input.IsPinned;
        news.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(news));
    }

    [HttpDelete("news/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var news = await _db.News.FindAsync(id);
        if (news == null) return NotFound(ApiResponse.Fail("新聞不存在"));
        _db.News.Remove(news);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok("已刪除"));
    }
}
