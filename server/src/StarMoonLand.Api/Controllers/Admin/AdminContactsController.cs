using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarMoonLand.Api.DTOs;
using StarMoonLand.Core.Interfaces;
using StarMoonLand.Infrastructure.Data;

namespace StarMoonLand.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/contacts")]
[Authorize]
public class AdminContactsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IEmailService _emailService;
    private readonly ILogger<AdminContactsController> _logger;

    public AdminContactsController(AppDbContext db, IEmailService emailService, ILogger<AdminContactsController> logger)
    {
        _db = db;
        _emailService = emailService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] bool? isRead = null)
    {
        var query = _db.ContactSubmissions.AsQueryable();
        if (isRead.HasValue) query = query.Where(c => c.IsRead == isRead);

        var totalCount = await query.CountAsync();
        var items = await query.OrderBy(c => c.IsRead).ThenByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return Ok(new { success = true, data = items, totalCount, totalPages = (int)Math.Ceiling((double)totalCount / pageSize), currentPage = page, pageSize });
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkRead(int id)
    {
        var contact = await _db.ContactSubmissions.FindAsync(id);
        if (contact == null) return NotFound(ApiResponse.Fail("表單不存在"));
        contact.IsRead = true;
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok("已標記為已讀"));
    }

    [HttpPut("{id}/reply")]
    public async Task<IActionResult> Reply(int id, [FromBody] ReplyRequest request)
    {
        var contact = await _db.ContactSubmissions.FindAsync(id);
        if (contact == null) return NotFound(ApiResponse.Fail("表單不存在"));

        contact.ReplyContent = request.ReplyContent;
        contact.ReplyNote = request.ReplyNote;
        contact.RepliedAt = DateTime.UtcNow;
        contact.IsRead = true;
        await _db.SaveChangesAsync();

        // 發送回覆 Email 給訪客
        try
        {
            await _emailService.SendEmailAsync(
                contact.Email,
                "[星月大地] 您的詢問已回覆",
                $"<h3>親愛的 {contact.Name}，您好</h3><p>感謝您的來信，以下是我們的回覆：</p><hr/><p>{request.ReplyContent}</p><hr/><p>您的原始詢問：</p><p>{contact.Message}</p><br/><p>星月大地景觀休閒園區</p>"
            );
        }
        catch (Exception ex) { _logger.LogWarning(ex, "回覆通知信發送失敗"); }

        return Ok(ApiResponse.Ok("已回覆"));
    }

    public class ReplyRequest
    {
        public string ReplyContent { get; set; } = string.Empty;
        public string? ReplyNote { get; set; }
    }
}
