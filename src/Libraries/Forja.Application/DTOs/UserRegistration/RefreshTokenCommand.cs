namespace Forja.Application.DTOs.UserRegistration;

/// <summary>
/// Represents a command used to request a new set of authentication tokens by providing an existing refresh token.
/// </summary>
/// <remarks>
/// This command is used for token refresh operations, typically when the access token has expired and a new one is required.
/// It contains the necessary refresh token required for the backend to generate new access and refresh tokens.
/// </remarks>
public class RefreshTokenCommand
{
    /// <summary>
    /// Represents a token used to refresh an expired or invalid access token, enabling continued authentication
    /// without requiring the user to re-login.
    /// This property is intended to be sent in requests to obtain new access and refresh tokens.
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}