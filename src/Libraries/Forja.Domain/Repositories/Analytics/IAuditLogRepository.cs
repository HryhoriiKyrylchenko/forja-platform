namespace Forja.Domain.Repositories.Analytics;

/// <summary>
/// Interface to manage AuditLog data.
/// </summary>
public interface IAuditLogRepository
{
    /// <summary>
    /// Retrieves an audit log by its ID.
    /// </summary>
    /// <param name="id">The ID of the audit log.</param>
    /// <returns>The audit log if found, null otherwise.</returns>
    Task<AuditLog?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all audit logs.
    /// </summary>
    /// <returns>A list of all audit logs.</returns>
    Task<IEnumerable<AuditLog>> GetAllAsync();

    /// <summary>
    /// Retrieves audit logs by user ID.
    /// </summary>
    /// <param name="userId">The user ID associated with the logs.</param>
    /// <returns>A list of audit logs for the specified user.</returns>
    Task<IEnumerable<AuditLog>> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Adds a new audit log to the database.
    /// </summary>
    /// <param name="auditLog">The audit log to add.</param>
    Task<AuditLog?> AddAsync(AuditLog auditLog);

    /// <summary>
    /// Updates an existing audit log.
    /// </summary>
    /// <param name="auditLog">The audit log to update.</param>
    Task<AuditLog?> UpdateAsync(AuditLog auditLog);

    /// <summary>
    /// Deletes an audit log by its ID.
    /// </summary>
    /// <param name="id">The ID of the audit log to delete.</param>
    Task DeleteAsync(Guid id);
}