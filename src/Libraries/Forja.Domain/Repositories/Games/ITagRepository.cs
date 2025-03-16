namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Interface for managing operations related to tags.
/// </summary>
public interface ITagRepository
{
    /// <summary>
    /// Gets all tags.
    /// </summary>
    /// <returns>A collection of all tags.</returns>
    Task<IEnumerable<Tag>> GetAllAsync();

    /// <summary>
    /// Gets a tag by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the tag.</param>
    /// <returns>The tag with the specified ID, or null if not found.</returns>
    Task<Tag?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new tag to the repository.
    /// </summary>
    /// <param name="tag">The tag to add.</param>
    /// <returns>The added tag.</returns>
    Task<Tag?> AddAsync(Tag tag);

    /// <summary>
    /// Updates an existing tag in the repository.
    /// </summary>
    /// <param name="tag">The tag with updated details.</param>
    /// <returns>The updated tag.</returns>
    Task<Tag?> UpdateAsync(Tag tag);

    /// <summary>
    /// Deletes a tag by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the tag to delete.</param>
    /// <returns>A task representing the operation.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Gets all tags, including their associated GameTags.
    /// </summary>
    /// <returns>A collection of tags including their associated GameTags.</returns>
    Task<IEnumerable<Tag>> GetAllWithDetailsAsync();
}