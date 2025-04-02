namespace Forja.Application.Requests.Authentication;

/// <summary>
/// Represents a request to initiate the forgot password process.
/// </summary>
/// <remarks>
/// This DTO contains the required information for requesting a password reset.
/// </remarks>
public class ForgotPasswordRequest
{
    /// <summary>
    /// Gets or sets the email address associated with the forgot password request.
    /// </summary>
    /// <remarks>
    /// This property contains the email address of the user requesting a password reset.
    /// It is a required field and is set to an empty string by default.
    /// </remarks>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the locale (language code) used for localization purposes.
    /// Defaults to "en" if not explicitly provided.
    /// </summary>
    /// <remarks>
    /// This property is typically used to determine which language or culture should be applied
    /// when generating content such as email messages or UI links (e.g., "en", "uk").
    /// </remarks>
    public string? Locale { get; set; } = string.Empty;

}