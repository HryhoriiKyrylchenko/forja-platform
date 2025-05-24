namespace Forja.Application.Interfaces.Games;

/// <summary>
/// Provides an interface for managing relationships between games and tags.
/// </summary>
public interface IGameTagService
{
    /// <summary>
    /// Asynchronously retrieves all game tag entries.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// an enumerable of <see cref="GameTagDto"/> representing all game tags.
    /// </returns>
    Task<IEnumerable<GameTagDto>> GetAllAsync();

    /// <summary>
    /// Retrieves a <see cref="GameTagDto"/> by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the GameTag to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="GameTagDto"/> if found; otherwise, null.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided id is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no GameTag is found with the specified id.</exception>
    Task<GameTagDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all game tags associated with a specific game identified by its ID.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game for which the tags should be retrieved.</param>
    /// <returns>A task representing the asynchronous operation, containing a collection of <see cref="GameTagDto"/> objects associated with the specified game.</returns>
    Task<IEnumerable<GameTagDto>> GetByGameIdAsync(Guid gameId);

    /// <summary>
    /// Retrieves a collection of GameTagDto objects that are associated with the specified tag ID.
    /// </summary>
    /// <param name="tagId">The unique identifier of the tag.</param>
    /// <returns>A task representing the asynchronous operation.
    /// The task result contains an enumerable collection of GameTagDto objects associated with the specified tag ID.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided tag ID is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no game tags are found for the specified tag ID.</exception>
    Task<IEnumerable<GameTagDto>> GetByTagIdAsync(Guid tagId);

    /// <summary>
    /// Creates a new association between a game and a tag.
    /// </summary>
    /// <param name="request">The request containing details for creating the game-tag association.</param>
    /// <returns>A <see cref="GameTagDto"/> representing the newly created game-tag association, or null if creation failed.</returns>
    Task<GameTagDto?> CreateAsync(GameTagCreateRequest request);

    /// <summary>
    /// Updates an existing game tag based on the provided update request.
    /// </summary>
    /// <param name="request">The request containing the Id of the game tag to update and the updated game and tag IDs.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated game tag as a <see cref="GameTagDto"/>, or null if the update operation failed.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided request is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if a game tag with the specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the update operation fails.</exception>
    Task<GameTagDto?> UpdateAsync(GameTagUpdateRequest request);

    /// <summary>
    /// Deletes a game tag entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game tag to delete.</param>
    /// <exception cref="ArgumentException">Thrown when the provided <paramref name="id"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no game tag is found with the specified <paramref name="id"/>.</exception>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid id);
}