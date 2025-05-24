namespace Forja.Infrastructure.Validators;

public static class EmailValidator
{
    /// <summary>
    /// Validates if the provided email address is valid.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns>True if the email is valid; otherwise, false.</returns>
    public static bool ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }
        
        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        return emailRegex.IsMatch(email);
    }
}
