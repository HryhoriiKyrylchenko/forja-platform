namespace Forja.Infrastructure.Repositories.Analytics;

/// <summary>
/// Implementation of IAnalyticsSessionRepository interface.
/// Provides data operations for AnalyticsSessions.
/// </summary>
public class AnalyticsSessionRepository : IAnalyticsSessionRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<AnalyticsSession> _analyticsSessions;

    public AnalyticsSessionRepository(ForjaDbContext context)
    {
        _context = context;
        _analyticsSessions = context.Set<AnalyticsSession>();
    }

    /// <inheritdoc />
    public async Task<AnalyticsSession?> GetByIdAsync(Guid sessionId)
    {
        if (sessionId == Guid.Empty)
        {
            throw new ArgumentException("Invalid session ID.", nameof(sessionId));
        }

        return await _analyticsSessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.SessionId == sessionId);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AnalyticsSession>> GetAllAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _analyticsSessions.AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(s => s.StartTime >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(s => s.EndTime <= endDate.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<int> GetSessionCountAsync(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            throw new ArgumentException("Start date must be before end date.");
        }
        
        return await _context.AnalyticsSessions
            .Where(session => session.StartTime >= startDate && session.EndTime <= endDate)
            .CountAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AnalyticsSession>> GetByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Invalid user ID.", nameof(userId));
        }

        return await _analyticsSessions
            .Include(s => s.User)
            .Where(s => s.UserId == userId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<AnalyticsSession?> AddAsync(AnalyticsSession analyticsSession)
    {
        if (!AnalyticsModelValidator.ValidateAnalyticsSessionModel(analyticsSession, out string? error))
        {
            throw new ArgumentException(error);
        }

        await _analyticsSessions.AddAsync(analyticsSession);
        await _context.SaveChangesAsync();
        
        return analyticsSession;
    }

    /// <inheritdoc />
    public async Task<AnalyticsSession?> UpdateAsync(AnalyticsSession analyticsSession)
    {
        if (!AnalyticsModelValidator.ValidateAnalyticsSessionModel(analyticsSession, out string? error))
        {
            throw new ArgumentException(error);
        }

        _analyticsSessions.Update(analyticsSession);
        await _context.SaveChangesAsync();
        
        return analyticsSession;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid sessionId)
    {
        if (sessionId == Guid.Empty)
        {
            throw new ArgumentException("Invalid session ID.", nameof(sessionId));
        }

        var session = await _analyticsSessions.FirstOrDefaultAsync(s => s.SessionId == sessionId);

        if (session == null)
        {
            throw new KeyNotFoundException($"Analytics session with ID {sessionId} not found.");
        }

        _analyticsSessions.Remove(session);
        await _context.SaveChangesAsync();
    }
}