using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarMoonLand.Api.DTOs;
using StarMoonLand.Core.Entities;
using StarMoonLand.Core.Interfaces;
using StarMoonLand.Infrastructure.Data;

namespace StarMoonLand.Api.Controllers.Public;

[ApiController]
[Route("api/contact")]
public class ContactController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IEmailService _emailService;

    public ContactController(AppDbContext db, IEmailService emailService)
    {
        _db = db;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] ContactSubmitRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Phone) ||
            string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Message))
            return BadRequest(ApiResponse.Fail("請填寫所有必填欄位"));

        var submission = new ContactSubmission
        {
            Name = request.Name,
            Phone = request.Phone,
            Email = request.Email,
            Message = request.Message,
        };

        _db.ContactSubmissions.Add(submission);
        await _db.SaveChangesAsync();

        // 通知管理者
        var adminEmail = await _db.SiteSettings.FirstOrDefaultAsync(s => s.Key == "admin_notification_email");
        if (adminEmail?.Value != null)
        {
            try
            {
                await _emailService.SendEmailAsync(
                    adminEmail.Value,
                    $"[星月大地] 新的聯絡表單 - {request.Name}",
                    $"<h3>新的聯絡表單</h3><p><strong>姓名：</strong>{request.Name}</p><p><strong>電話：</strong>{request.Phone}</p><p><strong>Email：</strong>{request.Email}</p><p><strong>詢問事項：</strong></p><p>{request.Message}</p>"
                );
            }
            catch { /* 發信失敗不影響表單送出 */ }
        }

        return Ok(ApiResponse.Ok("感謝您的留言，我們會盡快回覆"));
    }

    public class ContactSubmitRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
