namespace Forja.Application.Requests.Authentication;

/// <summary>
/// Represents a request to validate a reset token.
/// </summary>
public class ValidateResetTokenRequest
{
    /// <summary>
    /// Represents the reset token to be validated during a password reset process.
    /// </summary>
    public string Token { get; set; } = string.Empty;
}