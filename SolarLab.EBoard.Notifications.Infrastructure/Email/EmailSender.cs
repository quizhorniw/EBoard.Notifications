using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SolarLab.EBoard.Notifications.Application.Abstractions.Email;
using SolarLab.EBoard.Notifications.Domain.Entities;

namespace SolarLab.EBoard.Notifications.Infrastructure.Email;

public class EmailSender : IEmailSender
{
    private readonly ILogger<EmailSender> _logger;
    private readonly SmtpSettings _smtpSettings;

    public EmailSender(IOptions<SmtpSettings> smtpSettings, ILogger<EmailSender> logger)
    {
        _logger = logger;
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        var message = new MimeMessage();
        
        message.From.Add(new MailboxAddress(_smtpSettings.FromName, _smtpSettings.FromAddress));
        notification.To.ForEach(to => message.To.Add(new MailboxAddress(to.Value, to.Value)));
        message.Subject = notification.Subject;
        
        message.Body = new TextPart(notification.IsHtml ? "html" : "plain")
        {
            ContentTransferEncoding = ContentEncoding.QuotedPrintable,
            Text = notification.Content
        };
        
        try
        {
            await SendThroughSmtp(message, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email, notificationId: {0}", notification.Id);
            throw new ApplicationException("Failed to send email", ex);
        }
    }

    private async Task SendThroughSmtp(MimeMessage message, CancellationToken cancellationToken)
    {
        using var smtpClient = new SmtpClient();
        
        await smtpClient.ConnectAsync(
            _smtpSettings.Host,
            _smtpSettings.Port,
            _smtpSettings.UseSsl,
            cancellationToken);
            
        await smtpClient.AuthenticateAsync(
            _smtpSettings.Username,
            _smtpSettings.Password,
            cancellationToken);
            
        await smtpClient.SendAsync(message, cancellationToken);
            
        await smtpClient.DisconnectAsync(true, cancellationToken);
    }
}