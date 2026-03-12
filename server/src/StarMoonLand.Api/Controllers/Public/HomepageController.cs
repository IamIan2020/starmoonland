using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarMoonLand.Api.DTOs;
using StarMoonLand.Infrastructure.Data;

namespace StarMoonLand.Api.Controllers.Public;

[ApiController]
[Route("api/homepage")]
public class HomepageController : ControllerBase
{
    private readonly AppDbContext _db;
    public HomepageController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var slides = await _db.HomepageSlides.Where(s => s.IsActive).OrderBy(s => s.SortOrder)
            .Select(s => new { s.Id, s.ImageUrl, s.AltText, s.LinkUrl, s.SortOrder, s.IsActive }).ToListAsync();

        var latestNews = await _db.News.Include(n => n.Category).Where(n => n.IsPublished)
            .OrderByDescending(n => n.PublishDate).Take(3)
            .Select(n => new { n.Id, n.CategoryId, CategoryName = n.Category.Name, n.Title, n.Summary, n.CoverImage, n.PublishDate, n.IsPublished, n.IsPinned, n.ViewCount, n.CreatedAt })
            .ToListAsync();

        var featuredAlbums = await _db.Albums.Include(a => a.Category).Include(a => a.Photos)
            .Where(a => a.IsPublished).OrderBy(a => a.SortOrder).Take(6)
            .Select(a => new { a.Id, a.CategoryId, CategoryName = a.Category.Name, a.Title, a.Description, a.CoverImage, a.IsPublished, a.SortOrder, PhotoCount = a.Photos.Count, Photos = new object[0] })
            .ToListAsync();

        return Ok(ApiResponse<object>.Ok(new { slides, latestNews, featuredAlbums }));
    }
}
