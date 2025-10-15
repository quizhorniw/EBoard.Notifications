using Moq;
using SolarLab.EBoard.Notifications.Application.Abstractions.Email;
using SolarLab.EBoard.Notifications.Application.CQRS.Notifications.Send;
using SolarLab.EBoard.Notifications.Domain.Entities;
using SolarLab.EBoard.Notifications.Domain.ValueObjects;

namespace SolarLab.EBoard.Notifications.UnitTests.Application.Notifications;

public class SendNotificationCommandTests
{
    private readonly Mock<IEmailSender> _emailSenderMock;
    private readonly SendNotificationHandler _handler;
    
    public SendNotificationCommandTests()
    {
        _emailSenderMock = new Mock<IEmailSender>();

        _handler = new SendNotificationHandler(_emailSenderMock.Object);
    }
    
    [Theory]
    [InlineData("test1@mail.com", "Weather for November 1st", "Foggy")]
    [InlineData("johndoe@gmail.com", "Speeding ticket", "You must pay $100 for speeding")]
    [InlineData("testmail@yahoo.com", "Project analysis", "You have some bugs you should fix")]
    [InlineData("martin10@mail.fr", "Bakery discounts", "Hurry up and get 20% off")]
    public async Task SendNotification_SendsNotification(string to, string subject, string content)
    {
        // Arrange
        var request = new SendNotificationCommand([ to ], subject, content);

        Notification? capturedNotification = null;
        _emailSenderMock
            .Setup(s => s.SendAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
            .Callback<Notification, CancellationToken>((n, _) => capturedNotification = n)
            .Returns(Task.CompletedTask);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.NotNull(capturedNotification);
        Assert.Equal(1, capturedNotification.To.Count);
        Assert.Contains(new EmailAddress(to), capturedNotification.To);
        Assert.Equal(subject, capturedNotification.Subject);
        Assert.Equal(content, capturedNotification.Content);
    }
}