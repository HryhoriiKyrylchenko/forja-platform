namespace Forja.Application.Interfaces.Authentication;

/// <summary>
/// Defines methods for generating and validating tokens related to user authentication and management.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a password reset token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the password reset token will be generated.</param>
    /// <returns>Returns a string representing the generated password reset token.</returns>
    string GeneratePasswordResetToken(User user);
    
    /// <summary>
    /// Generates an email confirmation token for the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user for whom the email confirmation token will be generated.</param>
    /// <param name="email">The email address of the user for whom the email confirmation token will be generated.</param>
    /// <returns>Returns a string representing the generated email confirmation token.</returns>
    string GenerateEmailConfirmationToken(Guid userId, string email);

    /// <summary>
    /// Validates the provided password reset token and extracts the Keycloak user ID if valid.
    /// </summary>
    /// <param name="token">The JWT password reset token to validate.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the Keycloak user ID if the token is valid,
    /// or <c>null</c> if the token is invalid or expired.
    /// </returns>
    Task<string?> GetkeycloakUserIdFromToken(string token);

    /// <summary>
    /// Validates the provided JWT password reset token to ensure it is well-formed, signed correctly, and not expired.
    /// </summary>
    /// <param name="token">The JWT token to validate.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is <c>true</c> if the token is valid and contains claims;
    /// otherwise, <c>false</c>.
    /// </returns>
    Task<bool> ValidatePasswordResetToken(string token);

    /// <summary>
    /// Validates the provided email confirmation token for authenticity and purpose.
    /// </summary>
    /// <param name="token">The email confirmation token to validate.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains
    /// a boolean indicating whether the email confirmation token is valid.
    /// </returns>
    Task<bool> ValidateEmailConfirmationToken(string token);

    /// <summary>
    /// Retrieves the email associated with the provided email confirmation token.
    /// </summary>
    /// <param name="token">The email confirmation token from which the email will be extracted.</param>
    /// <returns>Returns a string representing the email if the token is valid; otherwise, returns null.</returns>
    Task<string?> GetEmailFromEmailConfirmationToken(string token);

    /// <summary>
    /// Extracts the user ID from the specified token.
    /// </summary>
    /// <param name="token">The token from which the user ID will be extracted.</param>
    /// <returns>Returns a nullable GUID representing the extracted user ID, or null if the token is invalid.</returns>
    string GetUserIdFromToken(string token);
}