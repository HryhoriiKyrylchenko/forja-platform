namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Interface for managing genre entities in the repository.
/// </summary>
public interface IGenreRepository
{
    /// <summary>
    /// Gets all genres.
    /// </summary>
    /// <returns>A collection of all genres.</returns>
    Task<IEnumerable<Genre>> GetAllAsync();

    /// <summary>
    /// Gets all deleted genres.
    /// </summary>
    /// <returns>A collection of deleted genres.</returns>
    Task<IEnumerable<Genre>> GetAllDeletedAsync();

    /// <summary>
    /// Gets a genre by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the genre.</param>
    /// <returns>The genre with the specified ID, or null if not found.</returns>
    Task<Genre?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new genre to the repository.
    /// </summary>
    /// <param name="genre">The genre to add.</param>
    /// <returns>The added genre.</returns>
    Task<Genre?> AddAsync(Genre genre);

    /// <summary>
    /// Updates an existing genre in the repository.
    /// </summary>
    /// <param name="genre">The genre with updated information.</param>
    /// <returns>The updated genre.</returns>
    Task<Genre?> UpdateAsync(Genre genre);

    /// <summary>
    /// Deletes a genre by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the genre to delete.</param>
    /// <returns>A task representing the operation.</returns>
    Task DeleteAsync(Guid id);
}