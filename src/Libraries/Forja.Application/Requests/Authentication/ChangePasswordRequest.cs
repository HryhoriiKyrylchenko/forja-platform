namespace Forja.Application.Requests.Authentication;

/// <summary>
/// Represents a request to change a user's password in the authentication system.
/// </summary>
/// <remarks>
/// The request includes the new password, which must meet the specified minimum length requirement.
/// </remarks>
public class ChangePasswordRequest
{
    /// <summary>
    /// Gets or sets the password required for the change password request.
    /// The password must have a minimum length of 8 characters.
    /// </summary>
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;
}