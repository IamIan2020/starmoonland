using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarMoonLand.Api.DTOs;
using StarMoonLand.Infrastructure.Data;

namespace StarMoonLand.Api.Controllers.Public;

[ApiController]
[Route("api/site-settings")]
public class SiteSettingsController : ControllerBase
{
    private readonly AppDbContext _db;
    public SiteSettingsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var settings = await _db.SiteSettings
            .Where(s => s.Key != "admin_notification_email")
            .Select(s => new { s.Id, s.Key, s.Value, s.Description }).ToListAsync();
        return Ok(ApiResponse<object>.Ok(settings));
    }
}
