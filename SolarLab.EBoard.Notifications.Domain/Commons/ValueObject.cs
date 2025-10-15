namespace SolarLab.EBoard.Notifications.Domain.Commons;

public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        using var thisValues = GetEqualityComponents().GetEnumerator();
        using var otherValues = other.GetEqualityComponents().GetEnumerator();

        while (thisValues.MoveNext() && otherValues.MoveNext())
        {
            if (thisValues.Current is null ^ otherValues.Current is null)
            {
                return false;
            }

            if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
            {
                return false;
            }
        }

        return !thisValues.MoveNext() && !otherValues.MoveNext();
    }

    public override int GetHashCode()
    {
        unchecked 
        {
            return GetEqualityComponents()
                .Aggregate(17, (current, obj) => current * 23 + (obj?.GetHashCode() ?? 0));
        }
    }

    public static bool operator ==(ValueObject? obj1, ValueObject? obj2)
    {
        return obj1?.Equals(obj2) ?? obj2 is null;
    }

    public static bool operator !=(ValueObject obj1, ValueObject obj2)
    {
        return !(obj1 == obj2);
    }
}