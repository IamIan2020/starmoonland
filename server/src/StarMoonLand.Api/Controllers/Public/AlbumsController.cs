using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarMoonLand.Api.DTOs;
using StarMoonLand.Infrastructure.Data;

namespace StarMoonLand.Api.Controllers.Public;

[ApiController]
[Route("api/albums")]
public class AlbumsController : ControllerBase
{
    private readonly AppDbContext _db;
    public AlbumsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] string? category = null)
    {
        var query = _db.Albums.Include(a => a.Category).Include(a => a.Photos)
            .Where(a => a.IsPublished).AsQueryable();
        if (!string.IsNullOrEmpty(category))
            query = query.Where(a => a.Category.Slug == category);

        var items = await query.OrderBy(a => a.SortOrder)
            .Select(a => new
            {
                a.Id, a.CategoryId, CategoryName = a.Category.Name, a.Title, a.Description,
                a.CoverImage, a.IsPublished, a.SortOrder, PhotoCount = a.Photos.Count,
                Photos = new object[0]
            })
            .ToListAsync();

        return Ok(ApiResponse<object>.Ok(items));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDetail(int id)
    {
        var album = await _db.Albums.Include(a => a.Category)
            .Include(a => a.Photos.OrderBy(p => p.SortOrder))
            .FirstOrDefaultAsync(a => a.Id == id && a.IsPublished);
        if (album == null) return NotFound(ApiResponse.Fail("相簿不存在"));

        var result = new
        {
            album.Id, album.CategoryId, CategoryName = album.Category.Name, album.Title, album.Description,
            album.CoverImage, album.IsPublished, album.SortOrder, PhotoCount = album.Photos.Count,
            Photos = album.Photos.Select(p => new { p.Id, p.ImageUrl, p.ThumbnailUrl, p.Caption, p.SortOrder })
        };
        return Ok(ApiResponse<object>.Ok(result));
    }
}
