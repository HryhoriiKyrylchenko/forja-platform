namespace Forja.Application.Interfaces.Support;

/// <summary>
/// Interface for managing TicketMessage entities.
/// </summary>
public interface ITicketMessageService
{
    /// <summary>
    /// Gets all ticket messages.
    /// </summary>
    /// <returns>A list of ticket message DTOs.</returns>
    Task<IEnumerable<TicketMessageDto>> GetAllAsync();

    /// <summary>
    /// Gets a specific ticket message by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the ticket message.</param>
    /// <returns>A ticket message DTO or null if not found.</returns>
    Task<TicketMessageDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets all ticket messages for a specific support ticket.
    /// </summary>
    /// <param name="supportTicketId">The unique identifier of the support ticket.</param>
    /// <returns>A list of ticket message DTOs for the specified support ticket.</returns>
    Task<IEnumerable<TicketMessageDto>> GetBySupportTicketIdAsync(Guid supportTicketId);

    /// <summary>
    /// Creates a new ticket message.
    /// </summary>
    /// <param name="request">The request to create the ticket message.</param>
    /// <returns>The created ticket message DTO.</returns>
    Task<TicketMessageDto?> CreateAsync(TicketMessageCreateRequest request);

    /// <summary>
    /// Updates an existing ticket message.
    /// </summary>
    /// <param name="request">The request to update the ticket message.</param>
    /// <returns>The updated ticket message DTO.</returns>
    Task<TicketMessageDto?> UpdateAsync(TicketMessageUpdateRequest request);

    /// <summary>
    /// Deletes a ticket message by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the ticket message to delete.</param>
    Task DeleteAsync(Guid id);
}