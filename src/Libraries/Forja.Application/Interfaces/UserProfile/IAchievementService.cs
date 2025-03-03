namespace Forja.Application.Interfaces.UserProfile;

/// <summary>
/// Provides methods for managing and interacting with achievements.
/// </summary>
public interface IAchievementService
{
    // AchievementRepository
    /// <summary>
    /// Adds an achievement to the specified user's profile.
    /// </summary>
    /// <param name="userKeycloakId">The unique Keycloak ID of the user to associate the achievement with.</param>
    /// <param name="achievementDto">The AchievementDto object containing the details of the achievement to be added.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task AddAchievementAsync(string userKeycloakId, AchievementDto achievementDto);

    /// <summary>
    /// Retrieves the achievement associated with the specified achievement ID.
    /// </summary>
    /// <param name="achievementId">The unique identifier of the achievement to retrieve.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains the AchievementDto for the specified achievement ID.</returns>
    Task<AchievementDto> GetAchievementByIdAsync(Guid achievementId);

    /// <summary>
    /// Updates the details of a specific achievement.
    /// </summary>
    /// <param name="achievementDto">The AchievementDto object containing the updated information for the achievement.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task UpdateAchievementAsync(AchievementDto achievementDto);

    /// <summary>
    /// Deletes an achievement based on its unique identifier.
    /// </summary>
    /// <param name="achievementId">The unique identifier of the achievement to be deleted.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task DeleteAchievementAsync(Guid achievementId);

    /// <summary>
    /// Restores a previously deleted achievement identified by the specified achievement ID.
    /// </summary>
    /// <param name="achievementId">The unique identifier of the achievement to restore.</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains the restored AchievementDto.</returns>
    Task<AchievementDto> RestoreAchievementAsync(Guid achievementId);

    /// <summary>
    /// Retrieves all achievements associated with the specified game ID.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game for which to retrieve achievements.</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains a list of AchievementDto objects associated with the specified game.</returns>
    Task<List<AchievementDto>> GetAllGameAchievementsAsync(Guid gameId);

    /// <summary>
    /// Retrieves a list of deleted achievements associated with the specified game.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game whose deleted achievements are to be retrieved.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of AchievementDto objects representing the deleted achievements associated with the specified game.</returns>
    Task<List<AchievementDto>> GetAllGameDeletedAchievementsAsync(Guid gameId);

    /// <summary>
    /// Retrieves a list of all achievements available in the system.
    /// </summary>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of AchievementDto objects representing all achievements.</returns>
    Task<List<AchievementDto>> GetAllAchievementsAsync();

    /// <summary>
    /// Retrieves a list of all deleted achievements.
    /// </summary>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of AchievementDto representing the deleted achievements.</returns>
    Task<List<AchievementDto>> GetAllDeletedAchievementsAsync();
}