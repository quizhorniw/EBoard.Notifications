using SolarLab.EBoard.Notifications.Domain.Commons;
using SolarLab.EBoard.Notifications.Domain.ValueObjects;

namespace SolarLab.EBoard.Notifications.Domain.Entities;

public sealed class Notification : Entity
{
    public Guid Id { get; private set; }
    public List<EmailAddress> To { get; private set; }
    public string Subject { get; private set; }
    public string Content { get; private set; }

    private Notification(List<EmailAddress> to, string subject, string content)
    {
        if (to.Count == 0)
        {
            throw new ArgumentException("At least one recipient is required");
        }

        if (string.IsNullOrWhiteSpace(subject))
        {
            throw new ArgumentException("Subject is required");
        }
        
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content is required");
        }
        
        Id = Guid.NewGuid();
        To = to;
        Subject = subject;
        Content = content;
    }

    public static Notification Create(List<EmailAddress> to, string subject, string content) => 
        new(to, subject, content);
}