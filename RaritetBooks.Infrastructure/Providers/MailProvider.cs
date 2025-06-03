using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using RaritetBooks.Application.Features.Notifications;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.ValueObjects;
using RaritetBooks.Infrastructure.Options;

namespace RaritetBooks.Infrastructure.Providers;

public class MailProvider : IMailProvider
{
    private readonly MailOptions _mailOptions;
    private readonly ILogger<MailProvider> _logger;

    public MailProvider(
        IOptions<MailOptions> mailOptions,
        ILogger<MailProvider> logger)
    {
        _mailOptions = mailOptions.Value;
        _logger = logger;
    }
    
    public async Task SendMessage(EmailNotification emailNotification)
    {
        try
        {
            var mail = InitializeMail(
                emailNotification.Subject,
                emailNotification.Message,
                emailNotification.Email);

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_mailOptions.Host, _mailOptions.Port, _mailOptions.UseSSL);
                await client.AuthenticateAsync(_mailOptions.UserName, _mailOptions.Password);
                await client.SendAsync(mail);
            
                _logger.LogInformation("Email sent to {email}", emailNotification.Email.Value);

                await client.DisconnectAsync(true);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error when sent an email {e}", e.Message);
        }
    }

    private MimeMessage InitializeMail(string subject, string message, Email email)
    {
        var mail = new MimeMessage();
        
        mail.From.Add(new MailboxAddress(_mailOptions.DisplayName, _mailOptions.From));
        mail.To.Add(new MailboxAddress("", email.Value));
        mail.Subject = subject;
        mail.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

        return mail;
    }
}