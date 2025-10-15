using SolarLab.EBoard.Notifications.Domain.Entities;
using SolarLab.EBoard.Notifications.Domain.ValueObjects;

namespace SolarLab.EBoard.Notifications.UnitTests.Domain;

public class NotificationTests
{
    [Theory]
    [InlineData("test1@mail.com", "Weather for November 1st", "Foggy")]
    [InlineData("johndoe@gmail.com", "Speeding ticket", "You must pay $100 for speeding")]
    [InlineData("testmail@yahoo.com", "Project analysis", "You have some bugs you should fix")]
    [InlineData("martin10@mail.fr", "Bakery discounts", "Hurry up and get 20% off")]
    public void CreateNotification_CreatesNotification(string to, string subject, string content)
    {
        // Arrange
        var emailAddresses = new List<EmailAddress>
        {
            new(to)
        };
        
        // Act
        var result = Notification.Create(emailAddresses, subject, content);
        
        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(emailAddresses, result.To);
        Assert.Equal(subject, result.Subject);
        Assert.Equal(content, result.Content);
    }

    [Fact]
    public void CreateNotification_WithZeroRecipientEmails_Throws()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => Notification.Create(
            [],
            TestConstants.TestNotificationSubject,
            TestConstants.TestNotificationContent));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\n")]
    [InlineData(" \n\r  \t")]
    public void CreateNotification_WithNullOrWhitespaceSubject_Throws(string subject)
    {
        // Arrange
        var emailAddresses = new List<EmailAddress>
        {
            new(TestConstants.TestNotificationTo)
        };
        
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => Notification.Create(
            emailAddresses,
            subject,
            TestConstants.TestNotificationContent));
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\n")]
    [InlineData(" \n\r  \t")]
    public void CreateNotification_WithNullOrWhitespaceContent_Throws(string content)
    {
        // Arrange
        var emailAddresses = new List<EmailAddress>
        {
            new(TestConstants.TestNotificationTo)
        };
        
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => Notification.Create(
            emailAddresses,
            TestConstants.TestNotificationSubject,
            content));
    }
}