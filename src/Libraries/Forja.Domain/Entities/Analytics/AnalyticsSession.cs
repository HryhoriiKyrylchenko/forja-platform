namespace Forja.Domain.Entities.Analytics;

/// <summary>
/// Represents an analytics session.
/// </summary>
[Table("AnalyticsSessions", Schema = "analytics")]
public class AnalyticsSession
{
    /// <summary>
    /// Gets or sets the unique identifier for the session.
    /// </summary>
    [Key]
    public Guid SessionId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the user associated with the session.
    /// </summary>
    [ForeignKey("User")]
    public Guid? UserId { get; set; }

    /// <summary>
    /// Gets or sets the start time of the session.
    /// </summary>
    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the end time of the session.
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Gets or sets the metadata associated with the session.
    /// </summary>
    public string Metadata { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the user associated with the session.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual User? User { get; set; }
}