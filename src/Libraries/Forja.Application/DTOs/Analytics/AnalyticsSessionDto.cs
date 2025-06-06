namespace Forja.Application.DTOs.Analytics;

public class AnalyticsSessionDto
{
    public Guid SessionId { get; set; }
    public Guid? UserId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = [];
}