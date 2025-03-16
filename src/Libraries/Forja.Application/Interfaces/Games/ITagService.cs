namespace Forja.Application.Interfaces.Games;

/// <summary>
/// Represents a contract for managing tags in the application.
/// </summary>
public interface ITagService
{
    /// <summary>
    /// Retrieves all tags asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains a collection of TagDto objects.</returns>
    Task<IEnumerable<TagDto>> GetAllAsync();

    /// <summary>
    /// Retrieves a tag by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the tag to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains the tag data transfer object if found;
    /// otherwise, null if the tag does not exist.</returns>
    Task<TagDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Creates a new tag based on the provided request data.
    /// </summary>
    /// <param name="request">The request object containing the data needed to create the tag.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created tag as a <see cref="TagDto"/>. Returns null if creation fails.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided request data is invalid.</exception>
    Task<TagDto?> CreateAsync(TagCreateRequest request);

    /// <summary>
    /// Updates an existing tag in the system with the provided details.
    /// </summary>
    /// <param name="request">
    /// An instance of <see cref="TagUpdateRequest"/> containing the tag's ID and updated title.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous operation.
    /// If the update is successful, returns an updated <see cref="TagDto"/> object.
    /// Returns null if the update fails.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the <paramref name="request"/> is invalid.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when the tag with the specified ID is not found.
    /// </exception>
    Task<TagDto?> UpdateAsync(TagUpdateRequest request);

    /// <summary>
    /// Deletes a tag by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the tag to be deleted.</param>
    /// <exception cref="ArgumentException">Thrown when the provided id is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when a tag with the specified id cannot be found.</exception>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid id);
}