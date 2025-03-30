namespace Forja.Application.Services.Analytics;

/// <summary>
/// Custom audit logging service that implements ILogger for seamless logging of audit log records.
/// </summary>
public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _auditLogRepository;

    public AuditLogService(IAuditLogRepository auditLogRepository)
    {
        _auditLogRepository = auditLogRepository;
    }

    /// <inheritdoc />
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (state == null || formatter == null)
        {
            throw new ArgumentNullException(nameof(state), "Log state and formatter cannot be null.");
        }

        var message = formatter(state, exception);
        
        var auditActionType = MapLogLevelToAuditActionType(logLevel, message, exception);

        var auditEntityType = MapStateToEntityType(state);

        var entityId = GetEntityIdFromState(state);
        
        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            EntityType = auditEntityType,
            EntityId = entityId,
            ActionType = auditActionType,
            UserId = null,
            ActionDate = DateTime.UtcNow,
            Details = new Dictionary<string, string>
            {
                { "LogLevel", logLevel.ToString() },
                { "Message", message },
                { "EventId", eventId.ToString() },
                { "Exception", exception?.ToString() ?? "N/A" }
            }
        };

        Task.Run(() => _auditLogRepository.AddAsync(auditLog)).Wait();
    }

    /// <inheritdoc />
    public async Task LogWithLogEntryAsync<TState>(LogEntry<TState> logEntry)
    {
        if (logEntry == null)
        {
            throw new ArgumentNullException(nameof(logEntry), "Log entry cannot be null.");
        }

        if (!IsEnabled(logEntry.LogLevel))
        {
            return;
        }

        var details = logEntry.Details ?? new Dictionary<string, string>();
        details["LogLevel"] = logEntry.LogLevel.ToString();
        details["Exception"] = logEntry.Exception?.ToString() ?? "N/A";

        var auditLogEntity = new AuditLog
        {
            Id = Guid.NewGuid(),
            EntityType = logEntry.EntityType,
            EntityId = GetEntityIdFromState(logEntry.State),
            ActionType = logEntry.ActionType,
            UserId = logEntry.UserId,
            ActionDate = DateTime.UtcNow,
            Details = details
        };

        await _auditLogRepository.AddAsync(auditLogEntity);
    }
    
    /// <inheritdoc />
    public async Task<List<AuditLogDto>> GetAllLogsAsync()
    {
        var logs = await _auditLogRepository.GetAllAsync();
        return logs.Select(AnalyticsEntityToDtoMapper.MapToAuditLogDto).ToList();
    }
    
    /// <inheritdoc />
    public async Task<List<AuditLogDto>> GetLogsByFilterAsync(
        Guid? userId = null,
        AuditEntityType? entityType = null,
        AuditActionType? actionType = null)
    {
        var logs = await _auditLogRepository.GetAllAsync();

        if (userId.HasValue)
        {
            logs = logs.Where(log => log.UserId == userId.Value);
        }

        if (entityType.HasValue)
        {
            logs = logs.Where(log => log.EntityType == entityType.Value);
        }

        if (actionType.HasValue)
        {
            logs = logs.Where(log => log.ActionType == actionType.Value);
        }

        return logs.Select(AnalyticsEntityToDtoMapper.MapToAuditLogDto).ToList();
    }
    
    /// <inheritdoc />
    public async Task DeleteLogAsync(Guid logId)
    {
        if (logId == Guid.Empty)
        {
            throw new ArgumentException("Invalid log ID provided.", nameof(logId));
        }

        await _auditLogRepository.DeleteAsync(logId);
    }

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= LogLevel.Information;
    }

    /// <inheritdoc />
    IDisposable ILogger.BeginScope<TState>(TState state)
    {
        return NullScope.Instance;
    }

    private static AuditActionType MapLogLevelToAuditActionType(LogLevel logLevel, string? message = null, Exception? exception = null)
    {
        if (exception is not null)
        {
            return exception switch
            {
                ValidationException => AuditActionType.ValidationError,
                DbUpdateException => AuditActionType.DatabaseError,
                HttpRequestException => AuditActionType.ApiError,
                AuthenticationException => AuditActionType.AuthenticationError,
                ApplicationException => AuditActionType.BusinessLogicError,
                _ => AuditActionType.SystemError,
            };
        }

        if (!string.IsNullOrEmpty(message))
        {
            if (message.Contains("created", StringComparison.OrdinalIgnoreCase)) return AuditActionType.Create;
            if (message.Contains("updated", StringComparison.OrdinalIgnoreCase)) return AuditActionType.Update;
            if (message.Contains("deleted", StringComparison.OrdinalIgnoreCase)) return AuditActionType.Delete;
            if (message.Contains("logged in", StringComparison.OrdinalIgnoreCase)) return AuditActionType.Login;
            if (message.Contains("logged out", StringComparison.OrdinalIgnoreCase)) return AuditActionType.Logout;
            if (message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)) return AuditActionType.UnauthorizedAccess;
        }

        return logLevel switch
        {
            LogLevel.Trace => AuditActionType.View,
            LogLevel.Debug => AuditActionType.View,
            LogLevel.Information => AuditActionType.SystemTriggeredEvent,
            LogLevel.Warning => AuditActionType.SystemTriggeredEvent,
            LogLevel.Error => AuditActionType.SystemError,
            LogLevel.Critical => AuditActionType.SystemError,
            _ => AuditActionType.Miscellaneous
        };
    }
    
    private static AuditEntityType MapStateToEntityType<TState>(TState state)
    {
        if (state is User) return AuditEntityType.User;
        if (state is UserProfileDto) return AuditEntityType.User;
        if (state is Product) return AuditEntityType.Product;
        if (state is ProductDto) return AuditEntityType.Product;
        if (state is Order) return AuditEntityType.Order;
        if (state is OrderDto) return AuditEntityType.Order;
        if (state is Payment) return AuditEntityType.Payment;
        if (state is PaymentDto) return AuditEntityType.Payment;
        
        return AuditEntityType.Other;
    }

    private Guid GetEntityIdFromState<TState>(TState state)
    {
        if (state is User user) return user.Id;
        if (state is UserProfileDto userProfile) return userProfile.Id;
        if (state is Product product) return product.Id;
        if (state is ProductDto productDto) return productDto.Id;
        if (state is Order order) return order.Id;
        if (state is OrderDto orderDto) return orderDto.Id;
        if (state is Payment payment) return payment.Id;
        if (state is PaymentDto paymentDto) return paymentDto.Id;
        
        return Guid.Empty;
    }

    /// <summary>
    /// A dummy scope implementation for ILogger that does nothing.
    /// </summary>
    private class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();
        public void Dispose() { }
    }
}