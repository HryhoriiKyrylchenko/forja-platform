namespace Forja.Application.Interfaces.Support;

/// <summary>
/// Service interface for managing SupportTicket entities.
/// </summary>
public interface ISupportTicketService
{
    /// <summary>
    /// Gets all support tickets.
    /// </summary>
    /// <returns>A list of support ticket DTOs.</returns>
    Task<IEnumerable<SupportTicketDto>> GetAllAsync();

    /// <summary>
    /// Gets a specific support ticket by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the support ticket.</param>
    /// <returns>The support ticket DTO, or null if not found.</returns>
    Task<SupportTicketDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets all support tickets created by a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of support ticket DTOs created by the user.</returns>
    Task<IEnumerable<SupportTicketDto>> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Creates a new support ticket.
    /// </summary>
    /// <param name="request">The request object containing data to create the support ticket.</param>
    /// <returns>The created support ticket DTO.</returns>
    Task<SupportTicketDto?> CreateAsync(SupportTicketCreateRequest request);

    /// <summary>
    /// Updates the status of a support ticket.
    /// </summary>
    /// <param name="request">The request object containing ticket ID and updated status.</param>
    /// <returns>The updated support ticket DTO.</returns>
    Task<SupportTicketDto?> UpdateStatusAsync(SupportTicketUpdateStatusRequest request);

    /// <summary>
    /// Deletes a support ticket by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the support ticket to delete.</param>
    Task DeleteAsync(Guid id);
}