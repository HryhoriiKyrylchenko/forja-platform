namespace Forja.Application.Logging;

public class LogEntry<TState>
{
    public required TState State { get; set; }
    public Guid? UserId { get; set; } 
    public Exception? Exception { get; set; }
    public AuditActionType ActionType { get; set; }
    public AuditEntityType EntityType { get; set; }
    public LogLevel LogLevel { get; set; }
    public Dictionary<string, string>? Details { get; set; }
}