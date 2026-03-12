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
public class AdminAlbumsController : ControllerBase
{
    private readonly AppDbContext _db;
    public AdminAlbumsController(AppDbContext db) => _db = db;

    [HttpGet("album-categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _db.AlbumCategories.OrderBy(c => c.SortOrder).ToListAsync();
        return Ok(ApiResponse<object>.Ok(categories));
    }

    [HttpGet("albums")]
    public async Task<IActionResult> GetList([FromQuery] int? categoryId = null)
    {
        var query = _db.Albums.Include(a => a.Category).Include(a => a.Photos).AsQueryable();
        if (categoryId.HasValue) query = query.Where(a => a.CategoryId == categoryId);
        var albums = await query.OrderBy(a => a.SortOrder).ToListAsync();
        return Ok(ApiResponse<object>.Ok(albums.Select(a => new { a.Id, a.CategoryId, CategoryName = a.Category.Name, a.Title, a.Description, a.CoverImage, a.IsPublished, a.SortOrder, PhotoCount = a.Photos.Count, Photos = a.Photos.OrderBy(p => p.SortOrder).Select(p => new { p.Id, p.ImageUrl, p.ThumbnailUrl, p.Caption, p.SortOrder }) })));
    }

    [HttpPost("albums")]
    public async Task<IActionResult> Create([FromBody] Album album)
    {
        _db.Albums.Add(album);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(album));
    }

    [HttpPut("albums/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Album input)
    {
        var album = await _db.Albums.FindAsync(id);
        if (album == null) return NotFound(ApiResponse.Fail("相簿不存在"));

        album.CategoryId = input.CategoryId;
        album.Title = input.Title;
        album.Description = input.Description;
        album.CoverImage = input.CoverImage;
        album.IsPublished = input.IsPublished;
        album.SortOrder = input.SortOrder;
        album.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(album));
    }

    [HttpDelete("albums/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var album = await _db.Albums.FindAsync(id);
        if (album == null) return NotFound(ApiResponse.Fail("相簿不存在"));
        _db.Albums.Remove(album);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok("已刪除"));
    }

    [HttpPut("albums/{id}/photos")]
    public async Task<IActionResult> UpdatePhotos(int id, [FromBody] List<AlbumPhoto> photos)
    {
        var album = await _db.Albums.Include(a => a.Photos).FirstOrDefaultAsync(a => a.Id == id);
        if (album == null) return NotFound(ApiResponse.Fail("相簿不存在"));

        _db.AlbumPhotos.RemoveRange(album.Photos);
        foreach (var photo in photos) { photo.AlbumId = id; photo.Id = 0; }
        _db.AlbumPhotos.AddRange(photos);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(photos));
    }
}
