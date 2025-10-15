namespace SolarLab.EBoard.Notifications.Infrastructure.Email;

public record SmtpSettings(
    string Host,
    int Port,
    bool UseSsl,
    string FromName,
    string FromAddress,
    string Username,
    string Password);