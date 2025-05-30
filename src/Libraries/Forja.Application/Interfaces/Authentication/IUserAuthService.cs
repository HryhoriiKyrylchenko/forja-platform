namespace Forja.Application.Interfaces.Authentication;

/// <summary>
/// Interface defining user registration, authentication, role management, and user-related operations.
/// </summary>
public interface IUserAuthService
{
    /// <summary>
    /// Registers a new user with the provided registration details.
    /// </summary>
    /// <param name="request">The registration request containing the user's information required for account creation.</param>
    /// <returns>A <see cref="UserProfileDto"/> containing the newly registered user's profile information, or null if the registration fails.</returns>
    Task<UserProfileDto?> RegisterUserAsync(RegisterUserRequest request);

    /// <summary>
    /// Authenticates a user with the provided credentials and generates a token response.
    /// </summary>
    /// <param name="request">The login request containing the user's email and password.</param>
    /// <returns>A <see cref="TokenResponse"/> containing the access token, refresh token, and other token information.</returns>
    Task<TokenResponse> LoginUserAsync(LoginUserRequest request);

    /// <summary>
    /// Logs out a user and invalidates the provided refresh token.
    /// </summary>
    /// <param name="request">The logout command containing the refresh token to be invalidated.</param>
    /// <returns>A task that represents the asynchronous logout operation.</returns>
    Task LogoutUserAsync(LogoutRequest request);

    /// <summary>
    /// Refreshes the authentication tokens using the provided refresh token.
    /// </summary>
    /// <param name="request">The command containing the refresh token to be used for generating new tokens.</param>
    /// <returns>A <see cref="TokenResponse"/> containing new access and refresh tokens.</returns>
    Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
    
    // Role management
    /// <summary>
    /// Asynchronously creates a new role with the specified details.
    /// </summary>
    /// <param name="request">An instance of <see cref="CreateRoleRequest"/> containing the role name and description.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateRoleAsync(CreateRoleRequest request);

    /// <summary>
    /// Retrieves all roles available in the Keycloak system.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains an enumerable list of <see cref="RoleRepresentation"/> representing all roles.</returns>
    Task<IEnumerable<RoleRepresentation>> GetAllRolesAsync();

    /// <summary>
    /// Retrieves a role representation by its name asynchronously.
    /// </summary>
    /// <param name="roleName">The name of the role to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="RoleRepresentation"/> object if found; otherwise, null.</returns>
    Task<RoleRepresentation?> GetRoleByNameAsync(string roleName);

    /// <summary>
    /// Retrieves the roles associated with a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="RoleRepresentation"/> associated with the user.</returns>
    Task<IEnumerable<RoleRepresentation>> GetUserRolesAsync(string userId);

    /// <summary>
    /// Checks if a user has a specific role.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="roleName">The name of the role to check.</param>
    /// <returns>Returns a boolean indicating whether the user has the specified role.</returns>
    Task<bool> CheckUserRoleAsync(string userId, string roleName);

    /// <summary>
    /// Assigns a list of roles to a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to whom the roles will be assigned.</param>
    /// <param name="roles">A collection of roles to be assigned to the user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AssignRolesToUserAsync(string userId, IEnumerable<RoleRepresentation> roles);

    /// <summary>
    /// Assigns a role to a specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to whom the role will be assigned.</param>
    /// <param name="role">The role to be assigned to the user.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AssignRoleToUserAsync(string userId, RoleRepresentation role);

    /// <summary>
    /// Assigns a specific role to a user in the system.
    /// </summary>
    /// <param name="userId">The identifier of the user to whom the role will be assigned.</param>
    /// <param name="role">The role that needs to be assigned to the user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AssignRoleToUserAsync(string userId, UserRole role);

    /// <summary>
    /// Deletes specified roles from a user in the Keycloak client.
    /// </summary>
    /// <param name="userId">The unique identifier of the user from whom the roles will be removed.</param>
    /// <param name="roles">A collection of roles to be removed from the user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteRolesFromUserAsync(string userId, IEnumerable<RoleRepresentation> roles);

    /// <summary>
    /// Removes a specific role from a user in the system.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="role">The role to be removed from the user.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteRoleFromUserAsync(string userId, RoleRepresentation role);

    // User management
    /// <summary>
    /// Retrieves a user from Keycloak using their email address.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <returns>A <see cref="UserRepresentation"/> object if the user is found; otherwise, null.</returns>
    Task<UserRepresentation?> GetKeycloakUserByEmailAsync(string email);

    /// <summary>
    /// Updates the password for a user in the Keycloak identity store.
    /// </summary>
    /// <param name="keycloakUserId">The unique identifier of the user in the Keycloak system.</param>
    /// <param name="newPassword">The new password to be set for the user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ChangePasswordAsync(string keycloakUserId, string newPassword);

    /// <summary>
    /// Enables two-factor authentication for the specified user in the identity provider.
    /// The two-factor authentication process adds an additional layer of security for user accounts.
    /// </summary>
    /// <param name="keycloakUserId">The unique identifier of the user in the identity provider for whom two-factor authentication is to be enabled.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task EnableTwoFactorAuthenticationAsync(string keycloakUserId);

    /// <summary>
    /// Disables two-factor authentication for a specified user in the system.
    /// </summary>
    /// <param name="keycloakUserId">The unique identifier of the user in Keycloak whose two-factor authentication is to be disabled.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DisableTwoFactorAuthenticationAsync(string keycloakUserId);

    /// <summary>
    /// Validates a given token by checking its authenticity using the associated identity provider or token service.
    /// </summary>
    /// <param name="token">The token to be validated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating if the token is valid or not.</returns>
    Task<bool> ValidateTokenAsync(string token);

    /// <summary>
    /// Initiates the forgot password process for a user by triggering the appropriate mechanism
    /// (e.g., sending a password reset link or token) via the identity provider.
    /// </summary>
    /// <param name="email">The email address of the user requesting a password reset.</param>
    /// <param name="locale">
    /// The locale (e.g., "en", "uk") to be used for generating localized content during the password reset process,
    /// such as the language of the email or the reset link path.
    /// </param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task TriggerForgotPasswordAsync(string email, string locale);


    /// <summary>
    /// Enables or disables a user in the system based on the provided keycloak user ID.
    /// </summary>
    /// <param name="request">The EnableDisableUserRequest.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task EnableDisableUserAsync(EnableDisableUserRequest request);

    /// <summary>
    /// Resets the password of a specified user in the system.
    /// </summary>
    /// <param name="request">The ResetUserPasswordRequest.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ResetUserPasswordAsync(ResetUserPasswordRequest request);

    /// <summary>
    /// Retrieves the Keycloak user ID associated with the specified access token.
    /// </summary>
    /// <param name="accessToken">The access token used to identify the user in Keycloak.</param>
    /// <returns>A task that represents the asynchronous operation, containing the user's Keycloak ID as a string.</returns>
    Task<string> GetKeycloakUserIdAsync(string accessToken);

    /// <summary>
    /// Validates the provided password reset token to ensure its authenticity and usability.
    /// </summary>
    /// <param name="request">The ValidateResetTokenRequest to validate.</param>
    /// <returns>A task that represents the asynchronous operation, including a boolean value indicating whether the token is valid.</returns>
    Task<bool> ValidateResetTokenAsync(ValidateResetTokenRequest request);

    /// <summary>
    /// Sends an email confirmation to the specified user, including a confirmation link to verify their email address.
    /// </summary>
    /// <param name="keycloakUserId">The id associated with the user in Keycloak database.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SendEmailConfirmationAsync(string keycloakUserId);
    
    /// <summary>
    /// Confirms a user's email using a provided confirmation token.
    /// </summary>
    /// <param name="token">The email confirmation token provided to the user.</param>
    /// <returns>A task representing the asynchronous email confirmation operation.</returns>
    Task ConfirmUserEmailAsync(string token);

    /// <summary>
    /// Extracts the user ID from the provided token.
    /// </summary>
    /// <param name="token">The token containing encoded user information.</param>
    /// <returns>A <see cref="Guid"/> representing the user's ID if successfully extracted; otherwise, null.</returns>
    Task<string> GetUserIdFromToken(string token);
}