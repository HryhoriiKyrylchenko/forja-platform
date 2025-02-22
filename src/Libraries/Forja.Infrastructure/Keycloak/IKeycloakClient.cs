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
    
    //TODO: Add change password functionality
    //TODO: Add 2FA functionality
    //TODO: Add validate token functionality
}