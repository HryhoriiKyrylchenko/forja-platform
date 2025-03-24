namespace Forja.Domain.Repositories.Support;

/// <summary>
/// Repository interface for managing TicketMessage entities.
/// </summary>
public interface ITicketMessageRepository
{
    /// <summary>
    /// Gets all ticket messages from the database.
    /// </summary>
    /// <returns>A list of ticket messages.</returns>
    Task<IEnumerable<TicketMessage>> GetAllAsync();

    /// <summary>
    /// Gets a specific ticket message by its Id.
    /// </summary>
    /// <param name="id">The unique identifier of the ticket message.</param>
    /// <returns>The TicketMessage object, or null if not found.</returns>
    Task<TicketMessage?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets all ticket messages for a specific support ticket.
    /// </summary>
    /// <param name="supportTicketId">The unique identifier of the support ticket.</param>
    /// <returns>A list of ticket messages for the specified support ticket.</returns>
    Task<IEnumerable<TicketMessage>> GetBySupportTicketIdAsync(Guid supportTicketId);

    /// <summary>
    /// Adds a new ticket message to the database.
    /// </summary>
    /// <param name="ticketMessage">The TicketMessage entity to add.</param>
    /// <returns>The created ticket message with its generated Id.</returns>
    Task<TicketMessage?> AddAsync(TicketMessage ticketMessage);

    /// <summary>
    /// Updates an existing ticket message.
    /// </summary>
    /// <param name="ticketMessage">The TicketMessage entity with updated values.</param>
    /// <returns>The updated ticket message, or null if not found.</returns>
    Task<TicketMessage?> UpdateAsync(TicketMessage ticketMessage);

    /// <summary>
    /// Deletes a ticket message by its Id.
    /// </summary>
    /// <param name="id">The unique identifier of the ticket message to delete.</param>
    Task DeleteAsync(Guid id);
}