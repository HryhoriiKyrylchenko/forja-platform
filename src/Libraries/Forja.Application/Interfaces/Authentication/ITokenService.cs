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

    /// Validates the provided password reset token to determine its validity.
    /// <param name="token">The password reset token to validate.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the token is valid.</returns>
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
}