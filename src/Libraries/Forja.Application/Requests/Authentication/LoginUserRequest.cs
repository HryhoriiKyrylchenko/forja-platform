namespace Forja.Application.Requests.Authentication;

/// <summary>
/// Represents a command for logging in a user by providing email and password.
/// This command is used to authenticate users and generate authentication tokens.
/// </summary>
public class LoginUserRequest
{
    /// <summary>
    /// Gets or sets the email address of the user attempting to log in.
    /// </summary>
    /// <remarks>
    /// This property is utilized as the unique identifier for user authentication
    /// and is required to perform login operations. It is expected to be in a
    /// valid email format.
    /// </remarks>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    /// <remarks>
    /// This property represents the password field required for user login or authentication.
    /// </remarks>
    public string Password { get; set; } = string.Empty;
}