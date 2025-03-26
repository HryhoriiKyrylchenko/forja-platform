namespace Forja.Infrastructure.Repositories.Analytics;

/// <summary>
/// Implementation of the IAuditLogRepository interface.
/// Provides data operations for AuditLogs.
/// </summary>
public class AuditLogRepository : IAuditLogRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<AuditLog> _auditLogs;

    public AuditLogRepository(ForjaDbContext context)
    {
        _context = context;
        _auditLogs = context.Set<AuditLog>();
    }

    /// <inheritdoc />
    public async Task<AuditLog?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid audit log ID.", nameof(id));
        }

        return await _auditLogs
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AuditLog>> GetAllAsync()
    {
        return await _auditLogs
            .Include(a => a.User)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Invalid user ID.", nameof(userId));
        }

        return await _auditLogs
            .Include(a => a.User)
            .Where(a => a.UserId == userId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<AuditLog?> AddAsync(AuditLog auditLog)
    {
        if (!AnalyticsModelValidator.ValidateAuditLogModel(auditLog, out string? error))
        {
            throw new ArgumentException(error);
        }

        await _auditLogs.AddAsync(auditLog);
        await _context.SaveChangesAsync();
        
        return auditLog;
    }

    /// <inheritdoc />
    public async Task<AuditLog?> UpdateAsync(AuditLog auditLog)
    {
        if (!AnalyticsModelValidator.ValidateAuditLogModel(auditLog, out string? error))
        {
            throw new ArgumentException(error);
        }

        _auditLogs.Update(auditLog);
        await _context.SaveChangesAsync();
        
        return auditLog;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid audit log ID.", nameof(id));
        }

        var auditLog = await _auditLogs.FirstOrDefaultAsync(a => a.Id == id);

        if (auditLog == null)
        {
            throw new KeyNotFoundException($"Audit log with ID {id} not found.");
        }

        _auditLogs.Remove(auditLog);
        await _context.SaveChangesAsync();
    }
}