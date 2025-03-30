namespace Forja.Application.Services.Analytics;

/// <summary>
/// Service for managing and handling operations related to analytics sessions.
/// </summary>
public class AnalyticsSessionService : IAnalyticsSessionService
{
    private readonly IAnalyticsSessionRepository _analyticsSessionRepository;

    public AnalyticsSessionService(IAnalyticsSessionRepository analyticsSessionRepository)
    {
        _analyticsSessionRepository = analyticsSessionRepository;
    }

    /// <inheritdoc />
    public async Task<AnalyticsSessionDto?> GetByIdAsync(Guid sessionId)
    {
        if (sessionId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(sessionId));
        }
        var analyticsSession = await _analyticsSessionRepository.GetByIdAsync(sessionId);
        return analyticsSession == null ? null : AnalyticsEntityToDtoMapper.MapToAnalyticsSessionDto(analyticsSession);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AnalyticsSessionDto>> GetAllAsync()
    {
        var analyticsSessions = await _analyticsSessionRepository.GetAllAsync();
        return analyticsSessions.Select(AnalyticsEntityToDtoMapper.MapToAnalyticsSessionDto);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AnalyticsSessionDto>> GetByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(userId));
        }
        var analyticsSessions = await _analyticsSessionRepository.GetByUserIdAsync(userId);
        return analyticsSessions.Select(AnalyticsEntityToDtoMapper.MapToAnalyticsSessionDto);
    }

    /// <inheritdoc />
    public async Task<AnalyticsSessionDto?> AddSessionAsync(Guid? userId, Dictionary<string, string> metadata)
    {
        if (userId != null && userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        }

        if (metadata == null)
        {
            throw new ArgumentNullException(nameof(metadata));
        }
        
        var sessionEntity = new AnalyticsSession
        {
            SessionId = Guid.NewGuid(),
            UserId = userId,
            StartTime = DateTime.UtcNow,
            Metadata = metadata
        };

        var addedSession = await _analyticsSessionRepository.AddAsync(sessionEntity);
        return addedSession == null ? null : AnalyticsEntityToDtoMapper.MapToAnalyticsSessionDto(addedSession);
    }

    /// <inheritdoc />
    public async Task EndSessionAsync(Guid sessionId)
    {
        if (sessionId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(sessionId));
        }
        
        var session = await _analyticsSessionRepository.GetByIdAsync(sessionId);
        if (session == null)
        {
            throw new KeyNotFoundException($"Session with ID {sessionId} not found.");
        }

        session.EndTime = DateTime.UtcNow;

        await _analyticsSessionRepository.UpdateAsync(session);
    }

    /// <inheritdoc />
    public async Task DeleteSessionAsync(Guid sessionId)
    {
        if (sessionId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(sessionId));
        }
        await _analyticsSessionRepository.DeleteAsync(sessionId);
    }
}