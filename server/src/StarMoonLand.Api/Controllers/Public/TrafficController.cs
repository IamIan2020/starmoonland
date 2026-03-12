using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarMoonLand.Api.DTOs;
using StarMoonLand.Infrastructure.Data;

namespace StarMoonLand.Api.Controllers.Public;

[ApiController]
[Route("api/traffic")]
public class TrafficController : ControllerBase
{
    private readonly AppDbContext _db;
    public TrafficController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var items = await _db.TrafficInfos.OrderBy(t => t.SortOrder)
            .Select(t => new { t.Id, t.TabName, t.Content, t.SortOrder }).ToListAsync();
        return Ok(ApiResponse<object>.Ok(items));
    }
}
