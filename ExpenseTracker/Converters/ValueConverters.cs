using System.Globalization;

namespace ExpenseTracker.Converters;

/// <summary>
/// Converts boolean values to colors (Blue for true, Gray for false)
/// </summary>
public class BoolToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? Colors.Blue : Colors.Gray;
        }
        return Colors.Gray;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts any value to a boolean indicating if it's not null
/// </summary>
public class IsNotNullConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value != null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts invoice status to boolean based on parameter
/// </summary>
public class StatusToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Models.InvoiceStatus status && parameter is string param)
        {
            return param switch
            {
                "NotPaid" => status != Models.InvoiceStatus.Paid,
                _ => false
            };
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts achievement unlock status to icon (checkmark for unlocked, lock for locked)
/// </summary>
public class AchievementIconConverter : IValueConverter
{
    public static readonly AchievementIconConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isUnlocked)
        {
            return isUnlocked ? "✓" : "🔒";
        }
        return "🔒";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
