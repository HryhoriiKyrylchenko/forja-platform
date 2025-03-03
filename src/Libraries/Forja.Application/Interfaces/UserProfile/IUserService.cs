namespace Forja.Application.Interfaces.UserProfile;

public interface IUserService
{
    // UserRepository
    /// <summary>
    /// Retrieves the user profile associated with the specified Keycloak ID.
    /// </summary>
    /// <param name="userKeycloakId">The unique Keycloak ID of the user to retrieve the profile for.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains the UserProfileDto for the user associated with the specified Keycloak ID.</returns>
    Task<UserProfileDto> GetUserProfileAsync(string userKeycloakId);

    /// <summary>
    /// Updates the user profile with the provided information.
    /// </summary>
    /// <param name="userProfileDto">The UserProfileDto containing updated user profile information.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateUserProfileAsync(UserProfileDto userProfileDto);

    /// <summary>
    /// Deletes a user identified by the given Keycloak ID.
    /// </summary>
    /// <param name="userKeycloakId">The Keycloak ID of the user to be deleted.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteUserAsync(string userKeycloakId);

    /// <summary>
    /// Restores a previously deleted user associated with the given Keycloak ID.
    /// </summary>
    /// <param name="userKeycloakId">The unique Keycloak ID of the user to be restored.</param>
    /// <returns>A Task representing the result of the asynchronous operation.</returns>
    Task RestoreUserAsync(string userKeycloakId);

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
}