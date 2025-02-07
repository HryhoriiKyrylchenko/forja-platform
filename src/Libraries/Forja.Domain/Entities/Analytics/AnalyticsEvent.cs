namespace Forja.Domain.Entities.Analytics;

[Table("AnalyticsEvents", Schema = "analytics")]
public class AnalyticsEvent
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public AnalyticEventType EventType { get; set; }

    [ForeignKey("User")]
    public Guid? UserId { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.Now;

    public string Metadata { get; set; } = string.Empty;
    
    public virtual User? User { get; set; }
}