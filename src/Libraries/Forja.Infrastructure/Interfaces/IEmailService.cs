namespace Forja.Infrastructure.Interfaces;

/// <summary>
/// Defines methods for sending various types of emails such as password reset
/// and email confirmation.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends a password reset email to the specified user with a provided reset link.
    /// </summary>
    /// <param name="email">The recipient's email address.</param>
    /// <param name="resetLink">The URL link to reset the password.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendPasswordResetEmailAsync(string email, string resetLink);

    /// <summary>
    /// Sends an email confirmation to a user with a given email, username, and confirmation link.
    /// </summary>
    /// <param name="email">The email address of the recipient.</param>
    /// <param name="username">The name of the user to personalize the email content.</param>
    /// <param name="confirmationLink">The link the user needs to click to confirm their email address.</param>
    /// <returns>A Task representing the asynchronous operation to send the email.</returns>
    Task SendEmailConfirmationAsync(string email, string username, string confirmationLink);
}