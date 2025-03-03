namespace Forja.Application.Interfaces.UserProfile;

/// <summary>
/// Interface for managing user achievements within the system.
/// </summary>
public interface IUserAchievementService
{
    /// <summary>
    /// Adds a new user achievement to the system.
    /// </summary>
    /// <param name="userAchievementDto">The UserAchievementDto containing the details of the user achievement to be added.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task AddUserAchievementAsync(UserAchievementDto userAchievementDto);

    /// <summary>
    /// Retrieves the user achievement associated with the specified achievement ID.
    /// </summary>
    /// <param name="userAchievementId">The unique identifier of the user achievement to retrieve.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains the UserAchievementDto for the user achievement associated with the specified ID.</returns>
    Task<UserAchievementDto> GetUserAchievementByIdAsync(Guid userAchievementId);

    /// <summary>
    /// Updates an existing user achievement with the provided details.
    /// </summary>
    /// <param name="userAchievementDto">The UserAchievementDto containing updated information for the user achievement.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task UpdateUserAchievement(UserAchievementDto userAchievementDto);

    /// <summary>
    /// Deletes a specific user achievement identified by the provided ID.
    /// </summary>
    /// <param name="userAchievementId">The unique identifier of the user achievement to be deleted.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task DeleteUserAchievementAsync(Guid userAchievementId);

    /// <summary>
    /// Retrieves all user achievements.
    /// </summary>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of UserAchievementDto objects representing all user achievements.</returns>
    Task<List<UserAchievementDto>> GetAllUserAchievementsAsync();

    /// <summary>
    /// Retrieves all achievements associated with the specified user's Keycloak ID.
    /// </summary>
    /// <param name="userKeycloakId">The unique Keycloak ID of the user whose achievements are to be retrieved.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of UserAchievementDto objects associated with the specified Keycloak ID.</returns>
    Task<List<UserAchievementDto>> GetAllUserAchievementsByUserKeycloakIdAsync(string userKeycloakId);

    /// <summary>
    /// Retrieves all user achievements associated with the specified game ID.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game to retrieve user achievements for.</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains a list of UserAchievementDto objects associated with the specified game ID.</returns>
    Task<List<UserAchievementDto>> GetAllUserAchievementsByGameIdAsync(Guid gameId);
}