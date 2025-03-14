namespace Forja.Application.Interfaces.Games;

/// <summary>
/// Provides an interface for managing genre-related operations.
/// </summary>
public interface IGenreService
{
    /// <summary>
    /// Asynchronously retrieves all genres.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of GenreDto objects.</returns>
    Task<IEnumerable<GenreDto>> GetAllAsync();

    /// <summary>
    /// Retrieves all deleted genres asynchronously.
    /// </summary>
    /// <returns>An enumerable collection of GenreDto objects representing all deleted genres.</returns>
    Task<IEnumerable<GenreDto>> GetAllDeletedAsync();

    /// <summary>
    /// Retrieves a genre by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the genre to retrieve.</param>
    /// <returns>A <see cref="GenreDto"/> representing the genre if found, or null if the genre does not exist.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided <paramref name="id"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when a genre with the given <paramref name="id"/> is not found.</exception>
    Task<GenreDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Creates a new genre based on the provided request data.
    /// </summary>
    /// <param name="request">The data required to create a genre, including its name.</param>
    /// <returns>
    /// Returns a <see cref="GenreDto"/> representing the newly created genre,
    /// or null if the genre could not be created.
    /// </returns>
    Task<GenreDto?> CreateAsync(GenreCreateRequest request);

    /// <summary>
    /// Updates an existing genre with the provided updated details.
    /// </summary>
    /// <param name="request">The request object containing the updated genre details, including its ID.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated genre as a <see cref="GenreDto"/>, or null if the update fails.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided <paramref name="request"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if a genre with the specified ID does not exist in the repository.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the update operation fails unexpectedly.</exception>
    Task<GenreDto?> UpdateAsync(GenreUpdateRequest request);

    /// <summary>
    /// Deletes the genre entity associated with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the genre to be deleted.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid id);
}