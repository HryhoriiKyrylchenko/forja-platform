namespace Forja.Application.Interfaces;

public interface IUserProfileService
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
    
    //TODO: Add restore user functionality

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
    
    // ReviewRepository
    /// <summary>
    /// Adds a new review for the specified user.
    /// </summary>
    /// <param name="userKeycloakId">The unique Keycloak ID of the user adding the review.</param>
    /// <param name="reviewDto">The ReviewDto object containing the details of the review to be added.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task AddReviewAsync(string userKeycloakId, ReviewDto reviewDto);

    /// <summary>
    /// Retrieves the review associated with the specified review ID.
    /// </summary>
    /// <param name="reviewId">The unique identifier of the review to retrieve.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains the ReviewDto associated with the specified review ID.</returns>
    Task<ReviewDto> GetReviewByIdAsync(Guid reviewId);

    /// <summary>
    /// Updates the review based on the provided review details.
    /// </summary>
    /// <param name="reviewDto">The ReviewDto object containing the review details to be updated, such as rating, user ID, and game ID.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task UpdateReviewAsync(ReviewDto reviewDto);

    /// <summary>
    /// Deletes the review associated with the specified review ID.
    /// </summary>
    /// <param name="reviewId">The unique identifier of the review to be deleted.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task DeleteReviewAsync(Guid reviewId);

    /// <summary>
    /// Restores a previously deleted review identified by the given review ID.
    /// </summary>
    /// <param name="reviewId">The unique identifier of the review to restore.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains the restored ReviewDto associated with the specified review ID.</returns>
    Task<ReviewDto> RestoreReviewAsync(Guid reviewId);

    /// <summary>
    /// Retrieves all reviews associated with the specified user's Keycloak ID.
    /// </summary>
    /// <param name="userKeycloakId">The unique Keycloak ID of the user whose reviews are to be retrieved.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of ReviewDto objects representing the reviews associated with the specified user.</returns>
    Task<List<ReviewDto>> GetAllUserReviewsAsync(string userKeycloakId);

    /// <summary>
    /// Retrieves all deleted reviews associated with the specified user's Keycloak ID.
    /// </summary>
    /// <param name="userKeycloakId">The unique Keycloak ID of the user whose deleted reviews are to be retrieved.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of ReviewDto objects representing the deleted reviews associated with the specified Keycloak ID.</returns>
    Task<List<ReviewDto>> GetAllUserDeletedReviewsAsync(string userKeycloakId);

    /// <summary>
    /// Retrieves all reviews associated with a specified game.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game to retrieve reviews for.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of ReviewDto objects associated with the specified game.</returns>
    Task<List<ReviewDto>> GetAllGameReviewsAsync(Guid gameId);

    /// <summary>
    /// Retrieves all deleted game reviews associated with the specified game ID.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game to retrieve deleted reviews for.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of ReviewDto objects representing the deleted reviews for the specified game.</returns>
    Task<List<ReviewDto>> GetAllDeletedGameReviewsAsync(Guid gameId);

    /// <summary>
    /// Retrieves all reviews present in the system.
    /// </summary>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of ReviewDto objects representing all reviews.</returns>
    Task<List<ReviewDto>> GetAllReviewsAsync();

    /// <summary>
    /// Retrieves all reviews that have not been approved.
    /// </summary>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of ReviewDto objects representing the reviews that are not approved.</returns>
    Task<List<ReviewDto>> GetAllNotApprovedReviewsAsync();

    /// <summary>
    /// Retrieves a list of all deleted reviews.
    /// </summary>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of ReviewDto objects representing the deleted reviews.</returns>
    Task<List<ReviewDto>> GetAllDeletedReviewsAsync();
    
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
    
    // UserAchievementRepository
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
    
    // UserLibraryGameRepository
    /// <summary>
    /// Adds a game to the user's library.
    /// </summary>
    /// <param name="userLibraryGameDto">The data transfer object that represents the game to be added to the user's library.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task AddUserLibraryGameAsync(UserLibraryGameDto userLibraryGameDto);

    /// <summary>
    /// Updates the details of a user's library game entry.
    /// </summary>
    /// <param name="userLibraryGameDto">The data transfer object containing updated information for the user library game entry.</param>
    /// <returns>A Task representing the result of the asynchronous operation.</returns>
    Task UpdateUserLibraryGameAsync(UserLibraryGameDto userLibraryGameDto);

    /// <summary>
    /// Deletes the user library game associated with the specified ID.
    /// </summary>
    /// <param name="userLibraryGameId">The unique identifier of the user library game to delete.</param>
    /// <returns>A Task representing the asynchronous operation of deleting the user library game.</returns>
    Task DeleteUserLibraryGameAsync(Guid userLibraryGameId);

    /// <summary>
    /// Restores a previously deleted user library game entry identified by the specified ID.
    /// </summary>
    /// <param name="userLibraryGameId">The unique identifier of the user library game to restore.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains the restored UserLibraryGameDto.</returns>
    Task<UserLibraryGameDto> RestoreUserLibraryGameAsync(Guid userLibraryGameId);

    /// <summary>
    /// Retrieves the user library game associated with the specified ID.
    /// </summary>
    /// <param name="userLibraryGameId">The unique identifier of the user library game to retrieve.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains the UserLibraryGameDto for the user library game associated with the specified ID.</returns>
    Task<UserLibraryGameDto> GetUserLibraryGameByIdAsync(Guid userLibraryGameId);

    /// <summary>
    /// Retrieves the deleted user library game associated with the specified ID.
    /// </summary>
    /// <param name="userLibraryGameId">The unique identifier of the deleted user library game to retrieve.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains the UserLibraryGameDto for the deleted user library game associated with the specified ID.</returns>
    Task<UserLibraryGameDto> GetDeletedUserLibraryGameByIdAsync(Guid userLibraryGameId);

    /// <summary>
    /// Retrieves a list of all user library games.
    /// </summary>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of UserLibraryGameDto representing all user library games.</returns>
    Task<List<UserLibraryGameDto>> GetAllUserLibraryGamesAsync();

    /// <summary>
    /// Retrieves a list of all deleted user library games.
    /// </summary>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of UserLibraryGameDto objects representing the deleted user library games.</returns>
    Task<List<UserLibraryGameDto>> GetAllDeletedUserLibraryGamesAsync();

    /// <summary>
    /// Retrieves all library games associated with the specified user's Keycloak ID.
    /// </summary>
    /// <param name="userKeycloakId">The unique Keycloak ID of the user whose library games are to be retrieved.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of UserLibraryGameDto objects for the library games associated with the specified user's Keycloak ID.</returns>
    Task<List<UserLibraryGameDto>> GetAllUserLibraryGamesByUserKeycloakIdAsync(string userKeycloakId);

    /// <summary>
    /// Retrieves all deleted user library games associated with the specified Keycloak ID.
    /// </summary>
    /// <param name="userKeycloakId">The unique Keycloak ID of the user whose deleted library games are to be retrieved.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of UserLibraryGameDto objects for the deleted library games associated with the specified Keycloak ID.</returns>
    Task<List<UserLibraryGameDto>> GetAllDeletedUserLibraryGamesByUserKeycloakIdAsync(string userKeycloakId);
    
    // UserLibraryAddonRepository
    /// <summary>
    /// Adds a new library addon to the user's library.
    /// </summary>
    /// <param name="userLibraryAddonDto">The data transfer object representing the library addon to be added to the user's library.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task AddUserLibraryAddonAsync(UserLibraryAddonDto userLibraryAddonDto);

    /// <summary>
    /// Updates the information of a user library addon with the provided details.
    /// </summary>
    /// <param name="userLibraryAddonDto">The data transfer object containing the updated details of the user library addon.</param>
    /// <returns>A Task representing the completion of the asynchronous operation.</returns>
    Task UpdateUserLibraryAddonAsync(UserLibraryAddonDto userLibraryAddonDto);

    /// <summary>
    /// Deletes the library addon associated with the specified library addon ID.
    /// </summary>
    /// <param name="userLibraryAddonId">The unique identifier of the library addon to delete.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task DeleteUserLibraryAddonAsync(Guid userLibraryAddonId);

    /// <summary>
    /// Restores a user library addon with the specified ID that was previously deleted.
    /// </summary>
    /// <param name="userLibraryAddonId">The unique identifier of the user library addon to restore.</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains the restored UserLibraryAddonDto.</returns>
    Task<UserLibraryAddonDto> RestoreUserLibraryAddonAsync(Guid userLibraryAddonId);

    /// <summary>
    /// Retrieves the user library addon associated with the specified addon ID.
    /// </summary>
    /// <param name="userLibraryAddonId">The unique identifier of the user library addon to retrieve.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains the UserLibraryAddonDto object associated with the specified addon ID.</returns>
    Task<UserLibraryAddonDto> GetUserLibraryAddonByIdAsync(Guid userLibraryAddonId);

    /// <summary>
    /// Retrieves the deleted user library addon associated with the specified ID.
    /// </summary>
    /// <param name="userLibraryAddonId">The unique identifier of the deleted user library addon to retrieve.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains the UserLibraryAddonDto for the deleted addon associated with the specified ID.</returns>
    Task<UserLibraryAddonDto> GetDeletedUserLibraryAddonByIdAsync(Guid userLibraryAddonId);

    /// <summary>
    /// Retrieves a list of all user library addons.
    /// </summary>
    /// <returns>
    /// A Task representing the result of the asynchronous operation. The task result contains a list of UserLibraryAddonDto objects representing all user library addons.
    /// </returns>
    Task<List<UserLibraryAddonDto>> GetAllUserLibraryAddonsAsync();

    /// <summary>
    /// Retrieves a list of all deleted user library addons.
    /// </summary>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of UserLibraryAddonDto for all deleted user library addons.</returns>
    Task<List<UserLibraryAddonDto>> GetAllDeletedUserLibraryAddonsAsync();

    /// <summary>
    /// Retrieves all library addons associated with a specified game ID.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game for which library addons are to be retrieved.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of UserLibraryAddonDto objects associated with the specified game ID.</returns>
    Task<List<UserLibraryAddonDto>> GetAllUserLibraryAddonsByGameIdAsync(Guid gameId);

    /// <summary>
    /// Retrieves all deleted user library addons associated with the specified game ID.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game for which the deleted user library addons are to be retrieved.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of UserLibraryAddonDto objects associated with the specified game ID.</returns>
    Task<List<UserLibraryAddonDto>> GetAllDeletedUserLibraryAddonsByGameIdAsync(Guid gameId);
}