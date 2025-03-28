namespace Forja.Application.Services.Analytics;

/// <summary>
/// Provides services for managing, querying, and storing analytics events.
/// </summary>
public class AnalyticsEventService : IAnalyticsEventService
{
    private readonly IAnalyticsEventRepository _analyticsEventRepository;

    public AnalyticsEventService(IAnalyticsEventRepository analyticsEventRepository)
    {
        _analyticsEventRepository = analyticsEventRepository;
    }

    /// <inheritdoc />
    public async Task<AnalyticsEventDto?> GetByIdAsync(Guid eventId)
    {
        if (eventId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(eventId));
        }
        var analyticsEvent = await _analyticsEventRepository.GetByIdAsync(eventId);
        return analyticsEvent == null ? null : AnalyticsEntityToDtoMapper.MapToAnalyticsEventDto(analyticsEvent);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AnalyticsEventDto>> GetAllAsync()
    {
        var analyticsEvents = await _analyticsEventRepository.GetAllAsync();
        return analyticsEvents.Select(AnalyticsEntityToDtoMapper.MapToAnalyticsEventDto);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AnalyticsEventDto>> GetByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(userId));
        }
        var analyticsEvents = await _analyticsEventRepository.GetByUserIdAsync(userId);
        return analyticsEvents.Select(AnalyticsEntityToDtoMapper.MapToAnalyticsEventDto);
    }

    /// <inheritdoc />
    public async Task<AnalyticsEventDto?> AddEventAsync(AnalyticEventType eventType,
        Guid? userId,
        Dictionary<string, string> metadata)
    {
        if (userId != null && userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        }

        var analyticsEvent = new AnalyticsEvent
        {
            Id = Guid.NewGuid(),
            EventType = eventType,
            UserId = userId,
            Timestamp = DateTime.UtcNow,
            Metadata = metadata
        };
        var addedEvent = await _analyticsEventRepository.AddAsync(analyticsEvent);

        return addedEvent == null ? null : AnalyticsEntityToDtoMapper.MapToAnalyticsEventDto(addedEvent);
    }

    /// <inheritdoc />
    public async Task<AnalyticsEventDto?> UpdateEventAsync(AnalyticsEventUpdateRequest request)
    {
        if (!AnalyticsRequestsValidator.ValidateAnalyticsEventUpdateRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(request));
        }
        
        var analyticsEvent = await _analyticsEventRepository.GetByIdAsync(request.Id);
        if (analyticsEvent == null)
        {
            throw new KeyNotFoundException($"Analytics event with ID {request.Id} not found.");
        }
        
        analyticsEvent.EventType = request.EventType;
        analyticsEvent.UserId = request.UserId;
        analyticsEvent.Metadata = request.Metadata;
        
        var updatedEvent = await _analyticsEventRepository.UpdateAsync(analyticsEvent);

        return updatedEvent == null ? null : AnalyticsEntityToDtoMapper.MapToAnalyticsEventDto(updatedEvent);
    }

    /// <inheritdoc />
    public async Task DeleteEventAsync(Guid eventId)
    {
        if (eventId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(eventId));
        }
        await _analyticsEventRepository.DeleteAsync(eventId);
    }
}