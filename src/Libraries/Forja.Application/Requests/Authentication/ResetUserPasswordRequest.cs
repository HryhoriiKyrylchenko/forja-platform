namespace Forja.Application.Requests.Authentication;

/// <summary>
/// Represents a request to reset a user's password.
/// </summary>
public class ResetUserPasswordRequest
{
    /// <summary>
    /// Represents a token as a string. Initialized to an empty string by default.
    /// </summary>
    /// <remarks>
    /// This property represents the token that was sent to the user's email address
    /// Token is used to verify the user's identity and to ensure that the user is the one 
    /// who requested the password reset. The token is generated by the system and sent to the user's email address.
    /// It contains a unique identifier that is used to identify the user and to verify the user's identity.
    /// </remarks>
    public string Token { get; set; } = string.Empty;

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