namespace Forja.Application.Interfaces.Analytics;

/// <summary>
/// Defines a service for handling operations related to audit logs.
/// </summary>
public interface IAuditLogService : ILogger
{
    /// <summary>
    /// Logs a message or event along with its associated state, user information, and other metadata using a <see cref="LogEntry{TState}"/> object.
    /// </summary>
    /// <typeparam name="TState">
    /// The type of the state object that contains information about the log entry.
    /// </typeparam>
    /// <param name="logEntry">
    /// A <see cref="LogEntry{TState}"/> object containing the log state, user ID, log level, entity type, action type, exception details, and additional metadata.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    Task LogWithLogEntryAsync<TState>(LogEntry<TState> logEntry);
    
    /// <summary>
    /// Retrieves all audit logs.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an enumerable collection of <see cref="AuditLog"/> objects.
    /// </returns>
    Task<List<AuditLogDto>> GetAllLogsAsync();

    /// <summary>
    /// Retrieves audit logs based on the specified filter criteria.
    /// </summary>
    /// <param name="userId">The identifier of the user associated with the logs to filter. Pass null to ignore this filter.</param>
    /// <param name="entityType">The type of entity to filter logs by. Pass null to ignore this filter.</param>
    /// <param name="actionType">The type of action to filter logs by. Pass null to ignore this filter.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an enumerable collection of <see cref="AuditLog"/> objects that meet the filter criteria.
    /// </returns>
    Task<List<AuditLogDto>> GetLogsByFilterAsync(
        Guid? userId = null,
        AuditEntityType? entityType = null,
        AuditActionType? actionType = null);

    /// <summary>
    /// Deletes an audit log entry specified by its unique identifier.
    /// </summary>
    /// <param name="logId">
    /// The unique identifier of the audit log entry to delete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    Task DeleteLogAsync(Guid logId);
}