namespace Forja.API.DTOs;

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
    /// Gets or sets the redirect URL to be used for password recovery purposes.
    /// This property allows specifying a custom URL for the recovery process.
    /// </summary>
    public string? RedirectUrl { get; set; }
}