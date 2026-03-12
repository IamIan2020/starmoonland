using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using StarMoonLand.Api.DTOs;
using StarMoonLand.Core.Entities;
using StarMoonLand.Infrastructure.Data;

namespace StarMoonLand.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/media")]
[Authorize]
public class AdminMediaController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<AdminMediaController> _logger;

    public AdminMediaController(AppDbContext db, IConfiguration config, IWebHostEnvironment env, ILogger<AdminMediaController> logger)
    {
        _db = db;
        _config = config;
        _env = env;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var files = await _db.MediaFiles.OrderByDescending(f => f.CreatedAt).ToListAsync();
        return Ok(ApiResponse<object>.Ok(files));
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(ApiResponse.Fail("請選擇檔案"));

        var maxSize = _config.GetValue<long>("Upload:MaxFileSize", 10485760);
        if (file.Length > maxSize)
            return BadRequest(ApiResponse.Fail("檔案大小不得超過 10MB"));

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var allowedExts = _config.GetSection("Upload:AllowedFileExtensions").Get<string[]>()
            ?? [".jpg", ".jpeg", ".png", ".gif", ".webp", ".pdf"];
        if (!allowedExts.Contains(ext))
            return BadRequest(ApiResponse.Fail("不支援的檔案格式"));

        var uploadPath = Path.Combine(_env.ContentRootPath, _config["Upload:UploadPath"] ?? "uploads");
        var dateDir = DateTime.UtcNow.ToString("yyyy/MM");
        var fullDir = Path.Combine(uploadPath, dateDir);
        Directory.CreateDirectory(fullDir);

        var guid = Guid.NewGuid().ToString("N");
        var filename = $"{guid}{ext}";
        var filePath = Path.Combine(fullDir, filename);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // 產生縮圖
        string? thumbnailPath = null;
        var imageExts = _config.GetSection("Upload:AllowedImageExtensions").Get<string[]>()
            ?? [".jpg", ".jpeg", ".png", ".gif", ".webp"];
        if (imageExts.Contains(ext))
        {
            try
            {
                var thumbFilename = $"{guid}_thumb{ext}";
                var thumbPath = Path.Combine(fullDir, thumbFilename);
                var maxWidth = _config.GetValue<int>("Upload:ThumbnailMaxWidth", 300);

                using var image = await Image.LoadAsync(filePath);
                if (image.Width > maxWidth)
                {
                    image.Mutate(x => x.Resize(maxWidth, 0));
                }
                await image.SaveAsync(thumbPath);
                thumbnailPath = $"{dateDir}/{thumbFilename}";
            }
            catch (Exception ex) { _logger.LogWarning(ex, "縮圖產生失敗: {FileName}", file.FileName); }
        }

        var mediaFile = new MediaFile
        {
            Filename = filename,
            OriginalName = file.FileName,
            ContentType = file.ContentType,
            FileSize = file.Length,
            FilePath = $"{dateDir}/{filename}",
            ThumbnailPath = thumbnailPath,
        };

        _db.MediaFiles.Add(mediaFile);
        await _db.SaveChangesAsync();

        return Ok(ApiResponse<object>.Ok(mediaFile));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var file = await _db.MediaFiles.FindAsync(id);
        if (file == null) return NotFound(ApiResponse.Fail("檔案不存在"));

        var uploadPath = Path.Combine(_env.ContentRootPath, _config["Upload:UploadPath"] ?? "uploads");
        var fullPath = Path.Combine(uploadPath, file.FilePath);
        if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);

        if (file.ThumbnailPath != null)
        {
            var thumbPath = Path.Combine(uploadPath, file.ThumbnailPath);
            if (System.IO.File.Exists(thumbPath)) System.IO.File.Delete(thumbPath);
        }

        _db.MediaFiles.Remove(file);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok("已刪除"));
    }
}
