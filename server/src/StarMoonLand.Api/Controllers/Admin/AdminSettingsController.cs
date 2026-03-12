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
public class AdminSettingsController : ControllerBase
{
    private readonly AppDbContext _db;
    public AdminSettingsController(AppDbContext db) => _db = db;

    [HttpPut("site-settings")]
    public async Task<IActionResult> UpdateSettings([FromBody] UpdateSettingsRequest request)
    {
        if (request?.Settings == null) return BadRequest(ApiResponse.Fail("設定資料不可為空"));

        foreach (var item in request.Settings)
        {
            var setting = await _db.SiteSettings.FirstOrDefaultAsync(s => s.Key == item.Key);
            if (setting != null)
            {
                setting.Value = item.Value;
                setting.UpdatedAt = DateTime.UtcNow;
            }
        }
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok("設定已更新"));
    }

    [HttpGet("homepage/slides")]
    public async Task<IActionResult> GetSlides()
    {
        var slides = await _db.HomepageSlides.OrderBy(s => s.SortOrder).ToListAsync();
        return Ok(ApiResponse<object>.Ok(slides));
    }

    [HttpPut("homepage/slides")]
    public async Task<IActionResult> UpdateSlides([FromBody] List<HomepageSlide> slides)
    {
        if (slides == null) return BadRequest(ApiResponse.Fail("輪播資料不可為空"));

        var existing = await _db.HomepageSlides.ToListAsync();
        _db.HomepageSlides.RemoveRange(existing);
        foreach (var s in slides) s.Id = 0;
        _db.HomepageSlides.AddRange(slides);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok("已更新首頁輪播"));
    }

    [HttpGet("traffic")]
    public async Task<IActionResult> GetTraffic()
    {
        var items = await _db.TrafficInfos.OrderBy(t => t.SortOrder).ToListAsync();
        return Ok(ApiResponse<object>.Ok(items));
    }

    [HttpPut("traffic")]
    public async Task<IActionResult> UpdateTraffic([FromBody] List<TrafficInfo> items)
    {
        if (items == null) return BadRequest(ApiResponse.Fail("交通資訊不可為空"));

        var existing = await _db.TrafficInfos.ToListAsync();
        _db.TrafficInfos.RemoveRange(existing);
        foreach (var t in items) t.Id = 0;
        _db.TrafficInfos.AddRange(items);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok("已更新交通資訊"));
    }

    public class UpdateSettingsRequest
    {
        public List<SettingItem> Settings { get; set; } = [];
    }

    public class SettingItem
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
