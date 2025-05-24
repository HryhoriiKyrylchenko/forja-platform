namespace Forja.Application.Interfaces.Support;

/// <summary>
/// Interface for managing FAQ entities.
/// </summary>
public interface IFAQService
{
    /// <summary>
    /// Gets all FAQs.
    /// </summary>
    /// <returns>A list of FAQs.</returns>
    Task<IEnumerable<FAQDto>> GetAllAsync();

    /// <summary>
    /// Gets a specific FAQ by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the FAQ.</param>
    /// <returns>The FAQ object, or null if not found.</returns>
    Task<FAQDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Creates a new FAQ.
    /// </summary>
    /// <param name="request">The FAQ creation request containing the question, answer, and order.</param>
    /// <returns>The created FAQ.</returns>
    Task<FAQDto?> CreateAsync(FAQCreateRequest request);

    /// <summary>
    /// Updates an existing FAQ.
    /// </summary>
    /// <param name="request">The FAQ update request containing the ID, updated question, answer, and order.</param>
    /// <returns>The updated FAQ.</returns>
    Task<FAQDto?> UpdateAsync(FAQUpdateRequest request);

    /// <summary>
    /// Deletes an FAQ by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the FAQ to delete.</param>
    Task DeleteAsync(Guid id);
}