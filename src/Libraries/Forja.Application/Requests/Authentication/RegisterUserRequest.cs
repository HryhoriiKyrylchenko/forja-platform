namespace Forja.Application.Requests.Authentication;

/// <summary>
/// Represents a command for registering a new user in the application.
/// </summary>
/// <remarks>
/// This command encapsulates the information required for user registration,
/// including the user's email and password. It is used as input data for
/// user registration processes in the application.
/// </remarks>
public class RegisterUserRequest
{
    /// <summary>
    /// Represents the email address of a user in the registration process.
    /// </summary>
    /// <remarks>
    /// The email property is required for creating a new user and is utilized across multiple layers,
    /// including external service integrations (e.g., Keycloak) and user storage within the application domain.
    /// </remarks>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password associated with the user.
    /// This property is used for creating a new user and is required for authentication purposes.
    /// It must be handled securely and should comply with security standards for sensitive data.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}