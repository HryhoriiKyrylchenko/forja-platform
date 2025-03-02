namespace Forja.API.DTOs;

/// <summary>
/// Represents a request to reset a user's password.
/// </summary>
public class ResetUserPasswordRequest
{
    /// <summary>
    /// Represents the unique identifier of a user in Keycloak.
    /// This property is used to specify the user whose password will be reset.
    /// </summary>
    public string KeycloakUserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the new password for the user during a password reset request.
    /// </summary>
    /// <remarks>
    /// This property represents the new password to be set for the user.
    /// The value provided must comply with the system's password complexity requirements.
    /// </remarks>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the new password is temporary.
    /// If set to true, the password will need to be changed by the user
    /// upon their next login. If false, the password is permanent and
    /// will not require the user to update it.
    /// </summary>
    public bool Temporary { get; set; }
}