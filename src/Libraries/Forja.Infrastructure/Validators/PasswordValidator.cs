namespace Forja.Infrastructure.Validators;

public static class PasswordValidator
{
    /// <summary>
    /// Validates if the provided password meets security requirements.
    /// </summary>
    /// <param name="password">The password to validate.</param>
    /// <returns>A tuple containing a boolean indicating validity and an error message if invalid.</returns>
    public static (bool IsValid, string ErrorMessage) ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return (false, "Password cannot be empty or whitespace.");
        }

        if (password.Length < 8)
        {
            return (false, "Password must be at least 8 characters long.");
        }

        if (!password.Any(char.IsUpper))
        {
            return (false, "Password must contain at least one uppercase letter.");
        }

        if (!password.Any(char.IsLower))
        {
            return (false, "Password must contain at least one lowercase letter.");
        }

        if (!password.Any(char.IsDigit))
        {
            return (false, "Password must contain at least one digit.");
        }

        if (!password.Any(ch => "!@#$%^&*()-_=+[]{}|;:',.<>?/".Contains(ch)))
        {
            return (false, "Password must contain at least one special character.");
        }

        return (true, string.Empty);
    }
}