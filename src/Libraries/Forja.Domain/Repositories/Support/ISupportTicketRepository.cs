namespace Forja.Domain.Repositories.Support;

/// <summary>
/// Repository interface for managing SupportTicket entities.
/// </summary>
public interface ISupportTicketRepository
{
    /// <summary>
    /// Gets all support tickets from the database.
    /// </summary>
    /// <returns>A list of support tickets.</returns>
    Task<IEnumerable<SupportTicket>> GetAllAsync();

    /// <summary>
    /// Gets a specific support ticket by its Id.
    /// </summary>
    /// <param name="id">The unique identifier of the support ticket.</param>
    /// <returns>The SupportTicket object, or null if not found.</returns>
    Task<SupportTicket?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets all support tickets created by a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of support tickets created by the user.</returns>
    Task<IEnumerable<SupportTicket>> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Adds a new support ticket to the database.
    /// </summary>
    /// <param name="ticket">The SupportTicket entity to add.</param>
    /// <returns>The created support ticket with its generated Id.</returns>
    Task<SupportTicket?> AddAsync(SupportTicket ticket);

    /// <summary>
    /// Updates an existing support ticket.
    /// </summary>
    /// <param name="ticket">The SupportTicket entity with updated values.</param>
    /// <returns>The updated support ticket, or null if not found.</returns>
    Task<SupportTicket?> UpdateAsync(SupportTicket ticket);

    /// <summary>
    /// Deletes a support ticket by its Id.
    /// </summary>
    /// <param name="id">The unique identifier of the support ticket to delete.</param>
    Task DeleteAsync(Guid id);
}