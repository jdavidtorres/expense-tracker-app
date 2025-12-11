namespace ExpenseTracker.Helpers;

/// <summary>
/// Helper class for common validation operations
/// </summary>
public static class ValidationHelper
{
    /// <summary>
    /// Validates that a string is not null or whitespace
    /// </summary>
    /// <param name="value">The string to validate</param>
    /// <param name="fieldName">The name of the field being validated</param>
    /// <param name="errorMessage">Output parameter for the error message</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool ValidateRequiredString(string? value, string fieldName, out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            errorMessage = $"{fieldName} is required.";
            return false;
        }

        errorMessage = null;
        return true;
    }

    /// <summary>
    /// Validates that a decimal value is greater than zero
    /// </summary>
    /// <param name="value">The decimal to validate</param>
    /// <param name="fieldName">The name of the field being validated</param>
    /// <param name="errorMessage">Output parameter for the error message</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool ValidatePositiveAmount(decimal value, string fieldName, out string? errorMessage)
    {
        if (value <= 0)
        {
            errorMessage = $"{fieldName} must be greater than 0.";
            return false;
        }

        errorMessage = null;
        return true;
    }

    /// <summary>
    /// Validates that a month is within the valid range (1-12)
    /// </summary>
    /// <param name="month">The month to validate</param>
    /// <param name="errorMessage">Output parameter for the error message</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool ValidateMonth(int month, out string? errorMessage)
    {
        if (month < 1 || month > 12)
        {
            errorMessage = "Month must be between 1 and 12.";
            return false;
        }

        errorMessage = null;
        return true;
    }

    /// <summary>
    /// Validates an email address format using a more robust pattern
    /// </summary>
    /// <param name="email">The email to validate</param>
    /// <param name="errorMessage">Output parameter for the error message</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool ValidateEmail(string? email, out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            errorMessage = "Email is required.";
            return false;
        }

        // More robust email validation pattern
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern))
        {
            errorMessage = "Please enter a valid email address.";
            return false;
        }

        errorMessage = null;
        return true;
    }
}
