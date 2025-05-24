namespace Forja.Application.Requests.Authentication;

/// <summary>
/// Represents a command to log out a user by invalidating an associated refresh token.
/// </summary>
public class LogoutRequest
{
    /// <summary>
    /// Refresh token used for re-authenticating a user or logging a user out by invalidating their session.
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}