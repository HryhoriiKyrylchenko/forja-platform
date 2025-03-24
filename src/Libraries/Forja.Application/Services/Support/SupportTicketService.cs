namespace Forja.Application.Services.Support;

/// <summary>
/// Service implementation for managing SupportTicket entities.
/// </summary>
public class SupportTicketService : ISupportTicketService
{
    private readonly ISupportTicketRepository _ticketRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="SupportTicketService"/> class.
    /// </summary>
    /// <param name="ticketRepository">The repository for handling support ticket data.</param>
    public SupportTicketService(ISupportTicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SupportTicketDto>> GetAllAsync()
    {
        var tickets = await _ticketRepository.GetAllAsync();
        return tickets.Select(SupportEntityToDtoMapper.MapToSupportTicketDto);
    }

    /// <inheritdoc />
    public async Task<SupportTicketDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid SupportTicket ID.", nameof(id));
        }

        var ticket = await _ticketRepository.GetByIdAsync(id);
        return ticket == null ? null : SupportEntityToDtoMapper.MapToSupportTicketDto(ticket);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SupportTicketDto>> GetByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Invalid User ID.", nameof(userId));
        }

        var tickets = await _ticketRepository.GetByUserIdAsync(userId);
        return tickets.Select(SupportEntityToDtoMapper.MapToSupportTicketDto);
    }

    /// <inheritdoc />
    public async Task<SupportTicketDto?> CreateAsync(SupportTicketCreateRequest request)
    {
        if (!SupportRequestsValidator.ValidateSupportTicketCreateRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(request));
        }

        var ticket = new SupportTicket
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Subject = request.Subject,
            Description = request.Description,
            Status = SupportTicketStatus.Open,
            CreatedAt = DateTime.UtcNow
        };

        var createdTicket = await _ticketRepository.AddAsync(ticket);
        return createdTicket == null ? null : SupportEntityToDtoMapper.MapToSupportTicketDto(createdTicket);
    }

    /// <inheritdoc />
    public async Task<SupportTicketDto?> UpdateStatusAsync(SupportTicketUpdateStatusRequest request)
    {
        if (!SupportRequestsValidator.ValidateSupportTicketUpdateStatusRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(request));
        }

        var existingTicket = await _ticketRepository.GetByIdAsync(request.Id);
        if (existingTicket == null)
        {
            throw new KeyNotFoundException("SupportTicket not found.");
        }

        existingTicket.Status = request.Status;
        existingTicket.UpdatedAt = DateTime.UtcNow;

        var updatedTicket = await _ticketRepository.UpdateAsync(existingTicket);
        return updatedTicket == null ? null : SupportEntityToDtoMapper.MapToSupportTicketDto(updatedTicket);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid SupportTicket ID.", nameof(id));
        }

        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null)
        {
            throw new KeyNotFoundException("SupportTicket not found.");
        }

        await _ticketRepository.DeleteAsync(id);
    }
}