namespace Forja.Application.Interfaces.UserProfile;

/// <summary>
/// Defines a contract for managing user profiles and their related data.
/// </summary>
/// <remarks>
/// The IUserService interface provides methods to perform CRUD operations on user profiles,
/// retrieve user data by various identifiers such as email or Keycloak ID,
/// and manage deleted user records.
/// </remarks>
public interface IUserService
{
    /// <summary>
    /// Retrieves the user profile associated with the specified unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task representing the asynchronous operation, containing the <see cref="UserProfileDto"/> object if found, or null if the user does not exist.</returns>
    Task<UserProfileDto?> GetUserByIdAsync(Guid userId);

    /// <summary>
    /// Retrieves the profile of a deleted user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the deleted user.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a <see cref="UserProfileDto"/> object if the user is found,
    /// otherwise null.
    /// </returns>
    Task<UserProfileDto?> GetDeletedUserByIdAsync(Guid userId);

    /// <summary>
    /// Retrieves a user profile based on the provided Keycloak ID.
    /// </summary>
    /// <param name="userKeycloakId">The Keycloak ID associated with the user profile to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation, containing a <see cref="UserProfileDto"/> object if a user is found, or null if no user matches the provided Keycloak ID.</returns>
    Task<UserProfileDto?> GetUserByKeycloakIdAsync(string userKeycloakId);

    /// <summary>
    /// Retrieves a deleted user profile by the specified Keycloak ID.
    /// </summary>
    /// <param name="userKeycloakId">The Keycloak ID of the user to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a <see cref="UserProfileDto"/>
    /// object of the deleted user, or null if no user is found.
    /// </returns>
    Task<UserProfileDto?> GetDeletedUserByKeycloakIdAsync(string userKeycloakId);

    /// <summary>
    /// Retrieves a user profile by the provided email address.
    /// </summary>
    /// <param name="email">The email address of the user whose profile is to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation, containing a <see cref="UserProfileDto"/> object if a user is found; otherwise, null.</returns>
    Task<UserProfileDto?> GetUserByEmailAsync(string email);

    /// <summary>
    /// Retrieves a deleted user profile by their email address.
    /// </summary>
    /// <param name="email">The email address of the deleted user to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation, containing a <see cref="UserProfileDto"/> object if the deleted user is found; otherwise, null.</returns>
    Task<UserProfileDto?> GetDeletedUserByEmailAsync(string email);
    
    /// <summary>
    /// Retrieves a list of all user profiles.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing a list of <see cref="UserProfileDto"/> objects.</returns>
    Task<List<UserProfileDto>> GetAllUsersAsync();

    /// <summary>
    /// Retrieves a list of all deleted user profiles.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="UserProfileDto"/> representing deleted user profiles.</returns>
    Task<List<UserProfileDto>> GetAllDeletedUsersAsync();

    /// <summary>
    /// Adds a new user profile to the system based on the specified creation request.
    /// </summary>
    /// <param name="request">The UserCreateRequest containing the details of the new user to be added.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains the UserProfileDto of the newly created user, or null if the operation fails.</returns>
    Task<UserProfileDto?> AddUserAsync(UserCreateRequest request);
    
    /// <summary>
    /// Updates the user profile with the provided information.
    /// </summary>
    /// <param name="request">The UserUpdateRequest containing updated user profile information.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<UserProfileDto?> UpdateUserAsync(UserUpdateRequest request);

    /// <summary>
    /// Deletes a user identified by the given Keycloak ID.
    /// </summary>
    /// <param name="userId">The ID of the user to be deleted.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteUserAsync(Guid userId);

    /// <summary>
    /// Permanently deletes a user profile associated with the specified Keycloak identifier.
    /// </summary>
    /// <param name="userKeycloakId">The Keycloak identifier of the user to be deleted.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteUserAsync(string userKeycloakId);

    /// <summary>
    /// Restores a previously deleted user associated with the given Keycloak ID.
    /// </summary>
    /// <param name="userId">The unique ID of the user to be restored.</param>
    /// <returns>A Task representing the result of the asynchronous operation.</returns>
    Task<UserProfileDto?> RestoreUserAsync(Guid userId);

    /// <summary>
    /// Removes or obfuscates personal data from the specified user profile to protect privacy.
    /// </summary>
    /// <param name="user">The user profile containing data to be sanitized.</param>
    /// <returns>A <see cref="UserProfileDto"/> object with personal information removed or hidden.</returns>
    UserProfileDto HidePersonalData(UserProfileDto user);
}