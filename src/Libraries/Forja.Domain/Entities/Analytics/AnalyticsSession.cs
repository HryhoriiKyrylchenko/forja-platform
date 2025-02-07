namespace Forja.Domain.Entities.Analytics;

[Table("AnalyticsSessions", Schema = "analytics")]
public class AnalyticsSession
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid SessionId { get; set; }

    [ForeignKey("User")]
    public Guid? UserId { get; set; }

    public DateTime StartTime { get; set; } = DateTime.Now;

    public DateTime? EndTime { get; set; }

    public string Metadata { get; set; } = string.Empty;
    
    public virtual User? User { get; set; }
}