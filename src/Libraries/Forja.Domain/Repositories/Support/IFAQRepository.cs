namespace Forja.Domain.Repositories.Support;

/// <summary>
/// Repository interface for managing FAQ entities.
/// </summary>
public interface IFAQRepository
{
    /// <summary>
    /// Gets all FAQs from the database.
    /// </summary>
    /// <returns>A list of FAQs.</returns>
    Task<IEnumerable<FAQ>> GetAllAsync();

    /// <summary>
    /// Gets a specific FAQ by its Id.
    /// </summary>
    /// <param name="id">The unique identifier of the FAQ.</param>
    /// <returns>The FAQ object, or null if not found.</returns>
    Task<FAQ?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new FAQ to the database.
    /// </summary>
    /// <param name="faq">The FAQ entity to add.</param>
    Task<FAQ?> AddAsync(FAQ faq);

    /// <summary>
    /// Updates an existing FAQ.
    /// </summary>
    /// <param name="faq">The FAQ entity with updated values.</param>
    Task<FAQ?> UpdateAsync(FAQ faq);

    /// <summary>
    /// Deletes an FAQ by its Id.
    /// </summary>
    /// <param name="id">The unique identifier of the FAQ to delete.</param>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Reorders the FAQs based on the provided ordering rules.
    /// </summary>
    /// <param name="orderedFaqs">The reordered list of FAQs.</param>
    Task ReorderAsync(IEnumerable<FAQ> orderedFaqs);
}