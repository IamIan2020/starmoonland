using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using StarMoonLand.Core.Interfaces;

namespace StarMoonLand.Core.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration config, ILogger<EmailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        var smtpUser = _config["Email:SmtpUser"];
        if (string.IsNullOrEmpty(smtpUser))
        {
            _logger.LogWarning("SMTP 未設定，跳過 Email 發送: {Subject} → {To}", subject, to);
            return;
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(
            _config["Email:FromName"] ?? "星月大地",
            _config["Email:FromEmail"] ?? smtpUser
        ));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = htmlBody };

        using var client = new SmtpClient();
        await client.ConnectAsync(
            _config["Email:SmtpHost"] ?? "smtp.gmail.com",
            _config.GetValue<int>("Email:SmtpPort", 587),
            MailKit.Security.SecureSocketOptions.StartTls
        );
        await client.AuthenticateAsync(smtpUser, _config["Email:SmtpPassword"]);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);

        _logger.LogInformation("Email 已發送: {Subject} → {To}", subject, to);
    }
}
