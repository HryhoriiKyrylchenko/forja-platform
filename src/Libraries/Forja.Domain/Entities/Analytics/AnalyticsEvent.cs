namespace Forja.Domain.Entities.Analytics;

/// <summary>
/// Represents an analytics event in the system.
/// </summary>
[Table("AnalyticsEvents", Schema = "analytics")]
public class AnalyticsEvent
{
    /// <summary>
    /// Gets or sets the unique identifier for the analytics event.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the type of the analytics event.
    /// </summary>
    [Required]
    public AnalyticEventType EventType { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user associated with the event.
    /// </summary>
    [ForeignKey("User")]
    public Guid? UserId { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the event occurred.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the metadata associated with the event.
    /// </summary>
    public string Metadata { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the user associated with the event.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual User? User { get; set; }
}