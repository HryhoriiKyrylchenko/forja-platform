namespace Forja.Infrastructure.Keycloak;

/// <summary>
/// Interface for managing communication with Keycloak.
/// </summary>
public interface IKeycloakClient
{
    /// <summary>
    /// Creates a new user in Keycloak based on the provided user details.
    /// </summary>
    /// <param name="user">The user details including email and password to create the new user.</param>
    /// <returns>The ID of the newly created user as a string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided user object is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the email or password is not provided in the user object.</exception>
    /// <exception cref="HttpRequestException">Thrown when there is a failure obtaining the admin token for the Keycloak request.</exception>
    /// <exception cref="Exception">
    /// Thrown when the user creation request fails, the response indicates an error, or if the Location header is missing in the response.
    /// </exception>
    Task<string> CreateUserAsync(KeycloakUser user);
    
    /// <summary>
    /// Asynchronously creates a new client role in Keycloak with the specified name and optional description.
    /// </summary>
    /// <param name="role">The name of the role to be created. This parameter cannot be null or whitespace.</param>
    /// <param name="description">An optional description for the role. Defaults to an empty string if not provided.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the KeycloakClient is not properly initialized.</exception>
    /// <exception cref="ArgumentException">Thrown when the role name is null, empty, or consists only of whitespace.</exception>
    /// <exception cref="HttpRequestException">Thrown when there is a failure to obtain the admin token.</exception>
    /// <exception cref="Exception">Thrown when the Keycloak server fails to create the client role, with details of the HTTP status and response.</exception>
    Task CreateClientRoleAsync(string role, string description = "");
    
    /// <summary>
    /// Gets the client roles from Keycloak.
    /// </summary>
    /// <returns>A list of RoleRepresentation objects.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the KeycloakClient is not properly initialized.</exception>
    /// <exception cref="HttpRequestException">Thrown when there is an issue obtaining the admin token.</exception>
    /// <exception cref="Exception">Thrown when there is an issue obtaining or deserializing the client roles.</exception>
    Task<List<RoleRepresentation>> GetClientRolesAsync();
    
    /// <summary>
    /// Retrieves the details of a client role by its name in the Keycloak server.
    /// </summary>
    /// <param name="userRole">The name of the client role to retrieve details for.</param>
    /// <returns>A <see cref="RoleRepresentation"/> object containing the details of the client role, or null if not found.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the Keycloak client is not properly initialized.</exception>
    /// <exception cref="HttpRequestException">Thrown when there is an issue obtaining the admin token.</exception>
    /// <exception cref="Exception">Thrown when the client role cannot be retrieved or deserialized.</exception>
    Task<RoleRepresentation?> GetClientRoleByNameAsync(string userRole);
    
    /// <summary>
    /// Retrieves a list of roles assigned to a specific user from Keycloak.
    /// </summary>
    /// <param name="userId">The unique identifier of the user for whom roles are to be retrieved.</param>
    /// <returns>A list of <see cref="RoleRepresentation"/> objects representing the roles assigned to the user.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Keycloak client is not properly configured.</exception>
    /// <exception cref="ArgumentException">Thrown if the userId parameter is null or whitespace.</exception>
    /// <exception cref="HttpRequestException">Thrown if there is an error while obtaining the admin token.</exception>
    /// <exception cref="Exception">Thrown if the request fails or if deserialization of the response fails.</exception>
    Task<List<RoleRepresentation>> GetUserRolesAsync(string userId);
    
    /// <summary>
    /// Checks if a user has a specific role assigned in Keycloak.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="userRole">The name of the role to check for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the user has the specified role.</returns>
    Task<bool> CheckUserRoleAsync(string userId, string userRole);
    
    /// <summary>
    /// Assigns multiple roles to a specified user in the Keycloak system.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to which roles will be assigned.</param>
    /// <param name="roles">A collection of roles that will be assigned to the user.</param>
    /// <returns>A task that represents the asynchronous operation of assigning roles.</returns>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="userId"/> is null, empty, or consists only of white-space characters, or if <paramref name="roles"/> is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the Keycloak client is not properly initialized.</exception>
    /// <exception cref="HttpRequestException">Thrown if there is an error obtaining the admin token.</exception>
    /// <exception cref="Exception">Thrown if the request to assign roles fails.</exception>
    Task AssignRolesAsync(string userId, IEnumerable<RoleRepresentation> roles);
    
    /// <summary>
    /// Assigns a specific role to a user in Keycloak.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to assign the role to.</param>
    /// <param name="role">The role to be assigned to the user.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AssignRoleAsync(string userId, RoleRepresentation role);
    
    /// <summary>
    /// Deletes specified client roles from a user in the Keycloak server.
    /// </summary>
    /// <param name="userId">The unique identifier of the user from whom the roles are to be removed.</param>
    /// <param name="roles">A collection of roles to be removed from the user.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Keycloak client is not properly initialized.</exception>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="userId"/> is null, empty, or whitespace, or if the <paramref name="roles"/> collection is null or empty.</exception>
    /// <exception cref="HttpRequestException">Thrown when the admin token could not be obtained.</exception>
    /// <exception cref="Exception">Thrown when the request to delete roles fails with an unsuccessful status.</exception>
    Task DeleteClientRolesAsync(string userId, IEnumerable<RoleRepresentation> roles);
    
    /// <summary>
    /// Deletes a specific client role from a user in Keycloak.
    /// </summary>
    /// <param name="userId">The unique identifier of the user from which the role is being removed.</param>
    /// <param name="role">The <see cref="RoleRepresentation"/> to be removed from the user's roles.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteClientRoleAsync(string userId, RoleRepresentation role);
    
    /// <summary>
    /// Retrieves a user from the Keycloak server by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <returns>A <see cref="UserRepresentation"/> object representing the user if found; otherwise, null.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Keycloak client is not properly initialized with the required configuration.</exception>
    /// <exception cref="ArgumentException">Thrown if the email parameter is null, empty, or consists only of whitespace.</exception>
    /// <exception cref="HttpRequestException">Thrown when there is an error obtaining an admin token from the Keycloak server.</exception>
    /// <exception cref="Exception">Thrown if the user data retrieval process fails or if the response cannot be deserialized.</exception>
    Task<UserRepresentation?> GetUserByEmailAsync(string email);
    
    /// <summary>
    /// Authenticates a user with the Keycloak server using the provided username and password.
    /// </summary>
    /// <param name="username">The username of the user to authenticate.</param>
    /// <param name="password">The password of the user to authenticate.</param>
    /// <returns>A <see cref="TokenResponse"/> containing the authentication token and related information.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Keycloak client is not properly initialized with required configuration values.</exception>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="username"/> or <paramref name="password"/> is null or whitespace.</exception>
    /// <exception cref="Exception">Thrown if the login attempt fails, the server response is invalid, or deserialization of the token response fails.</exception>
    Task<TokenResponse> LoginAsync(string username, string password);
    
    /// <summary>
    /// Logs out a user by invalidating the specified refresh token in the Keycloak server.
    /// </summary>
    /// <param name="refreshToken">The refresh token to be invalidated.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the KeycloakClient is not properly initialized.</exception>
    /// <exception cref="ArgumentException">Thrown if the refresh token is null or whitespace.</exception>
    /// <exception cref="Exception">Thrown if the logout request fails with an error response from the Keycloak server.</exception>
    Task LogoutAsync(string refreshToken);
    
    /// <summary>
    /// Requests new tokens from the Keycloak server using a refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token used to request new access and refresh tokens.</param>
    /// <returns>A <see cref="TokenResponse"/> containing the new access and refresh tokens.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the client has not been properly initialized with required configuration values.</exception>
    /// <exception cref="ArgumentException">Thrown when the provided refresh token is null or whitespace.</exception>
    /// <exception cref="Exception">Thrown when the request fails or the server response cannot be deserialized.</exception>
    Task<TokenResponse> RequestNewTokensAsync(string refreshToken);

    /// <summary>
    /// Changes the password of a user in Keycloak for the specified user ID.
    /// </summary>
    /// <param name="keycloakUserId">The unique identifier of the user in Keycloak whose password needs to be changed.</param>
    /// <param name="newPassword">The new password to be set for the user.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided user ID or password is null or empty.</exception>
    /// <exception cref="HttpRequestException">Thrown when there is an issue with the HTTP request to Keycloak.</exception>
    /// <exception cref="Exception">Thrown when an unexpected error occurs during the operation.</exception>
    Task ChangePasswordAsync(string keycloakUserId, string newPassword);

    /// <summary>
    /// Enables two-factor authentication (2FA) for the specified user in Keycloak by setting a required action for configuring TOTP.
    /// </summary>
    /// <param name="keycloakUserId">The unique ID of the user in Keycloak for whom 2FA will be enabled.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided Keycloak user ID is null or empty.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request to enable 2FA fails.</exception>
    /// <exception cref="Exception">Thrown when there is an error obtaining the access token or processing the request.</exception>
    Task EnableTwoFactorAuthenticationAsync(string keycloakUserId);

    /// <summary>
    /// Disables two-factor authentication (2FA) for a user in Keycloak.
    /// </summary>
    /// <param name="keycloakUserId">The unique identifier of the user in Keycloak whose 2FA is to be disabled.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided Keycloak user ID is null or empty.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request to Keycloak fails or the server returns an error status code.</exception>
    /// <exception cref="JsonException">Thrown when the response from Keycloak cannot be deserialized into the expected format.</exception>
    /// <exception cref="Exception">Thrown for other unexpected errors during the operation.</exception>
    Task DisableTwoFactorAuthenticationAsync(string keycloakUserId);

    /// <summary>
    /// Validates a given Keycloak authentication token.
    /// </summary>
    /// <param name="token">The Keycloak token to be validated.</param>
    /// <returns>A boolean indicating whether the token is active and valid.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the base URL, realm, client ID, client secret, or token is null or empty.
    /// </exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request to validate the token fails.</exception>
    /// <exception cref="JsonException">Thrown when the response content cannot be successfully deserialized.</exception>
    Task<bool> ValidateTokenAsync(string token);

    /// <summary>
    /// Enables or disables a user in Keycloak based on the given user ID and enable flag.
    /// </summary>
    /// <param name="keycloakUserId">The unique identifier of the user in Keycloak.</param>
    /// <param name="enable">A boolean indicating whether to enable (true) or disable (false) the user.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the user ID is null or empty.</exception>
    /// <exception cref="HttpRequestException">Thrown when an error occurs while sending the HTTP request to Keycloak.</exception>
    /// <exception cref="Exception">Thrown when there is an error obtaining the access token or handling the HTTP response.</exception>
    Task EnableDisableUserAsync(string keycloakUserId, bool enable);

    /// <summary>
    /// Extracts the Keycloak user ID from the provided access token.
    /// </summary>
    /// <param name="accessToken">The JWT access token containing the user ID claim.</param>
    /// <returns>The user ID extracted from the "sub" claim in the access token.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided access token is null.</exception>
    /// <exception cref="Exception">Thrown if the user ID ("sub" claim) is not found in the access token.</exception>
    string GetKeycloakUserId(string accessToken);

    /// <summary>
    /// Resets the password for a specified user in Keycloak.
    /// </summary>
    /// <param name="userId">The ID of the user whose password is to be reset.</param>
    /// <param name="newPassword">The new password to set for the user.</param>
    /// <param name="temporary">
    /// Indicates whether the new password should be marked as temporary, requiring the user to change it upon their next login.
    /// </param>
    /// <returns>An asynchronous operation representing the password reset task.</returns>
    /// <exception cref="ArgumentException">Thrown when the user ID or new password is null, empty, or contains only whitespaces.</exception>
    /// <exception cref="HttpRequestException">Thrown when the request fails, such as network issues or invalid response from the server.</exception>
    /// <exception cref="Exception">Thrown when there is a failure while obtaining the access token or processing the response.</exception>
    Task ResetUserPasswordAsync(string userId, string newPassword, bool temporary = false);

    /// <summary>
    /// Sends an email to the specified user to perform required actions, such as updating their password or verifying their email.
    /// </summary>
    /// <param name="userId">The ID of the user to whom the email should be sent.</param>
    /// <param name="actions">A collection of required actions for the user to complete (e.g., "VERIFY_EMAIL", "UPDATE_PASSWORD").</param>
    /// <param name="lifespan">The validity period of the action link in seconds. Defaults to 3600 seconds (1 hour).</param>
    /// <param name="redirectUri">Optional URI to redirect the user upon completing the required actions. Defaults to null.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="HttpRequestException">Thrown when the server request fails or the response indicates an HTTP error.</exception>
    /// <exception cref="Exception">Thrown when there is an error in creating or sending the request.</exception>
    Task SendRequiredActionEmailAsync(string userId, IEnumerable<string> actions, int lifespan = 3600, string? redirectUri = null);

    /// <summary>
    /// Confirms a user's email in Keycloak by marking the email as verified.
    /// </summary>
    /// <param name="keycloakUserId">The unique ID of the user in Keycloak whose email is to be confirmed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided keycloakUserId is null or whitespace.</exception>
    /// <exception cref="HttpRequestException">Thrown when there is an error during the HTTP request to Keycloak.</exception>
    /// <exception cref="Exception">Thrown when there is an issue obtaining the access token or with the email confirmation process.</exception>
    Task ConfirmUserEmailAsync(string keycloakUserId);
}