namespace Forja.Domain.Entities.Analytics;

/// <summary>
/// Represents an audit log entry in the system.
/// </summary>
[Table("AuditLogs", Schema = "analytics")]
public class AuditLog
{
    /// <summary>
    /// Gets or sets the unique identifier for the audit log entry.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the type of the entity being audited.
    /// </summary>
    [Required]
    public AuditEntityType EntityType { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the entity being audited.
    /// </summary>
    [Required]
    public Guid EntityId { get; set; }

    /// <summary>
    /// Gets or sets the type of action being audited.
    /// </summary>
    [Required]
    public AuditActionType ActionType { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who performed the action.
    /// </summary>
    [ForeignKey("User")]
    public Guid? UserId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the action was performed.
    /// </summary>
    public DateTime ActionDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets additional details about the action.
    /// </summary>
    public string Details { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user who performed the action.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual User? User { get; set; }
}