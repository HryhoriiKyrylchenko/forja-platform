namespace Forja.Application.Interfaces;

/// <summary>
/// Interface defining user registration, authentication, role management, and user-related operations.
/// </summary>
public interface IUserRegistrationService
{
    // User registration and authentication
    /// <summary>
    /// Registers a new user in the system by creating the user in the Keycloak identity store,
    /// generating a unique username, and saving the user details in the application database.
    /// </summary>
    /// <param name="request">The registration details, including the user's email and password.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RegisterUserAsync(RegisterUserCommand request);

    /// <summary>
    /// Authenticates a user with the provided credentials and generates a token response.
    /// </summary>
    /// <param name="request">The login request containing the user's email and password.</param>
    /// <returns>A <see cref="TokenResponse"/> containing the access token, refresh token, and other token information.</returns>
    Task<TokenResponse> LoginUserAsync(LoginUserCommand request);

    /// <summary>
    /// Logs out a user and invalidates the provided refresh token.
    /// </summary>
    /// <param name="request">The logout command containing the refresh token to be invalidated.</param>
    /// <returns>A task that represents the asynchronous logout operation.</returns>
    Task LogoutUserAsync(LogoutCommand request);

    /// <summary>
    /// Refreshes the authentication tokens using the provided refresh token.
    /// </summary>
    /// <param name="request">The command containing the refresh token to be used for generating new tokens.</param>
    /// <returns>A <see cref="TokenResponse"/> containing new access and refresh tokens.</returns>
    Task<TokenResponse> RefreshTokenAsync(RefreshTokenCommand request);
    
    // Role management
    /// <summary>
    /// Asynchronously creates a new role with the specified details.
    /// </summary>
    /// <param name="command">An instance of <see cref="CreateRoleCommand"/> containing the role name and description.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateRoleAsync(CreateRoleCommand command);

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
}