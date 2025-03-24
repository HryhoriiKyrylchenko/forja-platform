namespace Forja.Application.Services.Support;

/// <summary>
/// Service implementation for managing TicketMessage entities.
/// </summary>
public class TicketMessageService : ITicketMessageService
{
    private readonly ITicketMessageRepository _messageRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="TicketMessageService"/> class.
    /// </summary>
    /// <param name="messageRepository">The repository for handling TicketMessage data operations.</param>
    public TicketMessageService(ITicketMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TicketMessageDto>> GetAllAsync()
    {
        var messages = await _messageRepository.GetAllAsync();
        return messages.Select(SupportEntityToDtoMapper.MapToTicketMessageDto);
    }

    /// <inheritdoc />
    public async Task<TicketMessageDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid TicketMessage ID.", nameof(id));
        }

        var message = await _messageRepository.GetByIdAsync(id);
        return message == null ? null : SupportEntityToDtoMapper.MapToTicketMessageDto(message);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TicketMessageDto>> GetBySupportTicketIdAsync(Guid supportTicketId)
    {
        if (supportTicketId == Guid.Empty)
        {
            throw new ArgumentException("Invalid SupportTicket ID.", nameof(supportTicketId));
        }

        var messages = await _messageRepository.GetBySupportTicketIdAsync(supportTicketId);
        return messages.Select(SupportEntityToDtoMapper.MapToTicketMessageDto);
    }

    /// <inheritdoc />
    public async Task<TicketMessageDto?> CreateAsync(TicketMessageCreateRequest request)
    {
        if (!SupportRequestsValidator.ValidateTicketMessageCreateRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(request));
        }

        var message = new TicketMessage
        {
            Id = Guid.NewGuid(),
            SupportTicketId = request.SupportTicketId,
            SenderId = request.SenderId,
            IsSupportAgent = false, 
            Message = request.Message,
            SentAt = DateTime.UtcNow
        };

        var createdMessage = await _messageRepository.AddAsync(message);
        return createdMessage == null ? null : SupportEntityToDtoMapper.MapToTicketMessageDto(createdMessage);
    }

    /// <inheritdoc />
    public async Task<TicketMessageDto?> UpdateAsync(TicketMessageUpdateRequest request)
    {
        if (!SupportRequestsValidator.ValidateTicketMessageUpdateRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(request));
        }

        var existingMessage = await _messageRepository.GetByIdAsync(request.Id);
        if (existingMessage == null)
        {
            throw new KeyNotFoundException("TicketMessage not found.");
        }

        existingMessage.Message = request.Message;
        existingMessage.IsSupportAgent = request.IsSupportAgent;

        var updatedMessage = await _messageRepository.UpdateAsync(existingMessage);
        return updatedMessage == null ? null : SupportEntityToDtoMapper.MapToTicketMessageDto(updatedMessage);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid TicketMessage ID.", nameof(id));
        }

        var message = await _messageRepository.GetByIdAsync(id);
        if (message == null)
        {
            throw new KeyNotFoundException("TicketMessage not found.");
        }

        await _messageRepository.DeleteAsync(id);
    }
}