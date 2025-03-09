namespace Forja.Application.Validators;

/// <summary>
/// Provides static methods for validating authentication-related requests.
/// This class includes validation logic for various types of requests,
/// ensuring required data is present and conforms to expected standards.
/// </summary>
public static class AuthenticationRequestsValidator
{
    /// <summary>
    /// Validates the CreateRoleRequest to ensure that the necessary data is present.
    /// </summary>
    /// <param name="request">The CreateRoleRequest object containing the role details to be validated.</param>
    /// <param name="errorMessage">An output parameter that will hold the error message if validation fails.</param>
    /// <returns>
    /// A boolean value indicating whether the CreateRoleRequest is valid. Returns true if the request is valid; otherwise, false.
    /// </returns>
    public static bool ValidateCreateRoleRequest(CreateRoleRequest request, out string errorMessage)
    {
        if (string.IsNullOrEmpty(request.RoleName))
        {
            errorMessage = "RoleName cannot be empty.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates the request for enabling or disabling a user account.
    /// </summary>
    /// <param name="request">
    /// An instance of <see cref="EnableDisableUserRequest"/> containing the necessary data for the operation.
    /// </param>
    /// <param name="errorMessage">
    /// An output parameter that will contain the error message if the validation fails, or an empty string if it succeeds.
    /// </param>
    /// <returns>
    /// A boolean value indicating whether the request is valid.
    /// </returns>
    public static bool ValidateEnableDisableUserRequest(EnableDisableUserRequest request, out string errorMessage)
    {
        if (string.IsNullOrEmpty(request.KeycloakUserId))
        {
            errorMessage = "KeycloakUserId cannot be empty.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates the ForgotPasswordRequest object to ensure required fields are populated and valid.
    /// </summary>
    /// <param name="request">The ForgotPasswordRequest object containing the email to be validated.</param>
    /// <param name="errorMessage">An output parameter that will contain an error message if the validation fails; otherwise, it will be an empty string.</param>
    /// <returns>Returns true if the request is valid; otherwise, false.</returns>
    public static bool ValidateForgotPasswordRequest(ForgotPasswordRequest request, out string errorMessage)
    {
        if (string.IsNullOrEmpty(request.Email))
        {
            errorMessage = "Email cannot be empty.";
            return false;
        }

        if (!IsValidEmail(request.Email))
        {
            errorMessage = "Invalid email format.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates the login user request by checking the email and password fields for validity.
    /// </summary>
    /// <param name="request">The LoginUserRequest containing the email and password to validate.</param>
    /// <param name="errorMessage">An output string containing the error message, if validation fails.</param>
    /// <returns>A boolean value indicating whether the validation was successful.</returns>
    public static bool ValidateLoginUserRequest(LoginUserRequest request, out string errorMessage)
    {
        if (string.IsNullOrEmpty(request.Email))
        {
            errorMessage = "Email cannot be empty.";
            return false;
        }

        if (!IsValidEmail(request.Email))
        {
            errorMessage = "Invalid email format.";
            return false;
        }

        if (string.IsNullOrEmpty(request.Password))
        {
            errorMessage = "Password cannot be empty.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates the given logout request to ensure that it contains the required information.
    /// </summary>
    /// <param name="request">The logout request to validate, containing the refresh token.</param>
    /// <param name="errorMessage">An output parameter that will contain the error message if validation fails.</param>
    /// <returns>True if the logout request is valid; otherwise, false.</returns>
    public static bool ValidateLogoutRequest(LogoutRequest request, out string errorMessage)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
        {
            errorMessage = "RefreshToken cannot be empty.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates the provided RefreshTokenRequest to ensure it contains the required data.
    /// </summary>
    /// <param name="request">The RefreshTokenRequest to validate.</param>
    /// <param name="errorMessage">Outputs an error message if validation fails.</param>
    /// <returns>Returns true if the request is valid; otherwise, false.</returns>
    public static bool ValidateRefreshTokenRequest(RefreshTokenRequest request, out string errorMessage)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
        {
            errorMessage = "RefreshToken cannot be empty.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates a <see cref="RegisterUserRequest"/> object to ensure it contains
    /// valid data for user registration.
    /// </summary>
    /// <param name="request">
    /// The <see cref="RegisterUserRequest"/> instance containing user registration data.
    /// </param>
    /// <param name="errorMessage">
    /// Outputs an error message describing the validation failure, if any.
    /// This parameter will be an empty string if validation succeeds.
    /// </param>
    /// <returns>
    /// A boolean value indicating whether the validation succeeded.
    /// Returns true if the request is valid; otherwise, false.
    /// </returns>
    public static bool ValidateRegisterUserRequest(RegisterUserRequest request, out string errorMessage)
    {
        if (string.IsNullOrEmpty(request.Email))
        {
            errorMessage = "Email cannot be empty.";
            return false;
        }

        if (!IsValidEmail(request.Email))
        {
            errorMessage = "Invalid email format.";
            return false;
        }

        if (!IsValidPassword(request.Password))
        {
            errorMessage = "Password is not valid. It must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates the ResetUserPasswordRequest to ensure all required fields are populated and valid.
    /// </summary>
    /// <param name="request">The ResetUserPasswordRequest object containing the details for the password reset.</param>
    /// <param name="errorMessage">Returns a descriptive error message if the validation fails.</param>
    /// <returns>True if the request is valid; otherwise, false.</returns>
    public static bool ValidateResetUserPasswordRequest(ResetUserPasswordRequest request, out string errorMessage)
    {
        if (string.IsNullOrEmpty(request.KeycloakUserId))
        {
            errorMessage = "KeycloakUserId cannot be empty.";
            return false;
        }

        if (!IsValidPassword(request.Password))
        {
            errorMessage = "Password is not valid. It must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates the provided reset token request.
    /// </summary>
    /// <param name="request">The reset token request to be validated.</param>
    /// <param name="errorMessage">The error message if validation fails.</param>
    /// <returns>True if the validation succeeds; otherwise, false.</returns>
    public static bool ValidateValidateResetTokenRequest(ValidateResetTokenRequest request, out string errorMessage)
    {
        if (string.IsNullOrEmpty(request.Token))
        {
            errorMessage = "Token cannot be empty.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates the ChangePasswordRequest to ensure that the necessary data is present and meets required criteria.
    /// </summary>
    /// <param name="request">The ChangePasswordRequest object containing the new password details to be validated.</param>
    /// <param name="errorMessage">An output parameter that will hold the error message if validation fails.</param>
    /// <returns>
    /// A boolean value indicating whether the ChangePasswordRequest is valid. Returns true if the request is valid; otherwise, false.
    /// </returns>
    public static bool ValidateChangePasswordRequest(ChangePasswordRequest request, out string errorMessage)
    {
   
        if (string.IsNullOrWhiteSpace(request.Password))
        {
            errorMessage = "Password cannot be empty.";
            return false;
        }
    
        if (!IsValidPassword(request.Password))
        {
            errorMessage = "NewPassword must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character.";
            return false;
        }
    
        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates whether the provided email address has a valid format.
    /// </summary>
    /// <param name="email">The email address string to validate.</param>
    /// <returns>True if the email address is in a valid format; otherwise, false.</returns>
    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    
    /// <summary>
    /// Validates whether the provided password meets the security requirements.
    /// </summary>
    /// <param name="password">The password string to validate.</param>
    /// <returns>True if the password is valid; otherwise, false.</returns>
    private static bool IsValidPassword(string password)
    {
        if (password.Length < 8)
            return false;
    
        if (!password.Any(char.IsUpper))
            return false;
    
        if (!password.Any(char.IsLower))
            return false;
    
        if (!password.Any(char.IsDigit))
            return false;
    
        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            return false;
    
        return true;
    }
}