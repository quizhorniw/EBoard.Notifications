using System.Text.RegularExpressions;
using SolarLab.EBoard.Notifications.Domain.Commons;

namespace SolarLab.EBoard.Notifications.Domain.ValueObjects;

public sealed class EmailAddress : ValueObject
{
    private const string EmailRegexPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    public string Value { get; private set; }

    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required");
        }

        if (!Regex.IsMatch(value, EmailRegexPattern, RegexOptions.IgnoreCase))
        {
            throw new ArgumentException("Value must be valid email address");
        }
        
        Value = value;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}