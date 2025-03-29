namespace Forja.Infrastructure.Repositories.Analytics;

/// <summary>
/// Implementation of the IAnalyticsEventRepository interface.
/// Provides data operations for AnalyticsEvents.
/// </summary>
public class AnalyticsEventRepository : IAnalyticsEventRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<AnalyticsEvent> _analyticsEvents;

    public AnalyticsEventRepository(ForjaDbContext context)
    {
        _context = context;
        _analyticsEvents = context.Set<AnalyticsEvent>();
    }

    /// <inheritdoc />
    public async Task<AnalyticsEvent?> GetByIdAsync(Guid eventId)
    {
        if (eventId == Guid.Empty)
        {
            throw new ArgumentException("Invalid analytics event ID.", nameof(eventId));
        }

        return await _analyticsEvents
            .FirstOrDefaultAsync(e => e.Id == eventId);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AnalyticsEvent>> GetAllAsync(DateTime? startDate = null, DateTime? endDate = null, AnalyticEventType? eventType = null)
    {
        var query = _analyticsEvents.AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(e => e.Timestamp >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(e => e.Timestamp <= endDate.Value);
        }

        if (eventType.HasValue)
        {
            query = query.Where(e => e.EventType == eventType.Value);
        }

        return await query.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AnalyticsEvent>> GetByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Invalid user ID.", nameof(userId));
        }

        return await _analyticsEvents
            .Where(e => e.UserId == userId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<AnalyticsEvent?> AddAsync(AnalyticsEvent analyticsEvent)
    {
        if (!AnalyticsModelValidator.ValidateAnalyticsEventModel(analyticsEvent, out string? error))
        {
            throw new ArgumentException(error);
        }

        await _analyticsEvents.AddAsync(analyticsEvent);
        await _context.SaveChangesAsync();
        
        return analyticsEvent;
    }

    /// <inheritdoc />
    public async Task<AnalyticsEvent?> UpdateAsync(AnalyticsEvent analyticsEvent)
    {
        if (!AnalyticsModelValidator.ValidateAnalyticsEventModel(analyticsEvent, out string? error))
        {
            throw new ArgumentException(error);
        }

        _analyticsEvents.Update(analyticsEvent);
        await _context.SaveChangesAsync();
        
        return analyticsEvent;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid eventId)
    {
        if (eventId == Guid.Empty)
        {
            throw new ArgumentException("Invalid analytics event ID.", nameof(eventId));
        }

        var analyticsEvent = await _analyticsEvents.FirstOrDefaultAsync(e => e.Id == eventId);

        if (analyticsEvent == null)
        {
            throw new KeyNotFoundException($"Analytics event with ID {eventId} not found.");
        }

        _analyticsEvents.Remove(analyticsEvent);
        await _context.SaveChangesAsync();
    }
}