namespace Forja.Application.Interfaces.UserProfile;

/// <summary>
/// Represents a service for managing user library games and addons.
/// </summary>
public interface IUserLibraryService
{
    // UserLibraryGameRepository
    /// <summary>
    /// Adds a game to the user's library.
    /// </summary>
    /// <param name="request">The UserLibraryGameCreateRequest object that represents the game to be added to the user's library.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task<UserLibraryGameDto?> AddUserLibraryGameAsync(UserLibraryGameCreateRequest request);

    /// <summary>
    /// Updates the details of a user's library game entry.
    /// </summary>
    /// <param name="request">The UserLibraryGameUpdateRequest object containing updated information for the user library game entry.</param>
    /// <returns>A Task representing the result of the asynchronous operation.</returns>
    Task<UserLibraryGameDto?> UpdateUserLibraryGameAsync(UserLibraryGameUpdateRequest request);

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
    Task<UserLibraryGameDto?> RestoreUserLibraryGameAsync(Guid userLibraryGameId);

    /// <summary>
    /// Retrieves the user library game associated with the specified ID.
    /// </summary>
    /// <param name="userLibraryGameId">The unique identifier of the user library game to retrieve.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains the UserLibraryGameDto for the user library game associated with the specified ID.</returns>
    Task<UserLibraryGameDto?> GetUserLibraryGameByIdAsync(Guid userLibraryGameId);

    /// <summary>
    /// Retrieves the deleted user library game associated with the specified ID.
    /// </summary>
    /// <param name="userLibraryGameId">The unique identifier of the deleted user library game to retrieve.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains the UserLibraryGameDto for the deleted user library game associated with the specified ID.</returns>
    Task<UserLibraryGameDto?> GetDeletedUserLibraryGameByIdAsync(Guid userLibraryGameId);

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
    /// <param name="userId">The unique  ID of the user whose library games are to be retrieved.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of UserLibraryGameDto objects for the library games associated with the specified user's Keycloak ID.</returns>
    Task<List<UserLibraryGameDto>> GetAllUserLibraryGamesByUserIdAsync(Guid userId);

    /// <summary>
    /// Retrieves all deleted user library games associated with the specified Keycloak ID.
    /// </summary>
    /// <param name="userId">The unique ID of the user whose deleted library games are to be retrieved.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of UserLibraryGameDto objects for the deleted library games associated with the specified Keycloak ID.</returns>
    Task<List<UserLibraryGameDto>> GetAllDeletedUserLibraryGamesByUserIdAsync(Guid userId);
    
    // UserLibraryAddonRepository
    /// <summary>
    /// Adds a new library addon to the user's library.
    /// </summary>
    /// <param name="request">The UserLibraryAddonCreateRequest object representing the library addon to be added to the user's library.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task<UserLibraryAddonDto?> AddUserLibraryAddonAsync(UserLibraryAddonCreateRequest request);

    /// <summary>
    /// Updates the information of a user library addon with the provided details.
    /// </summary>
    /// <param name="request">The UserLibraryAddonUpdateRequest object containing the updated details of the user library addon.</param>
    /// <returns>A Task representing the completion of the asynchronous operation.</returns>
    Task<UserLibraryAddonDto?> UpdateUserLibraryAddonAsync(UserLibraryAddonUpdateRequest request);

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
    Task<UserLibraryAddonDto?> RestoreUserLibraryAddonAsync(Guid userLibraryAddonId);

    /// <summary>
    /// Retrieves the user library addon associated with the specified addon ID.
    /// </summary>
    /// <param name="userLibraryAddonId">The unique identifier of the user library addon to retrieve.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains the UserLibraryAddonDto object associated with the specified addon ID.</returns>
    Task<UserLibraryAddonDto?> GetUserLibraryAddonByIdAsync(Guid userLibraryAddonId);

    /// <summary>
    /// Retrieves the deleted user library addon associated with the specified ID.
    /// </summary>
    /// <param name="userLibraryAddonId">The unique identifier of the deleted user library addon to retrieve.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains the UserLibraryAddonDto for the deleted addon associated with the specified ID.</returns>
    Task<UserLibraryAddonDto?> GetDeletedUserLibraryAddonByIdAsync(Guid userLibraryAddonId);

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